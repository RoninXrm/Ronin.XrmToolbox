namespace Ronin.XrmToolbox.UserTimezoneManager.UI
{
    partial class UserTimezoneManagerControl
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
                components.Dispose();
            base.Dispose(disposing);
        }

        #region Designer generated code

        private void InitializeComponent()
        {
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.tsbClose = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator0 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbLoadUsers = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.lblViewDropdown = new System.Windows.Forms.ToolStripLabel();
            this.cmbViews = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbExportCsv = new System.Windows.Forms.ToolStripButton();
            this.pnlFilter = new System.Windows.Forms.Panel();
            this.txtFilter = new System.Windows.Forms.TextBox();
            this.lblFilter = new System.Windows.Forms.Label();
            this.pnlBulkUpdate = new System.Windows.Forms.Panel();
            this.btnApplyToSelected = new System.Windows.Forms.Button();
            this.cmbBulkTimezone = new System.Windows.Forms.ComboBox();
            this.lblBulkTimezone = new System.Windows.Forms.Label();
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.dgvUsers = new System.Windows.Forms.DataGridView();
            this.colSelect = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colFullName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEmailAddress = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCurrentTimezone = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNewTimezone = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.colApply = new System.Windows.Forms.DataGridViewButtonColumn();
            this.txtLog = new System.Windows.Forms.RichTextBox();
            this.toolStrip.SuspendLayout();
            this.pnlFilter.SuspendLayout();
            this.pnlBulkUpdate.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvUsers)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStrip
            // 
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                this.tsbClose,
                this.toolStripSeparator0,
                this.tsbLoadUsers,
                this.toolStripSeparator1,
                this.lblViewDropdown,
                this.cmbViews,
                this.toolStripSeparator2,
                this.tsbExportCsv});
            this.toolStrip.Location = new System.Drawing.Point(0, 0);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(900, 25);
            this.toolStrip.TabIndex = 0;
            // 
            // tsbClose
            // 
            this.tsbClose.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbClose.Name = "tsbClose";
            this.tsbClose.Size = new System.Drawing.Size(80, 22);
            this.tsbClose.Text = "Close Tool";
            this.tsbClose.Click += new System.EventHandler(this.tsbClose_Click);
            // 
            // toolStripSeparator0
            // 
            this.toolStripSeparator0.Name = "toolStripSeparator0";
            this.toolStripSeparator0.Size = new System.Drawing.Size(6, 25);
            // 
            // tsbLoadUsers
            // 
            this.tsbLoadUsers.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbLoadUsers.Name = "tsbLoadUsers";
            this.tsbLoadUsers.Size = new System.Drawing.Size(75, 22);
            this.tsbLoadUsers.Text = "Load Users";
            this.tsbLoadUsers.Click += new System.EventHandler(this.tsbLoadUsers_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // lblViewDropdown
            // 
            this.lblViewDropdown.Name = "lblViewDropdown";
            this.lblViewDropdown.Size = new System.Drawing.Size(33, 22);
            this.lblViewDropdown.Text = "View:";
            // 
            // cmbViews
            // 
            this.cmbViews.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbViews.Name = "cmbViews";
            this.cmbViews.Size = new System.Drawing.Size(220, 25);
            this.cmbViews.SelectedIndexChanged += new System.EventHandler(this.cmbViews_SelectedIndexChanged);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // tsbExportCsv
            // 
            this.tsbExportCsv.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbExportCsv.Name = "tsbExportCsv";
            this.tsbExportCsv.Size = new System.Drawing.Size(75, 22);
            this.tsbExportCsv.Text = "Export CSV";
            this.tsbExportCsv.Click += new System.EventHandler(this.tsbExportCsv_Click);
            // 
            // pnlFilter
            // 
            this.pnlFilter.Controls.Add(this.txtFilter);
            this.pnlFilter.Controls.Add(this.lblFilter);
            this.pnlFilter.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlFilter.Location = new System.Drawing.Point(0, 25);
            this.pnlFilter.Name = "pnlFilter";
            this.pnlFilter.Padding = new System.Windows.Forms.Padding(4);
            this.pnlFilter.Size = new System.Drawing.Size(900, 30);
            this.pnlFilter.TabIndex = 1;
            // 
            // lblFilter
            // 
            this.lblFilter.AutoSize = true;
            this.lblFilter.Location = new System.Drawing.Point(4, 8);
            this.lblFilter.Name = "lblFilter";
            this.lblFilter.Size = new System.Drawing.Size(72, 13);
            this.lblFilter.TabIndex = 0;
            this.lblFilter.Text = "Filter by Name:";
            // 
            // txtFilter
            // 
            this.txtFilter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFilter.Location = new System.Drawing.Point(84, 5);
            this.txtFilter.Name = "txtFilter";
            this.txtFilter.Size = new System.Drawing.Size(810, 20);
            this.txtFilter.TabIndex = 1;
            this.txtFilter.TextChanged += new System.EventHandler(this.txtFilter_TextChanged);
            // 
            // pnlBulkUpdate
            // 
            this.pnlBulkUpdate.Controls.Add(this.btnApplyToSelected);
            this.pnlBulkUpdate.Controls.Add(this.cmbBulkTimezone);
            this.pnlBulkUpdate.Controls.Add(this.lblBulkTimezone);
            this.pnlBulkUpdate.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlBulkUpdate.Location = new System.Drawing.Point(0, 55);
            this.pnlBulkUpdate.Name = "pnlBulkUpdate";
            this.pnlBulkUpdate.Padding = new System.Windows.Forms.Padding(4);
            this.pnlBulkUpdate.Size = new System.Drawing.Size(900, 34);
            this.pnlBulkUpdate.TabIndex = 2;
            // 
            // lblBulkTimezone
            // 
            this.lblBulkTimezone.AutoSize = true;
            this.lblBulkTimezone.Location = new System.Drawing.Point(4, 10);
            this.lblBulkTimezone.Name = "lblBulkTimezone";
            this.lblBulkTimezone.Size = new System.Drawing.Size(86, 13);
            this.lblBulkTimezone.TabIndex = 0;
            this.lblBulkTimezone.Text = "Bulk Timezone:";
            // 
            // cmbBulkTimezone
            // 
            this.cmbBulkTimezone.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBulkTimezone.FormattingEnabled = true;
            this.cmbBulkTimezone.Location = new System.Drawing.Point(96, 7);
            this.cmbBulkTimezone.Name = "cmbBulkTimezone";
            this.cmbBulkTimezone.Size = new System.Drawing.Size(300, 21);
            this.cmbBulkTimezone.TabIndex = 1;
            // 
            // btnApplyToSelected
            // 
            this.btnApplyToSelected.Location = new System.Drawing.Point(404, 6);
            this.btnApplyToSelected.Name = "btnApplyToSelected";
            this.btnApplyToSelected.Size = new System.Drawing.Size(130, 23);
            this.btnApplyToSelected.TabIndex = 2;
            this.btnApplyToSelected.Text = "Apply To Selected";
            this.btnApplyToSelected.UseVisualStyleBackColor = true;
            this.btnApplyToSelected.Click += new System.EventHandler(this.btnApplyToSelected_Click);
            // 
            // splitContainer
            // 
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer.Location = new System.Drawing.Point(0, 89);
            this.splitContainer.Name = "splitContainer";
            this.splitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer.Panel1 (grid)
            // 
            this.splitContainer.Panel1.Controls.Add(this.dgvUsers);
            // 
            // splitContainer.Panel2 (log)
            // 
            this.splitContainer.Panel2.Controls.Add(this.txtLog);
            this.splitContainer.Size = new System.Drawing.Size(900, 511);
            this.splitContainer.SplitterDistance = 350;
            this.splitContainer.TabIndex = 3;
            // 
            // dgvUsers
            // 
            this.dgvUsers.AllowUserToAddRows = false;
            this.dgvUsers.AllowUserToDeleteRows = false;
            this.dgvUsers.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.None;
            this.dgvUsers.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dgvUsers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvUsers.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
                this.colSelect,
                this.colFullName,
                this.colEmailAddress,
                this.colCurrentTimezone,
                this.colNewTimezone,
                this.colApply});
            this.dgvUsers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvUsers.MultiSelect = true;
            this.dgvUsers.Name = "dgvUsers";
            this.dgvUsers.ReadOnly = false;
            this.dgvUsers.RowHeadersWidth = 24;
            this.dgvUsers.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvUsers.Size = new System.Drawing.Size(900, 350);
            this.dgvUsers.TabIndex = 0;
            this.dgvUsers.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvUsers_CellClick);
            this.dgvUsers.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dgvUsers_DataError);
            // 
            // colSelect
            // 
            this.colSelect.DataPropertyName = "IsSelected";
            this.colSelect.HeaderText = "Select";
            this.colSelect.Name = "colSelect";
            this.colSelect.Width = 50;
            // 
            // colFullName
            // 
            this.colFullName.DataPropertyName = "FullName";
            this.colFullName.HeaderText = "Full Name";
            this.colFullName.Name = "colFullName";
            this.colFullName.ReadOnly = true;
            this.colFullName.Width = 180;
            // 
            // colEmailAddress
            // 
            this.colEmailAddress.DataPropertyName = "EmailAddress";
            this.colEmailAddress.HeaderText = "Email Address";
            this.colEmailAddress.Name = "colEmailAddress";
            this.colEmailAddress.ReadOnly = true;
            this.colEmailAddress.Width = 220;
            // 
            // colCurrentTimezone
            // 
            this.colCurrentTimezone.DataPropertyName = "CurrentTimezoneName";
            this.colCurrentTimezone.HeaderText = "Current Timezone";
            this.colCurrentTimezone.Name = "colCurrentTimezone";
            this.colCurrentTimezone.ReadOnly = true;
            this.colCurrentTimezone.Width = 200;
            // 
            // colNewTimezone
            // 
            this.colNewTimezone.DataPropertyName = "NewTimezoneCode";
            this.colNewTimezone.DisplayStyleForCurrentCellOnly = true;
            this.colNewTimezone.HeaderText = "New Timezone";
            this.colNewTimezone.Name = "colNewTimezone";
            this.colNewTimezone.Width = 200;
            // 
            // colApply
            // 
            this.colApply.HeaderText = "Action";
            this.colApply.Name = "colApply";
            this.colApply.Text = "Apply";
            this.colApply.UseColumnTextForButtonValue = true;
            this.colApply.Width = 70;
            // 
            // txtLog
            // 
            this.txtLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtLog.Name = "txtLog";
            this.txtLog.Size = new System.Drawing.Size(900, 157);
            this.txtLog.TabIndex = 0;
            this.txtLog.Text = "";
            // 
            // UserTimezoneManagerControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer);
            this.Controls.Add(this.pnlBulkUpdate);
            this.Controls.Add(this.pnlFilter);
            this.Controls.Add(this.toolStrip);
            this.Name = "UserTimezoneManagerControl";
            this.Size = new System.Drawing.Size(900, 600);
            this.Load += new System.EventHandler(this.UserTimezoneManagerControl_Load);
            this.OnCloseTool += new System.EventHandler(this.UserTimezoneManagerControl_OnCloseTool);
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.pnlFilter.ResumeLayout(false);
            this.pnlFilter.PerformLayout();
            this.pnlBulkUpdate.ResumeLayout(false);
            this.pnlBulkUpdate.PerformLayout();
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
            this.splitContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvUsers)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        // ── Designer fields ───────────────────────────────────────────────────────
        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.ToolStripButton tsbClose;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator0;
        private System.Windows.Forms.ToolStripButton tsbLoadUsers;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripLabel lblViewDropdown;
        private System.Windows.Forms.ToolStripComboBox cmbViews;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton tsbExportCsv;
        private System.Windows.Forms.Panel pnlFilter;
        private System.Windows.Forms.Label lblFilter;
        private System.Windows.Forms.TextBox txtFilter;
        private System.Windows.Forms.Panel pnlBulkUpdate;
        private System.Windows.Forms.Label lblBulkTimezone;
        private System.Windows.Forms.ComboBox cmbBulkTimezone;
        private System.Windows.Forms.Button btnApplyToSelected;
        private System.Windows.Forms.SplitContainer splitContainer;
        private System.Windows.Forms.DataGridView dgvUsers;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colSelect;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFullName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEmailAddress;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCurrentTimezone;
        private System.Windows.Forms.DataGridViewComboBoxColumn colNewTimezone;
        private System.Windows.Forms.DataGridViewButtonColumn colApply;
        private System.Windows.Forms.RichTextBox txtLog;
    }
}
