namespace JVSLib_KB.Src.Forms
{
    partial class frmAddCoinBinding
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
            this.lblMessage = new System.Windows.Forms.Label();
            this.tmrCheckInput = new System.Windows.Forms.Timer(this.components);
            this.numCoinSlot = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.numCoinSlot)).BeginInit();
            this.SuspendLayout();
            // 
            // lblMessage
            // 
            this.lblMessage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblMessage.Location = new System.Drawing.Point(0, 0);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(254, 99);
            this.lblMessage.TabIndex = 0;
            this.lblMessage.Text = "Select coin slot";
            this.lblMessage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // numCoinSlot
            // 
            this.numCoinSlot.Location = new System.Drawing.Point(87, 58);
            this.numCoinSlot.Name = "numCoinSlot";
            this.numCoinSlot.Size = new System.Drawing.Size(80, 20);
            this.numCoinSlot.TabIndex = 1;
            // 
            // frmAddCoinBinding
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(254, 99);
            this.Controls.Add(this.numCoinSlot);
            this.Controls.Add(this.lblMessage);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmAddCoinBinding";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "frmAddCoinBinding";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.frmAddSwitchBinding_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.frmAddSwitchBinding_KeyUp);
            ((System.ComponentModel.ISupportInitialize)(this.numCoinSlot)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblMessage;
        private System.Windows.Forms.Timer tmrCheckInput;
        private System.Windows.Forms.NumericUpDown numCoinSlot;
    }
}