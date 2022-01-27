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
    public partial class frmAddSwitchBinding : Form
    {
        frmMain frm;

        public frmAddSwitchBinding(frmMain frm)
        {
            this.frm = frm;
            InitializeComponent();
        }

        public bool confirmed = false;
        public byte switchBindingPlayer;
        public byte switchBindingSwitch;
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
        }

        private void tmrCheckInput_Tick(object sender, EventArgs e)
        {
            for (byte s=0; s < frm.systemInputs.Length; s++)
            {
                if (frm.systemInputs[s])
                {
                    switchBindingPlayer = 0;
                    switchBindingSwitch = s;
                    jvsBinded = true;
                    lblMessage.Text = "Press keyboard button";
                    tmrCheckInput.Stop();
                    return;
                }
            }
            for (byte p = 0; p < frm.numPlayers; p++)
            {
                for (byte s = 0; s < frm.numSwitches; s++)
                {
                    if (frm.inputs[p, s])
                    {
                        switchBindingPlayer = (byte)(p + 1);
                        switchBindingSwitch = s;
                        jvsBinded = true;
                        lblMessage.Text = "Press keyboard button";
                        tmrCheckInput.Stop();
                        return;
                    }
                }
            }
        }
    }
}
