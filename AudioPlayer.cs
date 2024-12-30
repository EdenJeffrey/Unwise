using NAudio.Wave;
using NAudio.Wave.SampleProviders;

namespace Unwise
{
    public static class AudioConversions
    {
        public static float dBToMultipier(float dB)
        {
            return (float)Math.Pow(10.0, dB / 20.0);
        }
        public static float centsToMultipier(float cents)
        {
            return (float)Math.Pow(2.0, cents / 1200.0);
        }
    }

    public class AudioPlayer
    {
        private IWavePlayer wavePlayer;
        private AudioFileReader audioFileReader;
        private VolumeSampleProvider volumeProvider;
        private SmbPitchShiftingSampleProvider pitchProvider;
        private string currentFilePath;

        public float Volume { get; set; }
        public float Pitch { get; set; }

        public AudioPlayer()
        {
            wavePlayer = new WaveOutEvent();
            Volume = 0.0f;
            Pitch = 0.0f;
        }

        public struct AudioPlaybackData
        {
            public string FilePath;
            public float Volume;
            public float Pitch;

            public AudioPlaybackData(string filePath, float volume = 0.0f, float pitch = 0.0f)
            {
                FilePath = filePath;
                Volume = volume;
                Pitch = pitch;
            }
        }

        // Getter function to retrieve resolved audio data from the selected node
        public AudioPlaybackData GetAudioData(TreeNode selectedNode)
        {
            // Null checks
            if (selectedNode == null || selectedNode?.Tag is not Container)
            {
                return new AudioPlaybackData(string.Empty, 0.0f, 0.0f);
            }

            // Try to get audio container from selected node. Setup resolved container
            var container = GetContainerFromNode(selectedNode).GetContainerToPlay();
            AudioContainer resolvedAudioContainer = null;

            // Check if selected node is an audio container, resolve from tree if not
            if (container is AudioContainer audioContainer)
            {
                resolvedAudioContainer = audioContainer;
            }
            else if (container is MultiContainer multiContainer)
            {
                resolvedAudioContainer = ResolveNestedAudioContainer(multiContainer);
            }

            // if the resolved container is valid, get file path and other properties from audio container
            if (resolvedAudioContainer != null)
            {
                string filePath = resolvedAudioContainer.AudioFilePath;

                if (string.IsNullOrEmpty(filePath))
                {
                    return new AudioPlaybackData(string.Empty, 0.0f, 0.0f);
                }

                currentFilePath = filePath;
                float resolvedVolume = resolvedAudioContainer.Volume;
                float resolvedPitch = resolvedAudioContainer.Pitch;
                return new AudioPlaybackData(currentFilePath, resolvedVolume, resolvedPitch);
            }
            return new AudioPlaybackData(string.Empty, 0.0f, 0.0f);
        }

        // Helper function to resolved nested multi containers
        private AudioContainer ResolveNestedAudioContainer(MultiContainer multiContainer)
        {
            if (multiContainer == null)
            {
                throw new ArgumentNullException(nameof(multiContainer));
            }

            // Recursively get the container to play
            Container containerToPlay = multiContainer.GetContainerToPlay();

            if (containerToPlay is AudioContainer audioContainer)
            {
                return audioContainer; // Found a valid AudioContainer
            }
            else if (containerToPlay is MultiContainer nestedMultiContainer)
            {
                // Recursively resolve the nested MultiContainer
                return ResolveNestedAudioContainer(nestedMultiContainer);
            }
            return null;
        }

        // Helper function to retrieve container class, if valid, from a tree node
        private Container GetContainerFromNode(TreeNode node)
        {
            if (node?.Tag is Container container)
            {
                return container;
            }
            else return new Container(string.Empty);
        }

        // Loads audio file and properties in NAudio
        private void LoadAudioFile(string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException("Audio file not found.", filePath);

            audioFileReader = new AudioFileReader(filePath);

            // Apply volume control
            volumeProvider = new VolumeSampleProvider(audioFileReader.ToSampleProvider())
            {
                Volume = Volume
            };

            // Apply pitch shifting
            pitchProvider = new SmbPitchShiftingSampleProvider(volumeProvider)
            {
                PitchFactor = Pitch
            };

            wavePlayer.Init(pitchProvider);
        }

        public void PlayAudio(TreeNode selectedNode)
        {
            if (selectedNode == null || selectedNode?.Tag is not Container) return;

            // If the audio is paused, resume
            if (IsPaused())
            {
                wavePlayer.Play();
                return;
            }

            StopAudio(); // Kill existing playback
            AudioPlaybackData audioPlaybackData = GetAudioData(selectedNode);

            if (string.IsNullOrEmpty(audioPlaybackData.FilePath)) return; //Check audio container has audio data

            // Set properties and play
            SetVolume(AudioConversions.dBToMultipier(audioPlaybackData.Volume));
            SetPitch(AudioConversions.centsToMultipier(audioPlaybackData.Pitch));
            LoadAudioFile(audioPlaybackData.FilePath);
            wavePlayer.Play();
        }

        public void PauseAudio()
        {
            wavePlayer.Pause();
        }

        public void StopAudio()
        {
            if (audioFileReader != null)
            {
                audioFileReader.Dispose();
                audioFileReader = null;
            }

            wavePlayer.Stop();
        }

        public bool IsPlaying()
        {
            return wavePlayer.PlaybackState == PlaybackState.Playing;
        }

        public bool IsPaused()
        {
            return wavePlayer.PlaybackState == PlaybackState.Paused;
        }

        public void SetVolume(float volume)
        {
            Volume = Math.Clamp(volume, 0.0f, 1.0f); // Ensure volume is between 0 and 1
            if (volumeProvider != null)
            {
                volumeProvider.Volume = Volume;
            }
        }

        public void SetPitch(float pitch)
        {
            Pitch = pitch;
            if (pitchProvider != null)
            {
                pitchProvider.PitchFactor = Pitch;
            }
        }
    }
}
