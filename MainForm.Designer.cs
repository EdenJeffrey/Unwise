namespace Unwise
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            transport = new GroupBox();
            progressBar1 = new ProgressBar();
            transportStop = new Button();
            transportPause = new Button();
            transportPlay = new Button();
            hierarchyTreeMain = new TreeView();
            propertyGridMain = new PropertyGrid();
            MenuStrip = new MenuStrip();
            MenuStripFile = new ToolStripMenuItem();
            FileNew = new ToolStripMenuItem();
            FileOpen = new ToolStripMenuItem();
            FileSaveAs = new ToolStripMenuItem();
            MenuStripEdit = new ToolStripMenuItem();
            EditRename = new ToolStripMenuItem();
            transport.SuspendLayout();
            MenuStrip.SuspendLayout();
            SuspendLayout();
            // 
            // transport
            // 
            transport.Controls.Add(progressBar1);
            transport.Controls.Add(transportStop);
            transport.Controls.Add(transportPause);
            transport.Controls.Add(transportPlay);
            transport.Dock = DockStyle.Bottom;
            transport.Location = new Point(0, 399);
            transport.Name = "transport";
            transport.Size = new Size(800, 51);
            transport.TabIndex = 0;
            transport.TabStop = false;
            transport.Text = "Transport";
            // 
            // progressBar1
            // 
            progressBar1.Location = new Point(161, 22);
            progressBar1.Name = "progressBar1";
            progressBar1.Size = new Size(627, 23);
            progressBar1.TabIndex = 3;
            // 
            // transportStop
            // 
            transportStop.Location = new Point(111, 22);
            transportStop.Name = "transportStop";
            transportStop.Size = new Size(44, 23);
            transportStop.TabIndex = 2;
            transportStop.Text = "Stop";
            transportStop.UseVisualStyleBackColor = true;
            transportStop.Click += transportStop_Click;
            // 
            // transportPause
            // 
            transportPause.Location = new Point(54, 22);
            transportPause.Name = "transportPause";
            transportPause.Size = new Size(51, 23);
            transportPause.TabIndex = 1;
            transportPause.Text = "Pause";
            transportPause.UseVisualStyleBackColor = true;
            transportPause.Click += transportPause_Click;
            // 
            // transportPlay
            // 
            transportPlay.Location = new Point(6, 22);
            transportPlay.Name = "transportPlay";
            transportPlay.Size = new Size(42, 23);
            transportPlay.TabIndex = 0;
            transportPlay.Text = "Play";
            transportPlay.UseVisualStyleBackColor = true;
            transportPlay.Click += transportPlay_Click;
            // 
            // hierarchyTreeMain
            // 
            hierarchyTreeMain.AllowDrop = true;
            hierarchyTreeMain.HideSelection = false;
            hierarchyTreeMain.LabelEdit = true;
            hierarchyTreeMain.Location = new Point(0, 27);
            hierarchyTreeMain.Name = "hierarchyTreeMain";
            hierarchyTreeMain.Size = new Size(249, 372);
            hierarchyTreeMain.TabIndex = 1;
            // 
            // propertyGridMain
            // 
            propertyGridMain.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            propertyGridMain.HelpVisible = false;
            propertyGridMain.Location = new Point(573, 27);
            propertyGridMain.Name = "propertyGridMain";
            propertyGridMain.Size = new Size(227, 372);
            propertyGridMain.TabIndex = 2;
            propertyGridMain.ToolbarVisible = false;
            // 
            // MenuStrip
            // 
            MenuStrip.Items.AddRange(new ToolStripItem[] { MenuStripFile, MenuStripEdit });
            MenuStrip.Location = new Point(0, 0);
            MenuStrip.Name = "MenuStrip";
            MenuStrip.Size = new Size(800, 24);
            MenuStrip.TabIndex = 3;
            MenuStrip.Text = "MenuStrip";
            // 
            // MenuStripFile
            // 
            MenuStripFile.DropDownItems.AddRange(new ToolStripItem[] { FileNew, FileOpen, FileSaveAs });
            MenuStripFile.Name = "MenuStripFile";
            MenuStripFile.Size = new Size(37, 20);
            MenuStripFile.Text = "File";
            // 
            // FileNew
            // 
            FileNew.Name = "FileNew";
            FileNew.Size = new Size(180, 22);
            FileNew.Text = "New";
            FileNew.Click += FileNew_Click;
            // 
            // FileOpen
            // 
            FileOpen.Name = "FileOpen";
            FileOpen.Size = new Size(180, 22);
            FileOpen.Text = "Open";
            FileOpen.Click += FileOpen_Click;
            // 
            // FileSaveAs
            // 
            FileSaveAs.Name = "FileSaveAs";
            FileSaveAs.Size = new Size(180, 22);
            FileSaveAs.Text = "Save As";
            FileSaveAs.Click += FileSaveAs_Click;
            // 
            // MenuStripEdit
            // 
            MenuStripEdit.DropDownItems.AddRange(new ToolStripItem[] { EditRename });
            MenuStripEdit.Name = "MenuStripEdit";
            MenuStripEdit.Size = new Size(39, 20);
            MenuStripEdit.Text = "Edit";
            // 
            // EditRename
            // 
            EditRename.Name = "EditRename";
            EditRename.Size = new Size(180, 22);
            EditRename.Text = "Rename";
            EditRename.Click += EditRename_Click;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(propertyGridMain);
            Controls.Add(hierarchyTreeMain);
            Controls.Add(transport);
            Controls.Add(MenuStrip);
            MainMenuStrip = MenuStrip;
            Name = "MainForm";
            Text = "MainForm";
            transport.ResumeLayout(false);
            MenuStrip.ResumeLayout(false);
            MenuStrip.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private GroupBox transport;
        private Button transportPlay;
        private ProgressBar progressBar1;
        private Button transportStop;
        private Button transportPause;
        private TreeView hierarchyTree;
        private PropertyGrid propertyGridMain;
        private MenuStrip MenuStrip;
        private ToolStripMenuItem MenuStripFile;
        private ToolStripMenuItem FileNew;
        private ToolStripMenuItem FileOpen;
        private ToolStripMenuItem FileSaveAs;
        private ToolStripMenuItem MenuStripEdit;
        private ToolStripMenuItem EditRename;
    }
}