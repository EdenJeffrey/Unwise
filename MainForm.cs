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
            // Initialize the event handler
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
    }
}
