using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JVSLib_KB.Src.Forms
{
    public partial class frmAddCoinBinding : Form
    {
        frmMain frm;

        public frmAddCoinBinding(frmMain frm)
        {
            this.frm = frm;
            InitializeComponent();
        }

        public bool confirmed = false;
        public byte coinSlot;
        public Keys keyBinding;

        byte dataBytes;
        bool jvsBinded = false;

        private void frmAddSwitchBinding_Load(object sender, EventArgs e)
        {
            dataBytes = (byte)Math.Ceiling((decimal)frm.numSwitches / 8);
            tmrCheckInput.Start();
        }

        private void frmAddSwitchBinding_KeyUp(object sender, KeyEventArgs e)
        {
            if (jvsBinded)
            {
                keyBinding = e.KeyCode;
                confirmed = true;
                Close();
            }
            else
            {
                if (e.KeyCode == Keys.Enter)
                {
                    e.Handled = true;
                    coinSlot = (byte)numCoinSlot.Value;
                    jvsBinded = true;
                }
            }
        }
    }
}
