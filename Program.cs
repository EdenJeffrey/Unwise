namespace Unwise
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(new MainForm());
        }
    }
}

//! =TO DO LIST=
//? -BUGS-
//? - Cannot de-select tree node when application is windowed at certain resolutions
//? - Volume / pitch properties aren't applied from up the tree, only on the AudioContainer being played
//? - Check for consistency between container name / tag and node name / label etc.
//=======================================================================================================
//TODO +FEATURES+
//TODO - Add sequence containers
//TODO - Add switch containers
//TODO - Change pitch-bending algorithmn
//TODO - Add waveform generation on audio import, and waveform displaying playback bar
//TODO - Add seek to playback bar
//TODO - Change container volume control from percentage to decibel offset
//TODO - Add project saving