namespace JVSLib_KB.Src.Forms
{
    partial class frmMain
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
            this.srpMain = new System.Windows.Forms.StatusStrip();
            this.lblConnectionState = new System.Windows.Forms.ToolStripStatusLabel();
            this.mnuMain = new System.Windows.Forms.MenuStrip();
            this.mnuFile = new System.Windows.Forms.ToolStripMenuItem();
            this.btnImportBindings = new System.Windows.Forms.ToolStripMenuItem();
            this.btnExportBindings = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuSep2 = new System.Windows.Forms.ToolStripSeparator();
            this.btnAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuSep1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnExit = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuBoard = new System.Windows.Forms.ToolStripMenuItem();
            this.btnConnect = new System.Windows.Forms.ToolStripMenuItem();
            this.btnDisconnect = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnGetFeatures = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnConnectOnStart = new System.Windows.Forms.ToolStripMenuItem();
            this.btnDoNotReset = new System.Windows.Forms.ToolStripMenuItem();
            this.btnNoFormEvents = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuPollingRate = new System.Windows.Forms.ToolStripMenuItem();
            this.btn50hz = new System.Windows.Forms.ToolStripMenuItem();
            this.btn100hz = new System.Windows.Forms.ToolStripMenuItem();
            this.btn125hz = new System.Windows.Forms.ToolStripMenuItem();
            this.btn200hz = new System.Windows.Forms.ToolStripMenuItem();
            this.btn500hz = new System.Windows.Forms.ToolStripMenuItem();
            this.btn1000hz = new System.Windows.Forms.ToolStripMenuItem();
            this.lstSwitchBindings = new System.Windows.Forms.ListView();
            this.clmJvsInput = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.clmKeybdOutput = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.mnuBindings = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.btnAddBinding = new System.Windows.Forms.ToolStripMenuItem();
            this.btnRemoveBinding = new System.Windows.Forms.ToolStripMenuItem();
            this.tcBindings = new System.Windows.Forms.TabControl();
            this.tabSwitchBindings = new System.Windows.Forms.TabPage();
            this.tabCoinBindings = new System.Windows.Forms.TabPage();
            this.lstCoinBindings = new System.Windows.Forms.ListView();
            this.clmCoinInput = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.clmCoinOutput = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tmrInput = new System.Windows.Forms.Timer(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.srpMain.SuspendLayout();
            this.mnuMain.SuspendLayout();
            this.mnuBindings.SuspendLayout();
            this.tcBindings.SuspendLayout();
            this.tabSwitchBindings.SuspendLayout();
            this.tabCoinBindings.SuspendLayout();
            this.SuspendLayout();
            // 
            // srpMain
            // 
            this.srpMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblConnectionState});
            this.srpMain.Location = new System.Drawing.Point(0, 428);
            this.srpMain.Name = "srpMain";
            this.srpMain.Size = new System.Drawing.Size(800, 22);
            this.srpMain.TabIndex = 0;
            this.srpMain.Text = "statusStrip1";
            // 
            // lblConnectionState
            // 
            this.lblConnectionState.Name = "lblConnectionState";
            this.lblConnectionState.Size = new System.Drawing.Size(79, 17);
            this.lblConnectionState.Text = "Disconnected";
            // 
            // mnuMain
            // 
            this.mnuMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuFile,
            this.mnuBoard,
            this.settingsToolStripMenuItem});
            this.mnuMain.Location = new System.Drawing.Point(0, 0);
            this.mnuMain.Name = "mnuMain";
            this.mnuMain.Size = new System.Drawing.Size(800, 24);
            this.mnuMain.TabIndex = 1;
            this.mnuMain.Text = "menuStrip1";
            // 
            // mnuFile
            // 
            this.mnuFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnImportBindings,
            this.btnExportBindings,
            this.mnuSep2,
            this.btnAbout,
            this.mnuSep1,
            this.btnExit});
            this.mnuFile.Name = "mnuFile";
            this.mnuFile.Size = new System.Drawing.Size(37, 20);
            this.mnuFile.Text = "File";
            // 
            // btnImportBindings
            // 
            this.btnImportBindings.Enabled = false;
            this.btnImportBindings.Name = "btnImportBindings";
            this.btnImportBindings.Size = new System.Drawing.Size(180, 22);
            this.btnImportBindings.Text = "Import bindings";
            this.btnImportBindings.Click += new System.EventHandler(this.btnImportBindings_Click);
            // 
            // btnExportBindings
            // 
            this.btnExportBindings.Enabled = false;
            this.btnExportBindings.Name = "btnExportBindings";
            this.btnExportBindings.Size = new System.Drawing.Size(180, 22);
            this.btnExportBindings.Text = "Export bindings";
            this.btnExportBindings.Click += new System.EventHandler(this.btnExportBindings_Click);
            // 
            // mnuSep2
            // 
            this.mnuSep2.Name = "mnuSep2";
            this.mnuSep2.Size = new System.Drawing.Size(177, 6);
            // 
            // btnAbout
            // 
            this.btnAbout.Name = "btnAbout";
            this.btnAbout.Size = new System.Drawing.Size(180, 22);
            this.btnAbout.Text = "About";
            this.btnAbout.Click += new System.EventHandler(this.btnAbout_Click);
            // 
            // mnuSep1
            // 
            this.mnuSep1.Name = "mnuSep1";
            this.mnuSep1.Size = new System.Drawing.Size(177, 6);
            // 
            // btnExit
            // 
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(180, 22);
            this.btnExit.Text = "Exit";
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // mnuBoard
            // 
            this.mnuBoard.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnConnect,
            this.btnDisconnect,
            this.toolStripSeparator1,
            this.btnGetFeatures});
            this.mnuBoard.Name = "mnuBoard";
            this.mnuBoard.Size = new System.Drawing.Size(50, 20);
            this.mnuBoard.Text = "Board";
            // 
            // btnConnect
            // 
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(150, 22);
            this.btnConnect.Text = "Connect";
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // btnDisconnect
            // 
            this.btnDisconnect.Name = "btnDisconnect";
            this.btnDisconnect.Size = new System.Drawing.Size(150, 22);
            this.btnDisconnect.Text = "Disconnect";
            this.btnDisconnect.Click += new System.EventHandler(this.btnDisconnect_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(147, 6);
            // 
            // btnGetFeatures
            // 
            this.btnGetFeatures.Enabled = false;
            this.btnGetFeatures.Name = "btnGetFeatures";
            this.btnGetFeatures.Size = new System.Drawing.Size(150, 22);
            this.btnGetFeatures.Text = "Get Board Info";
            this.btnGetFeatures.Click += new System.EventHandler(this.btnGetFeatures_Click);
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnConnectOnStart,
            this.btnDoNotReset,
            this.btnNoFormEvents,
            this.mnuPollingRate});
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.settingsToolStripMenuItem.Text = "Settings";
            // 
            // btnConnectOnStart
            // 
            this.btnConnectOnStart.CheckOnClick = true;
            this.btnConnectOnStart.Name = "btnConnectOnStart";
            this.btnConnectOnStart.Size = new System.Drawing.Size(225, 22);
            this.btnConnectOnStart.Text = "Connect on startup";
            this.btnConnectOnStart.Click += new System.EventHandler(this.btnConnectOnStart_Click);
            // 
            // btnDoNotReset
            // 
            this.btnDoNotReset.CheckOnClick = true;
            this.btnDoNotReset.Name = "btnDoNotReset";
            this.btnDoNotReset.Size = new System.Drawing.Size(225, 22);
            this.btnDoNotReset.Text = "Do not reset on connect";
            this.btnDoNotReset.Click += new System.EventHandler(this.btnDoNotReset_Click);
            // 
            // btnNoFormEvents
            // 
            this.btnNoFormEvents.Name = "btnNoFormEvents";
            this.btnNoFormEvents.Size = new System.Drawing.Size(225, 22);
            this.btnNoFormEvents.Text = "Do not prioritise form events";
            this.btnNoFormEvents.Visible = false;
            this.btnNoFormEvents.Click += new System.EventHandler(this.btnNoFormEvents_Click);
            // 
            // mnuPollingRate
            // 
            this.mnuPollingRate.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btn50hz,
            this.btn100hz,
            this.btn125hz,
            this.btn200hz,
            this.btn500hz,
            this.btn1000hz});
            this.mnuPollingRate.Name = "mnuPollingRate";
            this.mnuPollingRate.Size = new System.Drawing.Size(225, 22);
            this.mnuPollingRate.Text = "Polling rate";
            // 
            // btn50hz
            // 
            this.btn50hz.CheckOnClick = true;
            this.btn50hz.Name = "btn50hz";
            this.btn50hz.Size = new System.Drawing.Size(193, 22);
            this.btn50hz.Text = "50hz";
            this.btn50hz.Click += new System.EventHandler(this.btnhz_Click);
            // 
            // btn100hz
            // 
            this.btn100hz.CheckOnClick = true;
            this.btn100hz.Name = "btn100hz";
            this.btn100hz.Size = new System.Drawing.Size(193, 22);
            this.btn100hz.Text = "100hz (recommended)";
            this.btn100hz.Click += new System.EventHandler(this.btnhz_Click);
            // 
            // btn125hz
            // 
            this.btn125hz.CheckOnClick = true;
            this.btn125hz.Name = "btn125hz";
            this.btn125hz.Size = new System.Drawing.Size(193, 22);
            this.btn125hz.Text = "125hz";
            this.btn125hz.Click += new System.EventHandler(this.btnhz_Click);
            // 
            // btn200hz
            // 
            this.btn200hz.CheckOnClick = true;
            this.btn200hz.Name = "btn200hz";
            this.btn200hz.Size = new System.Drawing.Size(193, 22);
            this.btn200hz.Text = "200hz";
            this.btn200hz.Click += new System.EventHandler(this.btnhz_Click);
            // 
            // btn500hz
            // 
            this.btn500hz.CheckOnClick = true;
            this.btn500hz.Name = "btn500hz";
            this.btn500hz.Size = new System.Drawing.Size(193, 22);
            this.btn500hz.Text = "500hz";
            this.btn500hz.Click += new System.EventHandler(this.btnhz_Click);
            // 
            // btn1000hz
            // 
            this.btn1000hz.CheckOnClick = true;
            this.btn1000hz.Name = "btn1000hz";
            this.btn1000hz.Size = new System.Drawing.Size(193, 22);
            this.btn1000hz.Text = "1000hz";
            this.btn1000hz.Click += new System.EventHandler(this.btnhz_Click);
            // 
            // lstSwitchBindings
            // 
            this.lstSwitchBindings.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.clmJvsInput,
            this.clmKeybdOutput});
            this.lstSwitchBindings.ContextMenuStrip = this.mnuBindings;
            this.lstSwitchBindings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstSwitchBindings.FullRowSelect = true;
            this.lstSwitchBindings.GridLines = true;
            this.lstSwitchBindings.HideSelection = false;
            this.lstSwitchBindings.Location = new System.Drawing.Point(3, 3);
            this.lstSwitchBindings.Name = "lstSwitchBindings";
            this.lstSwitchBindings.Size = new System.Drawing.Size(786, 372);
            this.lstSwitchBindings.TabIndex = 2;
            this.lstSwitchBindings.UseCompatibleStateImageBehavior = false;
            this.lstSwitchBindings.View = System.Windows.Forms.View.Details;
            // 
            // clmJvsInput
            // 
            this.clmJvsInput.Text = "JVS Input";
            this.clmJvsInput.Width = 260;
            // 
            // clmKeybdOutput
            // 
            this.clmKeybdOutput.Text = "Keyboard Output";
            this.clmKeybdOutput.Width = 260;
            // 
            // mnuBindings
            // 
            this.mnuBindings.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnAddBinding,
            this.btnRemoveBinding});
            this.mnuBindings.Name = "mnuBindings";
            this.mnuBindings.Size = new System.Drawing.Size(162, 48);
            this.mnuBindings.Opening += new System.ComponentModel.CancelEventHandler(this.mnuBindings_Opening);
            // 
            // btnAddBinding
            // 
            this.btnAddBinding.Name = "btnAddBinding";
            this.btnAddBinding.Size = new System.Drawing.Size(161, 22);
            this.btnAddBinding.Text = "Add Binding";
            this.btnAddBinding.Click += new System.EventHandler(this.btnAddBinding_Click);
            // 
            // btnRemoveBinding
            // 
            this.btnRemoveBinding.Name = "btnRemoveBinding";
            this.btnRemoveBinding.Size = new System.Drawing.Size(161, 22);
            this.btnRemoveBinding.Text = "Remove Binding";
            this.btnRemoveBinding.Click += new System.EventHandler(this.btnRemoveBinding_Click);
            // 
            // tcBindings
            // 
            this.tcBindings.Controls.Add(this.tabSwitchBindings);
            this.tcBindings.Controls.Add(this.tabCoinBindings);
            this.tcBindings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tcBindings.Location = new System.Drawing.Point(0, 24);
            this.tcBindings.Name = "tcBindings";
            this.tcBindings.SelectedIndex = 0;
            this.tcBindings.Size = new System.Drawing.Size(800, 404);
            this.tcBindings.TabIndex = 3;
            // 
            // tabSwitchBindings
            // 
            this.tabSwitchBindings.Controls.Add(this.lstSwitchBindings);
            this.tabSwitchBindings.Location = new System.Drawing.Point(4, 22);
            this.tabSwitchBindings.Name = "tabSwitchBindings";
            this.tabSwitchBindings.Padding = new System.Windows.Forms.Padding(3);
            this.tabSwitchBindings.Size = new System.Drawing.Size(792, 378);
            this.tabSwitchBindings.TabIndex = 0;
            this.tabSwitchBindings.Text = "Switch Bindings";
            this.tabSwitchBindings.UseVisualStyleBackColor = true;
            // 
            // tabCoinBindings
            // 
            this.tabCoinBindings.Controls.Add(this.lstCoinBindings);
            this.tabCoinBindings.Location = new System.Drawing.Point(4, 22);
            this.tabCoinBindings.Name = "tabCoinBindings";
            this.tabCoinBindings.Padding = new System.Windows.Forms.Padding(3);
            this.tabCoinBindings.Size = new System.Drawing.Size(792, 378);
            this.tabCoinBindings.TabIndex = 1;
            this.tabCoinBindings.Text = "Coin Bindings";
            this.tabCoinBindings.UseVisualStyleBackColor = true;
            // 
            // lstCoinBindings
            // 
            this.lstCoinBindings.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.clmCoinInput,
            this.clmCoinOutput});
            this.lstCoinBindings.ContextMenuStrip = this.mnuBindings;
            this.lstCoinBindings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstCoinBindings.FullRowSelect = true;
            this.lstCoinBindings.GridLines = true;
            this.lstCoinBindings.HideSelection = false;
            this.lstCoinBindings.Location = new System.Drawing.Point(3, 3);
            this.lstCoinBindings.Name = "lstCoinBindings";
            this.lstCoinBindings.Size = new System.Drawing.Size(786, 372);
            this.lstCoinBindings.TabIndex = 3;
            this.lstCoinBindings.UseCompatibleStateImageBehavior = false;
            this.lstCoinBindings.View = System.Windows.Forms.View.Details;
            // 
            // clmCoinInput
            // 
            this.clmCoinInput.Text = "JVS Coin Slot";
            this.clmCoinInput.Width = 260;
            // 
            // clmCoinOutput
            // 
            this.clmCoinOutput.Text = "Keyboard Output";
            this.clmCoinOutput.Width = 260;
            // 
            // tmrInput
            // 
            this.tmrInput.Interval = 1;
            this.tmrInput.Tick += new System.EventHandler(this.tmrInput_Tick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(266, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "label1";
            this.label1.Visible = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(266, 1);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "label2";
            this.label2.Visible = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(444, 1);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "label3";
            this.label3.Visible = false;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tcBindings);
            this.Controls.Add(this.srpMain);
            this.Controls.Add(this.mnuMain);
            this.MainMenuStrip = this.mnuMain;
            this.Name = "frmMain";
            this.Text = "frmMain";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.srpMain.ResumeLayout(false);
            this.srpMain.PerformLayout();
            this.mnuMain.ResumeLayout(false);
            this.mnuMain.PerformLayout();
            this.mnuBindings.ResumeLayout(false);
            this.tcBindings.ResumeLayout(false);
            this.tabSwitchBindings.ResumeLayout(false);
            this.tabCoinBindings.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip srpMain;
        private System.Windows.Forms.ToolStripStatusLabel lblConnectionState;
        private System.Windows.Forms.MenuStrip mnuMain;
        private System.Windows.Forms.ToolStripMenuItem mnuFile;
        private System.Windows.Forms.ToolStripMenuItem btnAbout;
        private System.Windows.Forms.ToolStripSeparator mnuSep1;
        private System.Windows.Forms.ToolStripMenuItem btnExit;
        private System.Windows.Forms.ToolStripMenuItem mnuBoard;
        private System.Windows.Forms.ToolStripMenuItem btnConnect;
        private System.Windows.Forms.ToolStripMenuItem btnDisconnect;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem btnGetFeatures;
        private System.Windows.Forms.ListView lstSwitchBindings;
        private System.Windows.Forms.ColumnHeader clmJvsInput;
        private System.Windows.Forms.ColumnHeader clmKeybdOutput;
        private System.Windows.Forms.TabControl tcBindings;
        private System.Windows.Forms.TabPage tabSwitchBindings;
        private System.Windows.Forms.TabPage tabCoinBindings;
        private System.Windows.Forms.ListView lstCoinBindings;
        private System.Windows.Forms.ColumnHeader clmCoinInput;
        private System.Windows.Forms.ColumnHeader clmCoinOutput;
        private System.Windows.Forms.ContextMenuStrip mnuBindings;
        private System.Windows.Forms.ToolStripMenuItem btnAddBinding;
        private System.Windows.Forms.ToolStripMenuItem btnRemoveBinding;
        private System.Windows.Forms.Timer tmrInput;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem btnConnectOnStart;
        private System.Windows.Forms.ToolStripMenuItem mnuPollingRate;
        private System.Windows.Forms.ToolStripMenuItem btn50hz;
        private System.Windows.Forms.ToolStripMenuItem btn100hz;
        private System.Windows.Forms.ToolStripMenuItem btn125hz;
        private System.Windows.Forms.ToolStripMenuItem btn200hz;
        private System.Windows.Forms.ToolStripMenuItem btn500hz;
        private System.Windows.Forms.ToolStripMenuItem btn1000hz;
        private System.Windows.Forms.ToolStripMenuItem btnDoNotReset;
        private System.Windows.Forms.ToolStripMenuItem btnImportBindings;
        private System.Windows.Forms.ToolStripMenuItem btnExportBindings;
        private System.Windows.Forms.ToolStripSeparator mnuSep2;
        private System.Windows.Forms.ToolStripMenuItem btnNoFormEvents;
    }
}

