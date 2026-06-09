namespace Ronin.XrmToolbox
{
    partial class WebResourceMapperController
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur de composants

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WebResourceMapperController));
            this.toolStripMenu = new System.Windows.Forms.ToolStrip();
            this.tsbClose = new System.Windows.Forms.ToolStripButton();
            this.pnlTop = new System.Windows.Forms.Panel();
            this.btnLoadWebResources = new System.Windows.Forms.Button();
            this.cmbSolutions = new System.Windows.Forms.ComboBox();
            this.lblSolutions = new System.Windows.Forms.Label();
            this.pnlRootFolder = new System.Windows.Forms.Panel();
            this.flpActions = new System.Windows.Forms.FlowLayoutPanel();
            this.btnUpdateSelected = new System.Windows.Forms.Button();
            this.btnDownloadSelected = new System.Windows.Forms.Button();
            this.btnUploadNew = new System.Windows.Forms.Button();
            this.btnResetAutoMap = new System.Windows.Forms.Button();
            this.btnBrowseRootFolder = new System.Windows.Forms.Button();
            this.txtRootFolder = new System.Windows.Forms.TextBox();
            this.lblRootFolder = new System.Windows.Forms.Label();
            this.dgvWebResources = new System.Windows.Forms.DataGridView();
            this.colSelected = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDisplayName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLocalFilePath = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colBrowseFile = new System.Windows.Forms.DataGridViewButtonColumn();
            this.toolStripMenu.SuspendLayout();
            this.pnlTop.SuspendLayout();
            this.pnlRootFolder.SuspendLayout();
            this.flpActions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvWebResources)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStripMenu
            // 
            this.toolStripMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbClose});
            this.toolStripMenu.Location = new System.Drawing.Point(0, 0);
            this.toolStripMenu.Name = "toolStripMenu";
            this.toolStripMenu.Size = new System.Drawing.Size(800, 25);
            this.toolStripMenu.TabIndex = 0;
            // 
            // tsbClose
            // 
            this.tsbClose.Image = ((System.Drawing.Image)(resources.GetObject("tsbClose.Image")));
            this.tsbClose.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbClose.Name = "tsbClose";
            this.tsbClose.Size = new System.Drawing.Size(102, 22);
            this.tsbClose.Text = "Close this tool";
            this.tsbClose.Click += new System.EventHandler(this.tsbClose_Click);
            // 
            // pnlTop
            // 
            this.pnlTop.Controls.Add(this.btnLoadWebResources);
            this.pnlTop.Controls.Add(this.cmbSolutions);
            this.pnlTop.Controls.Add(this.lblSolutions);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Location = new System.Drawing.Point(0, 25);
            this.pnlTop.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Padding = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.pnlTop.Size = new System.Drawing.Size(800, 32);
            this.pnlTop.TabIndex = 1;
            // 
            // btnLoadWebResources
            // 
            this.btnLoadWebResources.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLoadWebResources.AutoSize = true;
            this.btnLoadWebResources.Image = ((System.Drawing.Image)(resources.GetObject("btnLoadWebResources.Image")));
            this.btnLoadWebResources.Location = new System.Drawing.Point(573, 6);
            this.btnLoadWebResources.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnLoadWebResources.MinimumSize = new System.Drawing.Size(0, 20);
            this.btnLoadWebResources.Name = "btnLoadWebResources";
            this.btnLoadWebResources.Padding = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.btnLoadWebResources.Size = new System.Drawing.Size(145, 23);
            this.btnLoadWebResources.TabIndex = 2;
            this.btnLoadWebResources.Text = "Load Web Resources";
            this.btnLoadWebResources.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnLoadWebResources.UseVisualStyleBackColor = true;
            this.btnLoadWebResources.Click += new System.EventHandler(this.btnLoadWebResources_Click);
            // 
            // cmbSolutions
            // 
            this.cmbSolutions.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSolutions.FormattingEnabled = true;
            this.cmbSolutions.Location = new System.Drawing.Point(56, 7);
            this.cmbSolutions.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.cmbSolutions.Name = "cmbSolutions";
            this.cmbSolutions.Size = new System.Drawing.Size(608, 21);
            this.cmbSolutions.TabIndex = 1;
            // 
            // lblSolutions
            // 
            this.lblSolutions.AutoSize = true;
            this.lblSolutions.Location = new System.Drawing.Point(7, 10);
            this.lblSolutions.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblSolutions.Name = "lblSolutions";
            this.lblSolutions.Size = new System.Drawing.Size(45, 13);
            this.lblSolutions.TabIndex = 0;
            this.lblSolutions.Text = "Solution";
            // 
            // pnlRootFolder
            // 
            this.pnlRootFolder.Controls.Add(this.flpActions);
            this.pnlRootFolder.Controls.Add(this.btnBrowseRootFolder);
            this.pnlRootFolder.Controls.Add(this.txtRootFolder);
            this.pnlRootFolder.Controls.Add(this.lblRootFolder);
            this.pnlRootFolder.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlRootFolder.Location = new System.Drawing.Point(0, 57);
            this.pnlRootFolder.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.pnlRootFolder.Name = "pnlRootFolder";
            this.pnlRootFolder.Padding = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.pnlRootFolder.Size = new System.Drawing.Size(800, 56);
            this.pnlRootFolder.TabIndex = 2;
            // 
            // flpActions
            // 
            this.flpActions.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.flpActions.AutoSize = true;
            this.flpActions.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flpActions.Controls.Add(this.btnUpdateSelected);
            this.flpActions.Controls.Add(this.btnDownloadSelected);
            this.flpActions.Controls.Add(this.btnUploadNew);
            this.flpActions.Controls.Add(this.btnResetAutoMap);
            this.flpActions.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flpActions.Location = new System.Drawing.Point(301, 30);
            this.flpActions.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.flpActions.Name = "flpActions";
            this.flpActions.Size = new System.Drawing.Size(494, 23);
            this.flpActions.TabIndex = 6;
            this.flpActions.WrapContents = false;
            // 
            // btnUpdateSelected
            // 
            this.btnUpdateSelected.AutoSize = true;
            this.btnUpdateSelected.Image = ((System.Drawing.Image)(resources.GetObject("btnUpdateSelected.Image")));
            this.btnUpdateSelected.Location = new System.Drawing.Point(373, 0);
            this.btnUpdateSelected.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.btnUpdateSelected.MinimumSize = new System.Drawing.Size(0, 20);
            this.btnUpdateSelected.Name = "btnUpdateSelected";
            this.btnUpdateSelected.Padding = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.btnUpdateSelected.Size = new System.Drawing.Size(121, 23);
            this.btnUpdateSelected.TabIndex = 5;
            this.btnUpdateSelected.Text = "Update Selected";
            this.btnUpdateSelected.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnUpdateSelected.UseVisualStyleBackColor = true;
            this.btnUpdateSelected.Click += new System.EventHandler(this.btnUpdateSelected_Click);
            // 
            // btnDownloadSelected
            // 
            this.btnDownloadSelected.AutoSize = true;
            this.btnDownloadSelected.Image = ((System.Drawing.Image)(resources.GetObject("btnDownloadSelected.Image")));
            this.btnDownloadSelected.Location = new System.Drawing.Point(236, 0);
            this.btnDownloadSelected.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.btnDownloadSelected.MinimumSize = new System.Drawing.Size(0, 20);
            this.btnDownloadSelected.Name = "btnDownloadSelected";
            this.btnDownloadSelected.Padding = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.btnDownloadSelected.Size = new System.Drawing.Size(134, 23);
            this.btnDownloadSelected.TabIndex = 3;
            this.btnDownloadSelected.Text = "Download Selected";
            this.btnDownloadSelected.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnDownloadSelected.UseVisualStyleBackColor = true;
            this.btnDownloadSelected.Click += new System.EventHandler(this.btnDownloadSelected_Click);
            // 
            // btnUploadNew
            // 
            this.btnUploadNew.AutoSize = true;
            this.btnUploadNew.Image = ((System.Drawing.Image)(resources.GetObject("btnUploadNew.Image")));
            this.btnUploadNew.Location = new System.Drawing.Point(133, 0);
            this.btnUploadNew.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.btnUploadNew.MinimumSize = new System.Drawing.Size(0, 20);
            this.btnUploadNew.Name = "btnUploadNew";
            this.btnUploadNew.Padding = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.btnUploadNew.Size = new System.Drawing.Size(100, 23);
            this.btnUploadNew.TabIndex = 4;
            this.btnUploadNew.Text = "Upload New";
            this.btnUploadNew.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnUploadNew.UseVisualStyleBackColor = true;
            this.btnUploadNew.Click += new System.EventHandler(this.btnUploadNew_Click);
            // 
            // btnResetAutoMap
            // 
            this.btnResetAutoMap.AutoSize = true;
            this.btnResetAutoMap.Image = ((System.Drawing.Image)(resources.GetObject("btnResetAutoMap.Image")));
            this.btnResetAutoMap.Location = new System.Drawing.Point(3, 0);
            this.btnResetAutoMap.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.btnResetAutoMap.MinimumSize = new System.Drawing.Size(0, 20);
            this.btnResetAutoMap.Name = "btnResetAutoMap";
            this.btnResetAutoMap.Padding = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.btnResetAutoMap.Size = new System.Drawing.Size(127, 23);
            this.btnResetAutoMap.TabIndex = 6;
            this.btnResetAutoMap.Text = "Reset + Auto Map";
            this.btnResetAutoMap.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnResetAutoMap.UseVisualStyleBackColor = true;
            this.btnResetAutoMap.Click += new System.EventHandler(this.btnResetAutoMap_Click);
            // 
            // btnBrowseRootFolder
            // 
            this.btnBrowseRootFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowseRootFolder.AutoSize = true;
            this.btnBrowseRootFolder.Image = ((System.Drawing.Image)(resources.GetObject("btnBrowseRootFolder.Image")));
            this.btnBrowseRootFolder.Location = new System.Drawing.Point(698, 5);
            this.btnBrowseRootFolder.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnBrowseRootFolder.MinimumSize = new System.Drawing.Size(0, 20);
            this.btnBrowseRootFolder.Name = "btnBrowseRootFolder";
            this.btnBrowseRootFolder.Padding = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.btnBrowseRootFolder.Size = new System.Drawing.Size(76, 23);
            this.btnBrowseRootFolder.TabIndex = 2;
            this.btnBrowseRootFolder.Text = "Browse";
            this.btnBrowseRootFolder.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnBrowseRootFolder.UseVisualStyleBackColor = true;
            this.btnBrowseRootFolder.Click += new System.EventHandler(this.btnBrowseRootFolder_Click);
            // 
            // txtRootFolder
            // 
            this.txtRootFolder.Location = new System.Drawing.Point(64, 8);
            this.txtRootFolder.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtRootFolder.Name = "txtRootFolder";
            this.txtRootFolder.Size = new System.Drawing.Size(656, 20);
            this.txtRootFolder.TabIndex = 1;
            this.txtRootFolder.TextChanged += new System.EventHandler(this.txtRootFolder_TextChanged);
            // 
            // lblRootFolder
            // 
            this.lblRootFolder.AutoSize = true;
            this.lblRootFolder.Location = new System.Drawing.Point(7, 10);
            this.lblRootFolder.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblRootFolder.Name = "lblRootFolder";
            this.lblRootFolder.Size = new System.Drawing.Size(59, 13);
            this.lblRootFolder.TabIndex = 0;
            this.lblRootFolder.Text = "Root folder";
            // 
            // dgvWebResources
            // 
            this.dgvWebResources.AllowUserToAddRows = false;
            this.dgvWebResources.AllowUserToDeleteRows = false;
            this.dgvWebResources.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvWebResources.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colSelected,
            this.colName,
            this.colDisplayName,
            this.colType,
            this.colLocalFilePath,
            this.colBrowseFile});
            this.dgvWebResources.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvWebResources.Location = new System.Drawing.Point(0, 113);
            this.dgvWebResources.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.dgvWebResources.Name = "dgvWebResources";
            this.dgvWebResources.RowHeadersWidth = 62;
            this.dgvWebResources.RowTemplate.Height = 28;
            this.dgvWebResources.Size = new System.Drawing.Size(800, 342);
            this.dgvWebResources.TabIndex = 3;
            this.dgvWebResources.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvWebResources_CellContentClick);
            // 
            // colSelected
            // 
            this.colSelected.DataPropertyName = "Selected";
            this.colSelected.HeaderText = "Select";
            this.colSelected.MinimumWidth = 8;
            this.colSelected.Name = "colSelected";
            this.colSelected.Width = 70;
            // 
            // colName
            // 
            this.colName.DataPropertyName = "Name";
            this.colName.HeaderText = "WebResource Name";
            this.colName.MinimumWidth = 8;
            this.colName.Name = "colName";
            this.colName.ReadOnly = true;
            this.colName.Width = 260;
            // 
            // colDisplayName
            // 
            this.colDisplayName.DataPropertyName = "DisplayName";
            this.colDisplayName.HeaderText = "Display Name";
            this.colDisplayName.MinimumWidth = 8;
            this.colDisplayName.Name = "colDisplayName";
            this.colDisplayName.ReadOnly = true;
            this.colDisplayName.Width = 210;
            // 
            // colType
            // 
            this.colType.DataPropertyName = "TypeLabel";
            this.colType.HeaderText = "Type";
            this.colType.MinimumWidth = 8;
            this.colType.Name = "colType";
            this.colType.ReadOnly = true;
            this.colType.Width = 140;
            // 
            // colLocalFilePath
            // 
            this.colLocalFilePath.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colLocalFilePath.DataPropertyName = "LocalFilePath";
            this.colLocalFilePath.HeaderText = "Local File Path";
            this.colLocalFilePath.MinimumWidth = 8;
            this.colLocalFilePath.Name = "colLocalFilePath";
            // 
            // colBrowseFile
            // 
            this.colBrowseFile.HeaderText = "Browse";
            this.colBrowseFile.MinimumWidth = 8;
            this.colBrowseFile.Name = "colBrowseFile";
            this.colBrowseFile.Text = "...";
            this.colBrowseFile.UseColumnTextForButtonValue = true;
            this.colBrowseFile.Width = 70;
            // 
            // WebResourceMapperController
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dgvWebResources);
            this.Controls.Add(this.pnlRootFolder);
            this.Controls.Add(this.pnlTop);
            this.Controls.Add(this.toolStripMenu);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "WebResourceMapperController";
            this.PluginIcon = ((System.Drawing.Icon)(resources.GetObject("$this.PluginIcon")));
            this.Size = new System.Drawing.Size(800, 455);
            this.TabIcon = ((System.Drawing.Image)(resources.GetObject("$this.TabIcon")));
            this.OnCloseTool += new System.EventHandler(this.MyPluginControl_OnCloseTool);
            this.Load += new System.EventHandler(this.MyPluginControl_Load);
            this.toolStripMenu.ResumeLayout(false);
            this.toolStripMenu.PerformLayout();
            this.pnlTop.ResumeLayout(false);
            this.pnlTop.PerformLayout();
            this.pnlRootFolder.ResumeLayout(false);
            this.pnlRootFolder.PerformLayout();
            this.flpActions.ResumeLayout(false);
            this.flpActions.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvWebResources)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStripMenu;
        private System.Windows.Forms.ToolStripButton tsbClose;
        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.ComboBox cmbSolutions;
        private System.Windows.Forms.Label lblSolutions;
        private System.Windows.Forms.Panel pnlRootFolder;
        private System.Windows.Forms.FlowLayoutPanel flpActions;
        private System.Windows.Forms.Button btnBrowseRootFolder;
        private System.Windows.Forms.TextBox txtRootFolder;
        private System.Windows.Forms.Label lblRootFolder;
        private System.Windows.Forms.Button btnLoadWebResources;
        private System.Windows.Forms.Button btnDownloadSelected;
        private System.Windows.Forms.Button btnUploadNew;
        private System.Windows.Forms.Button btnUpdateSelected;
        private System.Windows.Forms.Button btnResetAutoMap;
        private System.Windows.Forms.DataGridView dgvWebResources;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colSelected;
        private System.Windows.Forms.DataGridViewTextBoxColumn colName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDisplayName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colType;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLocalFilePath;
        private System.Windows.Forms.DataGridViewButtonColumn colBrowseFile;
    }
}
