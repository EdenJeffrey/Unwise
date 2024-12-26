using System.Reflection;

namespace Unwise
{
    // Containers event functions for UI interactions, will be split into classes per UI feature, maybe? idk
    public class EventHandler
    {
        private readonly TreeView _hierarchyTree;
        private readonly PropertyGrid _propertyGrid;
        private readonly ContextMenuStrip _contextMenu;

        // Constructor that links private handler variables to their Form counterparts
        public EventHandler(TreeView hierarchyTree, ContextMenuStrip contextMenu, PropertyGrid propertyGrid)
        {
            _hierarchyTree = hierarchyTree;
            _contextMenu = contextMenu;
            _propertyGrid = propertyGrid;

            // Attach events
            _hierarchyTree.ItemDrag += HierarchyTree_ItemDrag;
            _hierarchyTree.DragEnter += HierarchyTree_DragEnter;
            _hierarchyTree.DragDrop += HierarchyTree_DragDrop;
            _hierarchyTree.NodeMouseClick += HierarchyTree_NodeMouseClick;
            _hierarchyTree.MouseDown += HierarchyTree_MouseDown;
            _hierarchyTree.KeyDown += HierarchyTree_KeyDown;
            _hierarchyTree.AfterLabelEdit += HierarchyTree_AfterLabelEdit;
            _contextMenu.Opening += ContextMenu_Opening;
            _hierarchyTree.AfterSelect += HierarchyTree_AfterSelect;

        }

        // When hierarchy node / item / file is dragged across the tree
        private void HierarchyTree_ItemDrag(object sender, ItemDragEventArgs e)
        {
            _hierarchyTree.DoDragDrop(e.Item, DragDropEffects.Move);
        }

