namespace BESM3CA
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("Character");
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportToTextToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.printToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.printPreviewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.customizeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contentsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.indexToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.searchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.AttributeListBox = new System.Windows.Forms.ListBox();
            this.bnAdd = new System.Windows.Forms.Button();
            this.bnIncreaseLevel = new System.Windows.Forms.Button();
            this.CharacterTreeView = new System.Windows.Forms.TreeView();
            this.splitTopBottom = new System.Windows.Forms.SplitContainer();
            this.splitLeftRight = new System.Windows.Forms.SplitContainer();
            this.lbPositionAdj = new System.Windows.Forms.Label();
            this.lbLevelAdj = new System.Windows.Forms.Label();
            this.bnMoveDown = new System.Windows.Forms.Button();
            this.bnMoveUp = new System.Windows.Forms.Button();
            this.bnDecreaseLevel = new System.Windows.Forms.Button();
            this.lbVariant = new System.Windows.Forms.Label();
            this.bnDelete = new System.Windows.Forms.Button();
            this.FilterComboBox = new System.Windows.Forms.ComboBox();
            this.VariantListBox = new System.Windows.Forms.ListBox();
            this.SoulTextBox = new System.Windows.Forms.NumericUpDown();
            this.MindTextBox = new System.Windows.Forms.NumericUpDown();
            this.BodyTextBox = new System.Windows.Forms.NumericUpDown();
            this.DescriptionLabel = new System.Windows.Forms.Label();
            this.DescriptionTextBox = new System.Windows.Forms.TextBox();
            this.NotesLabel = new System.Windows.Forms.Label();
            this.PointCostLabel = new System.Windows.Forms.Label();
            this.PointCostTextBox = new System.Windows.Forms.TextBox();
            this.PointsPerLevelLabel = new System.Windows.Forms.Label();
            this.PointsPerLevelTextBox = new System.Windows.Forms.TextBox();
            this.DCVLabel = new System.Windows.Forms.Label();
            this.DCVTextBox = new System.Windows.Forms.TextBox();
            this.ACVTextBox = new System.Windows.Forms.TextBox();
            this.ACVLabel = new System.Windows.Forms.Label();
            this.EnergyLabel = new System.Windows.Forms.Label();
            this.EnergyTextBox = new System.Windows.Forms.TextBox();
            this.HealthTextBox = new System.Windows.Forms.TextBox();
            this.HealthLabel = new System.Windows.Forms.Label();
            this.LevelLabel = new System.Windows.Forms.Label();
            this.LevelTextBox = new System.Windows.Forms.TextBox();
            this.SoulLabel = new System.Windows.Forms.Label();
            this.MindLabel = new System.Windows.Forms.Label();
            this.BodyLabel = new System.Windows.Forms.Label();
            this.NotesTextBox = new System.Windows.Forms.TextBox();
            this.toolTipCheckThis = new System.Windows.Forms.ToolTip(this.components);
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitTopBottom)).BeginInit();
            this.splitTopBottom.Panel1.SuspendLayout();
            this.splitTopBottom.Panel2.SuspendLayout();
            this.splitTopBottom.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitLeftRight)).BeginInit();
            this.splitLeftRight.Panel1.SuspendLayout();
            this.splitLeftRight.Panel2.SuspendLayout();
            this.splitLeftRight.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SoulTextBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MindTextBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.BodyTextBox)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.menuStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.toolsToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(7, 2, 0, 2);
            this.menuStrip1.Size = new System.Drawing.Size(197, 33);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "mnMainMenuBar";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.openToolStripMenuItem,
            this.toolStripSeparator,
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.exportToTextToolStripMenuItem,
            this.toolStripSeparator1,
            this.printToolStripMenuItem,
            this.printPreviewToolStripMenuItem,
            this.toolStripSeparator2,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(54, 29);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("newToolStripMenuItem.Image")));
            this.newToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.newToolStripMenuItem.Size = new System.Drawing.Size(282, 34);
            this.newToolStripMenuItem.Text = "&New";
            this.newToolStripMenuItem.Click += new System.EventHandler(this.newToolStripMenuItem_Click);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("openToolStripMenuItem.Image")));
            this.openToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.openToolStripMenuItem.Size = new System.Drawing.Size(282, 34);
            this.openToolStripMenuItem.Text = "&Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // toolStripSeparator
            // 
            this.toolStripSeparator.Name = "toolStripSeparator";
            this.toolStripSeparator.Size = new System.Drawing.Size(279, 6);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("saveToolStripMenuItem.Image")));
            this.saveToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(282, 34);
            this.saveToolStripMenuItem.Text = "&Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(282, 34);
            this.saveAsToolStripMenuItem.Text = "Save &As";
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
            // 
            // exportToTextToolStripMenuItem
            // 
            this.exportToTextToolStripMenuItem.Name = "exportToTextToolStripMenuItem";
            this.exportToTextToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.T)));
            this.exportToTextToolStripMenuItem.Size = new System.Drawing.Size(282, 34);
            this.exportToTextToolStripMenuItem.Text = "Export to &Text";
            this.exportToTextToolStripMenuItem.Click += new System.EventHandler(this.exportToTextToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(279, 6);
            // 
            // printToolStripMenuItem
            // 
            this.printToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("printToolStripMenuItem.Image")));
            this.printToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.printToolStripMenuItem.Name = "printToolStripMenuItem";
            this.printToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.P)));
            this.printToolStripMenuItem.Size = new System.Drawing.Size(282, 34);
            this.printToolStripMenuItem.Text = "&Print";
            // 
            // printPreviewToolStripMenuItem
            // 
            this.printPreviewToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("printPreviewToolStripMenuItem.Image")));
            this.printPreviewToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.printPreviewToolStripMenuItem.Name = "printPreviewToolStripMenuItem";
            this.printPreviewToolStripMenuItem.Size = new System.Drawing.Size(282, 34);
            this.printPreviewToolStripMenuItem.Text = "Print Pre&view";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(279, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(282, 34);
            this.exitToolStripMenuItem.Text = "E&xit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.customizeToolStripMenuItem,
            this.optionsToolStripMenuItem});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(69, 29);
            this.toolsToolStripMenuItem.Text = "&Tools";
            // 
            // customizeToolStripMenuItem
            // 
            this.customizeToolStripMenuItem.Name = "customizeToolStripMenuItem";
            this.customizeToolStripMenuItem.Size = new System.Drawing.Size(197, 34);
            this.customizeToolStripMenuItem.Text = "&Customize";
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(197, 34);
            this.optionsToolStripMenuItem.Text = "&Options";
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.contentsToolStripMenuItem,
            this.indexToolStripMenuItem,
            this.searchToolStripMenuItem,
            this.toolStripSeparator5,
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(65, 29);
            this.helpToolStripMenuItem.Text = "&Help";
            // 
            // contentsToolStripMenuItem
            // 
            this.contentsToolStripMenuItem.Name = "contentsToolStripMenuItem";
            this.contentsToolStripMenuItem.Size = new System.Drawing.Size(185, 34);
            this.contentsToolStripMenuItem.Text = "&Contents";
            // 
            // indexToolStripMenuItem
            // 
            this.indexToolStripMenuItem.Name = "indexToolStripMenuItem";
            this.indexToolStripMenuItem.Size = new System.Drawing.Size(185, 34);
            this.indexToolStripMenuItem.Text = "&Index";
            // 
            // searchToolStripMenuItem
            // 
            this.searchToolStripMenuItem.Name = "searchToolStripMenuItem";
            this.searchToolStripMenuItem.Size = new System.Drawing.Size(185, 34);
            this.searchToolStripMenuItem.Text = "&Search";
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(182, 6);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(185, 34);
            this.aboutToolStripMenuItem.Text = "&About...";
            // 
            // AttributeListBox
            // 
            this.AttributeListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AttributeListBox.FormattingEnabled = true;
            this.AttributeListBox.IntegralHeight = false;
            this.AttributeListBox.ItemHeight = 25;
            this.AttributeListBox.Location = new System.Drawing.Point(6, 5);
            this.AttributeListBox.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.AttributeListBox.Name = "AttributeListBox";
            this.AttributeListBox.Size = new System.Drawing.Size(366, 662);
            this.AttributeListBox.TabIndex = 2;
            this.AttributeListBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.lbAttributeList_KeyPress);
            this.AttributeListBox.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lbAttributeList_MouseDoubleClick);
            // 
            // bnAdd
            // 
            this.bnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bnAdd.Location = new System.Drawing.Point(34, 739);
            this.bnAdd.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.bnAdd.Name = "bnAdd";
            this.bnAdd.Size = new System.Drawing.Size(159, 45);
            this.bnAdd.TabIndex = 3;
            this.bnAdd.Text = "Add";
            this.bnAdd.UseVisualStyleBackColor = true;
            this.bnAdd.Click += new System.EventHandler(this.bnAdd_Click);
            // 
            // bnIncreaseLevel
            // 
            this.bnIncreaseLevel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bnIncreaseLevel.Location = new System.Drawing.Point(821, 397);
            this.bnIncreaseLevel.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.bnIncreaseLevel.Name = "bnIncreaseLevel";
            this.bnIncreaseLevel.Size = new System.Drawing.Size(50, 45);
            this.bnIncreaseLevel.TabIndex = 4;
            this.bnIncreaseLevel.Text = "^";
            this.bnIncreaseLevel.UseVisualStyleBackColor = true;
            this.bnIncreaseLevel.Click += new System.EventHandler(this.bnIncreaseLevel_Click);
            // 
            // CharacterTreeView
            // 
            this.CharacterTreeView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CharacterTreeView.HideSelection = false;
            this.CharacterTreeView.Location = new System.Drawing.Point(6, 5);
            this.CharacterTreeView.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.CharacterTreeView.Name = "CharacterTreeView";
            treeNode2.Name = "root";
            treeNode2.Text = "Character";
            this.CharacterTreeView.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode2});
            this.CharacterTreeView.Size = new System.Drawing.Size(769, 797);
            this.CharacterTreeView.TabIndex = 5;
            this.CharacterTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
            // 
            // splitTopBottom
            // 
            this.splitTopBottom.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitTopBottom.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splitTopBottom.Cursor = System.Windows.Forms.Cursors.HSplit;
            this.splitTopBottom.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitTopBottom.IsSplitterFixed = true;
            this.splitTopBottom.Location = new System.Drawing.Point(0, 38);
            this.splitTopBottom.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.splitTopBottom.Name = "splitTopBottom";
            this.splitTopBottom.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitTopBottom.Panel1
            // 
            this.splitTopBottom.Panel1.Controls.Add(this.splitLeftRight);
            this.splitTopBottom.Panel1MinSize = 0;
            // 
            // splitTopBottom.Panel2
            // 
            this.splitTopBottom.Panel2.Controls.Add(this.SoulTextBox);
            this.splitTopBottom.Panel2.Controls.Add(this.MindTextBox);
            this.splitTopBottom.Panel2.Controls.Add(this.BodyTextBox);
            this.splitTopBottom.Panel2.Controls.Add(this.DescriptionLabel);
            this.splitTopBottom.Panel2.Controls.Add(this.DescriptionTextBox);
            this.splitTopBottom.Panel2.Controls.Add(this.NotesLabel);
            this.splitTopBottom.Panel2.Controls.Add(this.PointCostLabel);
            this.splitTopBottom.Panel2.Controls.Add(this.PointCostTextBox);
            this.splitTopBottom.Panel2.Controls.Add(this.PointsPerLevelLabel);
            this.splitTopBottom.Panel2.Controls.Add(this.PointsPerLevelTextBox);
            this.splitTopBottom.Panel2.Controls.Add(this.DCVLabel);
            this.splitTopBottom.Panel2.Controls.Add(this.DCVTextBox);
            this.splitTopBottom.Panel2.Controls.Add(this.ACVTextBox);
            this.splitTopBottom.Panel2.Controls.Add(this.ACVLabel);
            this.splitTopBottom.Panel2.Controls.Add(this.EnergyLabel);
            this.splitTopBottom.Panel2.Controls.Add(this.EnergyTextBox);
            this.splitTopBottom.Panel2.Controls.Add(this.HealthTextBox);
            this.splitTopBottom.Panel2.Controls.Add(this.HealthLabel);
            this.splitTopBottom.Panel2.Controls.Add(this.LevelLabel);
            this.splitTopBottom.Panel2.Controls.Add(this.LevelTextBox);
            this.splitTopBottom.Panel2.Controls.Add(this.SoulLabel);
            this.splitTopBottom.Panel2.Controls.Add(this.MindLabel);
            this.splitTopBottom.Panel2.Controls.Add(this.BodyLabel);
            this.splitTopBottom.Panel2.Controls.Add(this.NotesTextBox);
            this.splitTopBottom.Panel2MinSize = 0;
            this.splitTopBottom.Size = new System.Drawing.Size(1416, 1043);
            this.splitTopBottom.SplitterDistance = 813;
            this.splitTopBottom.SplitterWidth = 8;
            this.splitTopBottom.TabIndex = 6;
            // 
            // splitLeftRight
            // 
            this.splitLeftRight.Cursor = System.Windows.Forms.Cursors.Default;
            this.splitLeftRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitLeftRight.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitLeftRight.IsSplitterFixed = true;
            this.splitLeftRight.Location = new System.Drawing.Point(0, 0);
            this.splitLeftRight.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.splitLeftRight.Name = "splitLeftRight";
            // 
            // splitLeftRight.Panel1
            // 
            this.splitLeftRight.Panel1.Controls.Add(this.lbPositionAdj);
            this.splitLeftRight.Panel1.Controls.Add(this.lbLevelAdj);
            this.splitLeftRight.Panel1.Controls.Add(this.bnMoveDown);
            this.splitLeftRight.Panel1.Controls.Add(this.bnMoveUp);
            this.splitLeftRight.Panel1.Controls.Add(this.CharacterTreeView);
            this.splitLeftRight.Panel1.Controls.Add(this.bnDecreaseLevel);
            this.splitLeftRight.Panel1.Controls.Add(this.bnIncreaseLevel);
            // 
            // splitLeftRight.Panel2
            // 
            this.splitLeftRight.Panel2.Controls.Add(this.lbVariant);
            this.splitLeftRight.Panel2.Controls.Add(this.bnDelete);
            this.splitLeftRight.Panel2.Controls.Add(this.FilterComboBox);
            this.splitLeftRight.Panel2.Controls.Add(this.bnAdd);
            this.splitLeftRight.Panel2.Controls.Add(this.VariantListBox);
            this.splitLeftRight.Panel2.Controls.Add(this.AttributeListBox);
            this.splitLeftRight.Size = new System.Drawing.Size(1412, 809);
            this.splitLeftRight.SplitterDistance = 909;
            this.splitLeftRight.SplitterWidth = 7;
            this.splitLeftRight.TabIndex = 0;
            // 
            // lbPositionAdj
            // 
            this.lbPositionAdj.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lbPositionAdj.AutoSize = true;
            this.lbPositionAdj.Location = new System.Drawing.Point(803, 115);
            this.lbPositionAdj.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lbPositionAdj.Name = "lbPositionAdj";
            this.lbPositionAdj.Size = new System.Drawing.Size(75, 25);
            this.lbPositionAdj.TabIndex = 8;
            this.lbPositionAdj.Text = "Position";
            // 
            // lbLevelAdj
            // 
            this.lbLevelAdj.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lbLevelAdj.AutoSize = true;
            this.lbLevelAdj.Location = new System.Drawing.Point(816, 365);
            this.lbLevelAdj.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lbLevelAdj.Name = "lbLevelAdj";
            this.lbLevelAdj.Size = new System.Drawing.Size(51, 25);
            this.lbLevelAdj.TabIndex = 7;
            this.lbLevelAdj.Text = "Level";
            // 
            // bnMoveDown
            // 
            this.bnMoveDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bnMoveDown.Location = new System.Drawing.Point(803, 233);
            this.bnMoveDown.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.bnMoveDown.Name = "bnMoveDown";
            this.bnMoveDown.Size = new System.Drawing.Size(84, 77);
            this.bnMoveDown.TabIndex = 6;
            this.bnMoveDown.Text = "Move Down";
            this.bnMoveDown.UseVisualStyleBackColor = true;
            this.bnMoveDown.Click += new System.EventHandler(this.bnMoveDown_Click);
            // 
            // bnMoveUp
            // 
            this.bnMoveUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bnMoveUp.Location = new System.Drawing.Point(803, 147);
            this.bnMoveUp.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.bnMoveUp.Name = "bnMoveUp";
            this.bnMoveUp.Size = new System.Drawing.Size(84, 77);
            this.bnMoveUp.TabIndex = 6;
            this.bnMoveUp.Text = "Move up";
            this.bnMoveUp.UseVisualStyleBackColor = true;
            this.bnMoveUp.Click += new System.EventHandler(this.bnMoveUp_Click);
            // 
            // bnDecreaseLevel
            // 
            this.bnDecreaseLevel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bnDecreaseLevel.Location = new System.Drawing.Point(821, 452);
            this.bnDecreaseLevel.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.bnDecreaseLevel.Name = "bnDecreaseLevel";
            this.bnDecreaseLevel.Size = new System.Drawing.Size(50, 45);
            this.bnDecreaseLevel.TabIndex = 6;
            this.bnDecreaseLevel.Text = "v";
            this.bnDecreaseLevel.UseVisualStyleBackColor = true;
            this.bnDecreaseLevel.Click += new System.EventHandler(this.bnDecreaseLevel_Click);
            // 
            // lbVariant
            // 
            this.lbVariant.AutoSize = true;
            this.lbVariant.Location = new System.Drawing.Point(6, 3);
            this.lbVariant.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lbVariant.Name = "lbVariant";
            this.lbVariant.Size = new System.Drawing.Size(70, 25);
            this.lbVariant.TabIndex = 9;
            this.lbVariant.Text = "Variant:";
            // 
            // bnDelete
            // 
            this.bnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bnDelete.Location = new System.Drawing.Point(216, 739);
            this.bnDelete.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.bnDelete.Name = "bnDelete";
            this.bnDelete.Size = new System.Drawing.Size(156, 45);
            this.bnDelete.TabIndex = 7;
            this.bnDelete.Text = "Delete";
            this.bnDelete.UseVisualStyleBackColor = true;
            this.bnDelete.Click += new System.EventHandler(this.bnDelete_Click);
            // 
            // FilterComboBox
            // 
            this.FilterComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.FilterComboBox.FormattingEnabled = true;
            this.FilterComboBox.Location = new System.Drawing.Point(6, 33);
            this.FilterComboBox.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.FilterComboBox.Name = "FilterComboBox";
            this.FilterComboBox.Size = new System.Drawing.Size(366, 33);
            this.FilterComboBox.TabIndex = 5;
            this.FilterComboBox.SelectedIndexChanged += new System.EventHandler(this.cbFilter_SelectedIndexChanged);
            // 
            // VariantListBox
            // 
            this.VariantListBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.VariantListBox.FormattingEnabled = true;
            this.VariantListBox.IntegralHeight = false;
            this.VariantListBox.ItemHeight = 25;
            this.VariantListBox.Location = new System.Drawing.Point(6, 33);
            this.VariantListBox.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.VariantListBox.Name = "VariantListBox";
            this.VariantListBox.Size = new System.Drawing.Size(366, 104);
            this.VariantListBox.TabIndex = 8;
            this.VariantListBox.SelectedIndexChanged += new System.EventHandler(this.lbVariantList_SelectedIndexChanged);
            // 
            // SoulTextBox
            // 
            this.SoulTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.SoulTextBox.Location = new System.Drawing.Point(1287, 40);
            this.SoulTextBox.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.SoulTextBox.Maximum = new decimal(new int[] {
            1024,
            0,
            0,
            0});
            this.SoulTextBox.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.SoulTextBox.Name = "SoulTextBox";
            this.SoulTextBox.Size = new System.Drawing.Size(114, 31);
            this.SoulTextBox.TabIndex = 26;
            this.SoulTextBox.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.SoulTextBox.ValueChanged += new System.EventHandler(this.tbSoul_ValueChanged);
            this.SoulTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.tbSoul_Validating);
            this.SoulTextBox.Validated += new System.EventHandler(this.tbSoul_Validated);
            // 
            // MindTextBox
            // 
            this.MindTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.MindTextBox.Location = new System.Drawing.Point(1128, 40);
            this.MindTextBox.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.MindTextBox.Maximum = new decimal(new int[] {
            1024,
            0,
            0,
            0});
            this.MindTextBox.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.MindTextBox.Name = "MindTextBox";
            this.MindTextBox.Size = new System.Drawing.Size(114, 31);
            this.MindTextBox.TabIndex = 25;
            this.MindTextBox.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.MindTextBox.ValueChanged += new System.EventHandler(this.tbMind_ValueChanged);
            this.MindTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.tbMind_Validating);
            this.MindTextBox.Validated += new System.EventHandler(this.tbMind_Validated);
            // 
            // BodyTextBox
            // 
            this.BodyTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BodyTextBox.Location = new System.Drawing.Point(978, 40);
            this.BodyTextBox.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.BodyTextBox.Maximum = new decimal(new int[] {
            1024,
            0,
            0,
            0});
            this.BodyTextBox.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.BodyTextBox.Name = "BodyTextBox";
            this.BodyTextBox.Size = new System.Drawing.Size(114, 31);
            this.BodyTextBox.TabIndex = 24;
            this.BodyTextBox.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.BodyTextBox.ValueChanged += new System.EventHandler(this.tbBody_ValueChanged);
            this.BodyTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.tbBody_Validating);
            this.BodyTextBox.Validated += new System.EventHandler(this.tbBody_Validated);
            // 
            // DescriptionLabel
            // 
            this.DescriptionLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.DescriptionLabel.AutoSize = true;
            this.DescriptionLabel.Location = new System.Drawing.Point(598, 90);
            this.DescriptionLabel.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.DescriptionLabel.Name = "DescriptionLabel";
            this.DescriptionLabel.Size = new System.Drawing.Size(102, 25);
            this.DescriptionLabel.TabIndex = 23;
            this.DescriptionLabel.Text = "Description";
            // 
            // DescriptionTextBox
            // 
            this.DescriptionTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.DescriptionTextBox.Enabled = false;
            this.DescriptionTextBox.Location = new System.Drawing.Point(594, 120);
            this.DescriptionTextBox.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.DescriptionTextBox.Name = "DescriptionTextBox";
            this.DescriptionTextBox.Size = new System.Drawing.Size(801, 31);
            this.DescriptionTextBox.TabIndex = 22;
            // 
            // NotesLabel
            // 
            this.NotesLabel.AutoSize = true;
            this.NotesLabel.Location = new System.Drawing.Point(6, 8);
            this.NotesLabel.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.NotesLabel.Name = "NotesLabel";
            this.NotesLabel.Size = new System.Drawing.Size(59, 25);
            this.NotesLabel.TabIndex = 21;
            this.NotesLabel.Text = "Notes";
            // 
            // PointCostLabel
            // 
            this.PointCostLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.PointCostLabel.AutoSize = true;
            this.PointCostLabel.Location = new System.Drawing.Point(836, 8);
            this.PointCostLabel.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.PointCostLabel.Name = "PointCostLabel";
            this.PointCostLabel.Size = new System.Drawing.Size(93, 25);
            this.PointCostLabel.TabIndex = 20;
            this.PointCostLabel.Text = "Point Cost";
            // 
            // PointCostTextBox
            // 
            this.PointCostTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.PointCostTextBox.Enabled = false;
            this.PointCostTextBox.Location = new System.Drawing.Point(840, 40);
            this.PointCostTextBox.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.PointCostTextBox.Name = "PointCostTextBox";
            this.PointCostTextBox.Size = new System.Drawing.Size(97, 31);
            this.PointCostTextBox.TabIndex = 19;
            // 
            // PointsPerLevelLabel
            // 
            this.PointsPerLevelLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.PointsPerLevelLabel.AutoSize = true;
            this.PointsPerLevelLabel.Location = new System.Drawing.Point(690, 8);
            this.PointsPerLevelLabel.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.PointsPerLevelLabel.Name = "PointsPerLevelLabel";
            this.PointsPerLevelLabel.Size = new System.Drawing.Size(133, 25);
            this.PointsPerLevelLabel.TabIndex = 18;
            this.PointsPerLevelLabel.Text = "Points Per Level";
            // 
            // PointsPerLevelTextBox
            // 
            this.PointsPerLevelTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.PointsPerLevelTextBox.Enabled = false;
            this.PointsPerLevelTextBox.Location = new System.Drawing.Point(696, 40);
            this.PointsPerLevelTextBox.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.PointsPerLevelTextBox.Name = "PointsPerLevelTextBox";
            this.PointsPerLevelTextBox.Size = new System.Drawing.Size(133, 31);
            this.PointsPerLevelTextBox.TabIndex = 17;
            // 
            // DCVLabel
            // 
            this.DCVLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.DCVLabel.AutoSize = true;
            this.DCVLabel.Location = new System.Drawing.Point(796, 53);
            this.DCVLabel.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.DCVLabel.Name = "DCVLabel";
            this.DCVLabel.Size = new System.Drawing.Size(47, 25);
            this.DCVLabel.TabIndex = 16;
            this.DCVLabel.Text = "DCV";
            // 
            // DCVTextBox
            // 
            this.DCVTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.DCVTextBox.Enabled = false;
            this.DCVTextBox.Location = new System.Drawing.Point(800, 83);
            this.DCVTextBox.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.DCVTextBox.Name = "DCVTextBox";
            this.DCVTextBox.Size = new System.Drawing.Size(134, 31);
            this.DCVTextBox.TabIndex = 15;
            // 
            // ACVTextBox
            // 
            this.ACVTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ACVTextBox.Enabled = false;
            this.ACVTextBox.Location = new System.Drawing.Point(618, 83);
            this.ACVTextBox.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.ACVTextBox.Name = "ACVTextBox";
            this.ACVTextBox.Size = new System.Drawing.Size(141, 31);
            this.ACVTextBox.TabIndex = 14;
            // 
            // ACVLabel
            // 
            this.ACVLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ACVLabel.AutoSize = true;
            this.ACVLabel.Location = new System.Drawing.Point(614, 53);
            this.ACVLabel.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.ACVLabel.Name = "ACVLabel";
            this.ACVLabel.Size = new System.Drawing.Size(46, 25);
            this.ACVLabel.TabIndex = 13;
            this.ACVLabel.Text = "ACV";
            // 
            // EnergyLabel
            // 
            this.EnergyLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.EnergyLabel.AutoSize = true;
            this.EnergyLabel.Location = new System.Drawing.Point(1157, 83);
            this.EnergyLabel.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.EnergyLabel.Name = "EnergyLabel";
            this.EnergyLabel.Size = new System.Drawing.Size(66, 25);
            this.EnergyLabel.TabIndex = 12;
            this.EnergyLabel.Text = "Energy";
            // 
            // EnergyTextBox
            // 
            this.EnergyTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.EnergyTextBox.Enabled = false;
            this.EnergyTextBox.Location = new System.Drawing.Point(1161, 115);
            this.EnergyTextBox.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.EnergyTextBox.Name = "EnergyTextBox";
            this.EnergyTextBox.Size = new System.Drawing.Size(134, 31);
            this.EnergyTextBox.TabIndex = 11;
            // 
            // HealthTextBox
            // 
            this.HealthTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.HealthTextBox.Enabled = false;
            this.HealthTextBox.Location = new System.Drawing.Point(980, 115);
            this.HealthTextBox.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.HealthTextBox.Name = "HealthTextBox";
            this.HealthTextBox.Size = new System.Drawing.Size(141, 31);
            this.HealthTextBox.TabIndex = 10;
            // 
            // HealthLabel
            // 
            this.HealthLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.HealthLabel.AutoSize = true;
            this.HealthLabel.Location = new System.Drawing.Point(976, 83);
            this.HealthLabel.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.HealthLabel.Name = "HealthLabel";
            this.HealthLabel.Size = new System.Drawing.Size(63, 25);
            this.HealthLabel.TabIndex = 9;
            this.HealthLabel.Text = "Health";
            // 
            // LevelLabel
            // 
            this.LevelLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.LevelLabel.AutoSize = true;
            this.LevelLabel.Location = new System.Drawing.Point(594, 8);
            this.LevelLabel.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.LevelLabel.Name = "LevelLabel";
            this.LevelLabel.Size = new System.Drawing.Size(51, 25);
            this.LevelLabel.TabIndex = 8;
            this.LevelLabel.Text = "Level";
            // 
            // LevelTextBox
            // 
            this.LevelTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.LevelTextBox.Enabled = false;
            this.LevelTextBox.Location = new System.Drawing.Point(598, 40);
            this.LevelTextBox.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.LevelTextBox.Name = "LevelTextBox";
            this.LevelTextBox.Size = new System.Drawing.Size(84, 31);
            this.LevelTextBox.TabIndex = 7;
            // 
            // SoulLabel
            // 
            this.SoulLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.SoulLabel.AutoSize = true;
            this.SoulLabel.Location = new System.Drawing.Point(1281, 8);
            this.SoulLabel.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.SoulLabel.Name = "SoulLabel";
            this.SoulLabel.Size = new System.Drawing.Size(47, 25);
            this.SoulLabel.TabIndex = 6;
            this.SoulLabel.Text = "Soul";
            // 
            // MindLabel
            // 
            this.MindLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.MindLabel.AutoSize = true;
            this.MindLabel.Location = new System.Drawing.Point(1128, 8);
            this.MindLabel.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.MindLabel.Name = "MindLabel";
            this.MindLabel.Size = new System.Drawing.Size(53, 25);
            this.MindLabel.TabIndex = 5;
            this.MindLabel.Text = "Mind";
            // 
            // BodyLabel
            // 
            this.BodyLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BodyLabel.AutoSize = true;
            this.BodyLabel.Location = new System.Drawing.Point(976, 8);
            this.BodyLabel.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.BodyLabel.Name = "BodyLabel";
            this.BodyLabel.Size = new System.Drawing.Size(53, 25);
            this.BodyLabel.TabIndex = 4;
            this.BodyLabel.Text = "Body";
            // 
            // NotesTextBox
            // 
            this.NotesTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.NotesTextBox.Location = new System.Drawing.Point(6, 42);
            this.NotesTextBox.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.NotesTextBox.Multiline = true;
            this.NotesTextBox.Name = "NotesTextBox";
            this.NotesTextBox.Size = new System.Drawing.Size(580, 109);
            this.NotesTextBox.TabIndex = 0;
            this.NotesTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.tbNotes_Validating);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(1416, 1081);
            this.Controls.Add(this.splitTopBottom);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.MinimumSize = new System.Drawing.Size(1095, 1008);
            this.Name = "MainForm";
            this.Text = "BESM3CA";
            this.Load += new System.EventHandler(this.BESM3CA_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.splitTopBottom.Panel1.ResumeLayout(false);
            this.splitTopBottom.Panel2.ResumeLayout(false);
            this.splitTopBottom.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitTopBottom)).EndInit();
            this.splitTopBottom.ResumeLayout(false);
            this.splitLeftRight.Panel1.ResumeLayout(false);
            this.splitLeftRight.Panel1.PerformLayout();
            this.splitLeftRight.Panel2.ResumeLayout(false);
            this.splitLeftRight.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitLeftRight)).EndInit();
            this.splitLeftRight.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.SoulTextBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MindTextBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.BodyTextBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem printToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem printPreviewToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem customizeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem contentsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem indexToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem searchToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ListBox AttributeListBox;
        private System.Windows.Forms.Button bnAdd;
        private System.Windows.Forms.Button bnIncreaseLevel;
        private System.Windows.Forms.TreeView CharacterTreeView;
        private System.Windows.Forms.SplitContainer splitTopBottom;
        private System.Windows.Forms.SplitContainer splitLeftRight;
        private System.Windows.Forms.TextBox NotesTextBox;
        private System.Windows.Forms.ComboBox FilterComboBox;
        private System.Windows.Forms.Button bnDelete;
        private System.Windows.Forms.Button bnDecreaseLevel;
        private System.Windows.Forms.Label BodyLabel;
        private System.Windows.Forms.Label SoulLabel;
        private System.Windows.Forms.Label MindLabel;
        private System.Windows.Forms.ListBox VariantListBox;
        private System.Windows.Forms.Label lbVariant;
        private System.Windows.Forms.Label LevelLabel;
        private System.Windows.Forms.TextBox LevelTextBox;
        private System.Windows.Forms.Button bnMoveDown;
        private System.Windows.Forms.Button bnMoveUp;
        private System.Windows.Forms.Label HealthLabel;
        private System.Windows.Forms.TextBox HealthTextBox;
        private System.Windows.Forms.Label EnergyLabel;
        private System.Windows.Forms.TextBox EnergyTextBox;
        private System.Windows.Forms.Label DCVLabel;
        private System.Windows.Forms.TextBox DCVTextBox;
        private System.Windows.Forms.TextBox ACVTextBox;
        private System.Windows.Forms.Label ACVLabel;
        private System.Windows.Forms.ToolStripMenuItem exportToTextToolStripMenuItem;
        private System.Windows.Forms.Label PointsPerLevelLabel;
        private System.Windows.Forms.TextBox PointsPerLevelTextBox;
        private System.Windows.Forms.Label PointCostLabel;
        private System.Windows.Forms.TextBox PointCostTextBox;
        private System.Windows.Forms.Label NotesLabel;
        private System.Windows.Forms.Label lbPositionAdj;
        private System.Windows.Forms.Label lbLevelAdj;
        private System.Windows.Forms.ToolTip toolTipCheckThis;
        private System.Windows.Forms.TextBox DescriptionTextBox;
        private System.Windows.Forms.Label DescriptionLabel;
        private System.Windows.Forms.NumericUpDown BodyTextBox;
        private System.Windows.Forms.NumericUpDown MindTextBox;
        private System.Windows.Forms.NumericUpDown SoulTextBox;
    }
}

