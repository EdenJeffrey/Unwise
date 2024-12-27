using Unwise_Serialization;

namespace Unwise
{
    public partial class MainForm : Form
    {
        private TreeView hierarchyTreeMain;
        private PropertyGrid propertyGrid;
        private ContextMenuStrip contextMenu;
        private EventHandler eventHandler;
        private AudioPlayer audioPlayer;

        public MainForm()
        {
            InitializeComponent();
            InitializeHierarchyEditor();
            InitializePropertyGrid();
            InitializeEventHandler();
            InitializeAudioPlayer();
        }

        private void InitializeHierarchyEditor()
        {
            hierarchyTree = hierarchyTreeMain;
            contextMenu = new ContextMenuStrip();
            hierarchyTree.ContextMenuStrip = contextMenu;
            Controls.Add(hierarchyTree);
        }

        private void InitializePropertyGrid()
        {
            propertyGrid = propertyGridMain;
            Controls.Add(propertyGrid);
        }

        private void InitializeEventHandler()
        {
            eventHandler = new EventHandler(hierarchyTree, contextMenu, propertyGrid);
        }

        private void InitializeAudioPlayer()
        {
            audioPlayer = new AudioPlayer();
        }

        private void transportPlay_Click(object sender, EventArgs e)
        {
            audioPlayer.PlayAudio(hierarchyTree.SelectedNode);
            //System.Diagnostics.Debug.WriteLine("Play");
        }

        private void transportPause_Click(object sender, EventArgs e)
        {
            if (audioPlayer.IsPlaying())
            {
                audioPlayer.PauseAudio();
            }
        }

        private void transportStop_Click(object sender, EventArgs e)
        {
            audioPlayer.StopAudio();
        }

        private void FileNew_Click(object sender, EventArgs e)
        {
            hierarchyTree.Nodes.Clear();
        }

        private void FileOpen_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "XML Files (*.xml)|*.xml|All Files (*.*)|*.*";
                openFileDialog.Title = "Open File";
                openFileDialog.InitialDirectory = @"C:\";  // Set the initial directory

                // Show the dialog and check if the user clicked open
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = openFileDialog.FileName;
                    Serialization.DeserializeTreeViewFromXml(hierarchyTree, filePath);
                }
            }
        }

        private void FileSaveAs_Click(object sender, EventArgs e)
        {
            // Create and configure the SaveFileDialog
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "XML Files (*.xml)|*.xml|All Files (*.*)|*.*";
                saveFileDialog.Title = "Save As";
                saveFileDialog.FileName = "NewUnwiseProject.xml"; // Default file name

                // Show the dialog and check if the user clicked save as
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = saveFileDialog.FileName;  // Get the file path
                    Serialization.SerializeTreeViewToXml(hierarchyTree, filePath);
                }
            }

        }

        private void EditRename_Click(object sender, EventArgs e)
        {
            //MenuStripBehaviour.MenuStrip_EditRename();
            if (hierarchyTree.SelectedNode != null)
            {
                eventHandler.RenameContainer(hierarchyTree.SelectedNode);
            }
        }
    }
}