        // When a dragged item enters over another hierachy node / item etc.
        private void HierarchyTree_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(TreeNode)))
            {
                e.Effect = DragDropEffects.Move; // Allow moving tree nodes
            }
            else if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy; // Allow file import
            }
            else
            {
                e.Effect = DragDropEffects.None; // Disallow other data types
            }
        }

        // fucntion to handle when an item is dropped into the hierarchy
        private void HierarchyTree_DragDrop(object sender, DragEventArgs e)
        {
            // determine if a tree node is dragged, creating a reference if so
            if (e.Data.GetData(typeof(TreeNode)) is TreeNode draggedNode)
            {
                // Drop point
                Point pt = _hierarchyTree.PointToClient(new Point(e.X, e.Y));
                TreeNode targetNode = _hierarchyTree.GetNodeAt(pt);

                if (targetNode != null && draggedNode != targetNode && !IsChildNode(draggedNode, targetNode))
                {
                    // Check if the target node is an audio container
                    if (targetNode?.Tag is AudioContainer)
                    {
                        // Don't allow anything to be dragged onto "AudioContainer" class (audio containers should be the lowest point on a tree)
                        return;
                    }
                    else if (targetNode?.Tag is MultiContainer multiContainer)
                    {
                        if (draggedNode?.Tag is Container container)
                        {
                            // If the parent of a dragged node is a multi container, remove that node from parent to prevent duplicated references
                            var parentNode = draggedNode.Parent;
                            if (parentNode?.Tag is MultiContainer parentMultiContainer)
                            {
                                parentMultiContainer.RemoveContainer(container);
                            }

                            // Add the dragged container to the target MultiContainer
                            multiContainer.AddContainer(container);
                            draggedNode.Remove();
                            targetNode.Nodes.Add(draggedNode);
                            targetNode.Expand();
                        }
                    }
                }
                else if (targetNode == null)
                {
                    var parentNode = draggedNode.Parent;
                    if (draggedNode?.Tag is Container container)
                    {
                        if (parentNode?.Tag is MultiContainer multiContainer)
                        {
                            multiContainer.RemoveContainer(container);
                        }
                    }
                    // If dropped into the empty tree, move the dragged node to the tree root
                    draggedNode.Remove();
                    _hierarchyTree.Nodes.Add(draggedNode);
                }
            }
            else if (e.Data.GetData(DataFormats.FileDrop) is string[] filePaths)
            {
                AudioImport(sender, e, filePaths);
            }
        }

        private void HierarchyTree_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            // If tree is right clicked display context menu
            if (e.Button == MouseButtons.Right)
            {
                _hierarchyTree.SelectedNode = e.Node;
                _contextMenu.Show(_hierarchyTree, e.Location);
            }
        }

        private void HierarchyTree_MouseDown(object sender, MouseEventArgs e)
        {
            Point pt = _hierarchyTree.PointToClient(new Point(e.X, e.Y));
            if (!_hierarchyTree.ClientRectangle.Contains(pt)) return;

            if (e.Button == MouseButtons.Left || e.Button == MouseButtons.Right)
            {
                // Deselect currently selected node if empty area on tree clicked
                var clickedNode = _hierarchyTree.GetNodeAt(e.Location);
                if (clickedNode == null)
                {
                    _hierarchyTree.SelectedNode = null;
                }
            }
        }

        //Hotkeys for existing event actions
        private void HierarchyTree_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                if (_hierarchyTree.SelectedNode != null)
                {
                    DeleteContainer(_hierarchyTree.SelectedNode);
                }
            }

            if (e.KeyCode == Keys.F2)
            {
                if (_hierarchyTree.SelectedNode != null)
                {
                    RenameContainer(_hierarchyTree.SelectedNode);
                }
            }

            if (e.KeyCode == Keys.F3)
            {
                string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Test.xml");
                Serialization.SerializeTreeViewToXml(_hierarchyTree, path);
            }

            if (e.KeyCode == Keys.F4)
            {
                string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Test.xml");
                Serialization.DeserializeTreeViewFromXml(_hierarchyTree, path);
            }
        }

        // Dynamically populate context menu for hierarchy tree dependent on what is clicked
        private void ContextMenu_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _contextMenu.Items.Clear();

            if (_hierarchyTree.SelectedNode == null)
            {
                // Add "Create Container" option with submenu for child classes, excluding containers meant to be inherited only
                var createContainerItem = new ToolStripMenuItem("Create Container");
                var childClasses = GetChildClassesOfContainer().Where(c =>
                    c != typeof(Container) &&
                    c != typeof(MultiContainer) &&
                    c.IsSubclassOf(typeof(Container)) // Ensure derivatives are included
                ).ToList();

                // Populate the context menu with items
                foreach (var childClass in childClasses)
                {
                    var submenuItem = new ToolStripMenuItem(childClass.Name, null, (s, args) => CreateContainer(childClass));
                    createContainerItem.DropDownItems.Add(submenuItem);
                }
                _contextMenu.Items.Add(createContainerItem);
            }
            else if (_hierarchyTree.SelectedNode?.Tag is AudioContainer)
            {
                // Only allow audio containers to be renamed or deleted, not given childen
                _contextMenu.Items.Add("Rename", null, (s, args) => RenameContainer(_hierarchyTree.SelectedNode));
                _contextMenu.Items.Add("Delete", null, (s, args) => DeleteContainer(_hierarchyTree.SelectedNode));
            }
            else
            {
                // Add "Create Child" option with submenu for child classes
                var createChildItem = new ToolStripMenuItem("Create Child");
                var childClasses = GetChildClassesOfContainer().Where(c =>
                    c != typeof(Container) &&
                    c != typeof(MultiContainer) &&
                    c.IsSubclassOf(typeof(Container)) // Ensure derivatives are included
                ).ToList();

                foreach (var childClass in childClasses)
                {
                    var submenuItem = new ToolStripMenuItem(childClass.Name, null, (s, args) => CreateChild(_hierarchyTree.SelectedNode, childClass));
                    createChildItem.DropDownItems.Add(submenuItem);
                }
                _contextMenu.Items.Add(createChildItem);

                // Add other options
                _contextMenu.Items.Add("Rename", null, (s, args) => RenameContainer(_hierarchyTree.SelectedNode));
                _contextMenu.Items.Add("Delete", null, (s, args) => DeleteContainer(_hierarchyTree.SelectedNode));
            }
        }


        private void HierarchyTree_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            if (e.Label != null)
            {
                // Ensure the new label is not empty and validate if necessary
                if (string.IsNullOrWhiteSpace(e.Label))
                {
                    e.Node.Text = e.Node.Tag.ToString();
                }
                else
                {
                    var container = e.Node.Tag as Container;
                    if (container != null)
                    {
                        container.Name = e.Label; // Update the name in the container object
                    }
                }
            }
        }


        private void HierarchyTree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeNode selectedNode = e.Node;

            // Check if the selected node has container, update the property grid to show the selected container's properties
            if (selectedNode?.Tag is Container selectedContainer)
            {
                if (_propertyGrid != null)
                {
                    _propertyGrid.SelectedObject = selectedContainer;
                }
            }
            else
            {
                // Clear the PropertyGrid if nothing is selected or invalid
                if (_propertyGrid != null)
                {
                    _propertyGrid.SelectedObject = null;
                }
            }
        }

        private void CreateContainer(Type containerType)
        {
            var container = (Container)Activator.CreateInstance(containerType, "New " + containerType.Name);
            var node = new TreeNode(container.Name) { Tag = container };
            _hierarchyTree.Nodes.Add(node);
        }

        private void CreateChild(TreeNode parentNode, Type containerType)
        {
            if (parentNode == null) return;
            var container = (Container)Activator.CreateInstance(containerType, "New " + containerType.Name);
            var childNode = new TreeNode(container.Name) { Tag = container };
            parentNode.Nodes.Add(childNode);
            parentNode.Expand();
            if (parentNode.Tag is MultiContainer multiContainer)
            {
                multiContainer.AddContainer(container);
            }
        }

        private void RenameContainer(TreeNode node)
        {
            if (node != null)
            {
                node.BeginEdit();
            }
        }

        private void DeleteContainer(TreeNode node)
        {
            if (node == null) return;

            if (node.Parent?.Tag is MultiContainer parentMultiContainer)
            {
                if (node.Tag is Container container)
                {
                    parentMultiContainer.RemoveContainer(container);
                }
            }
            node.Remove();
        }

        private bool IsChildNode(TreeNode parent, TreeNode child)
        {
            while (child.Parent != null)
            {
                if (child.Parent == parent)
                    return true;

                child = child.Parent;
            }
            return false;
        }

        private List<Type> GetChildClassesOfContainer()
        {
            // Get all types from the current assembly
            var assembly = Assembly.GetExecutingAssembly();
            var types = assembly.GetTypes();

            // Filter to get only classes that inherit from Container
            return types.Where(t => t.IsSubclassOf(typeof(Container)) && !t.IsAbstract).ToList();
        }

        // Audio import, handles container setup for imported audio files
        private void AudioImport(object sender, DragEventArgs e, string[] filePaths)
        {
            Point pt = _hierarchyTree.PointToClient(new Point(e.X, e.Y));
            TreeNode targetNode = _hierarchyTree.GetNodeAt(pt);

            // If the target node is an audio contaienr, populate for one audio file, ignore and create audio containers for multiple files
            if (targetNode?.Tag is AudioContainer targetAudioContainer)
            {
                if (filePaths.Length == 1)
                {
                    targetAudioContainer.AudioFilePath = filePaths[0];
                    targetAudioContainer.Name = Path.GetFileName(filePaths[0]);
                    targetNode.Name = Path.GetFileName(filePaths[0]);
                    targetNode.Text = Path.GetFileName(filePaths[0]);
                }
                else
                {
                    for (int i = 0; i < filePaths.Length; i++)
                    {
                        TreeNode newAudioContainer = CreateAudioContainer(filePaths[i]);
                        RenameContainer(newAudioContainer);
                    }
                }
            }
            else if (targetNode?.Tag is MultiContainer multiContainer)
            {
                // Add each dropped file as a new audio container under the existing multi container
                foreach (var filePath in filePaths)
                {
                    var audioContainer = (CreateAudioContainer(filePath, targetNode)?.Tag as AudioContainer);
                    multiContainer.AddContainer(audioContainer);
                }

                targetNode.Expand(); // Expand multi container node
            }
            else if (targetNode == null)
            {
                // If dropped into the empty tree, create new audio containers for each file at root
                foreach (var filePath in filePaths)
                {
                    string destinationPath = CopyFileToProjectDirectory(filePath);
                    CreateAudioContainer(destinationPath);
                }
            }
        }

        // Audio import helper, handles copying files from import location to project directory, sorting maybe be implemented later
        private string CopyFileToProjectDirectory(string filePath)
        {
            string projectDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string destinationDirectory = Path.Combine(projectDirectory, "AudioFiles");
            Directory.CreateDirectory(destinationDirectory); // Ensure directory exists

            string destinationPath = Path.Combine(destinationDirectory, Path.GetFileName(filePath));
            File.Copy(filePath, destinationPath, true); // Copy the file, overwriting if necessary
            return destinationPath;
        }

        // Audio container creation function, takes path and optional parent node if the container is being created as a child node.
        TreeNode CreateAudioContainer(string audioFilePath, TreeNode parentNode = null)
        {
            string fileName = Path.GetFileName(audioFilePath);

            // Create the AudioContainer using the given file path
            var audioContainer = new AudioContainer(fileName, audioFilePath);
            var node = new TreeNode(audioContainer.Name) { Tag = audioContainer };

            // Add the node to the appropriate place in the hierarchy
            if (parentNode == null) // If dropped on the tree itself
            {
                _hierarchyTree.Nodes.Add(node);
            }
            else // If dropped on a container node
            {
                parentNode.Nodes.Add(node);
                parentNode.Expand();
            }

            return node;
        }

    }
}
