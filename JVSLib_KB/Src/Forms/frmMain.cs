using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.Runtime.InteropServices;
using System.IO;

using JVSLib.Src;
using JVSLib.Src.DataTypes;

using JVSLib_KB.Src.DataTypes;
using JVSLib_KB.Src.Helpers;

namespace JVSLib_KB.Src.Forms
{
    public partial class frmMain : Form
    {
        const byte nodeNum = 1;

        const uint KEYEVENTF_EXTENDEDKEY = 0x0001;
        const uint KEYEVENTF_KEYUP = 0x0002;
        
        const uint MAPVK_VK_TO_VSC = 0x00;
        const uint MAPVK_VSC_TO_VK = 0x01;
        const uint MAPVK_VK_TO_CHAR = 0x02;
        const uint MAPVK_VSC_TO_VK_EX = 0x03;
        const uint MAPVK_VK_TO_VSC_EX = 0x04;

        [DllImport("user32.dll", SetLastError = true)]
        static extern void keybd_event(byte bVk, ushort bScan, uint dwFlags, int dwExtraInfo);

        [DllImport("user32.dll")]
        static extern ushort MapVirtualKeyEx(byte uCode, uint uMapType, IntPtr dwhkl);

        string[] args;

        public frmMain(string[] args)
        {
            this.args = args;
            InitializeComponent();
        }

        JVSConnection jvs;
        public string identifier;
        public bool[] systemInputs;
        public bool[,] inputs;
        public byte numPlayers;
        public byte numSwitches;

        IOConfig jvsConfig;
        public bool[] prevSystemInputs;
        public bool[,] prevInputs;

        CoreConfig config;

        private void frmMain_Load(object sender, EventArgs e)
        {
            if (args.Contains("-silent") || args.Contains("-s"))
            {
                BeginInvoke(new MethodInvoker(delegate { Hide(); }));
            }
            config = new CoreConfig("config.bin");
            jvs = new JVSConnection();
            btnConnectOnStart.Checked = config.connectOnStart;
            btnDoNotReset.Checked = config.noReset;
            //btnNoFormEvents.Checked = config.doNotPrioritiseFormEvents;
            switch (config.pollingRate)
            {
                case 50:
                    btn50hz.Checked = true;
                    break;
                case 100:
                    btn100hz.Checked = true;
                    break;
                case 125:
                    btn125hz.Checked = true;
                    break;
                case 200:
                    btn200hz.Checked = true;
                    break;
                case 500:
                    btn500hz.Checked = true;
                    break;
                case 1000:
                    btn1000hz.Checked = true;
                    break;
            }
            //if (!config.doNotPrioritiseFormEvents)
            //{
            //    tmrInput.Interval = 10000;
            //}
            if (config.connectOnStart)
            {
                btnConnect_Click(this, null);
            }
        }

        private void btnAbout_Click(object sender, EventArgs e)
        {
            MessageBox.Show("JVSLib_KB\r\nBy derole");
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (jvs != null)
            {
                jvs.Close();
            }
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            if (!jvs.connected)
            {
                lblConnectionState.Text = "Connecting...";
                jvs.wait = 10;
                jvs.Connect();
                if (jvs.connected)
                {
                    if (!config.noReset)
                    {
                        jvs.Reset();
                        jvs.Reset();
                        jvs.SetAddress(nodeNum);
                    }
                    var ioIdentify = jvs.IOIdentify(nodeNum);
                    if (ioIdentify != null)
                    {
                        identifier = ioIdentify.identifier;
                        var data = jvs.FeatureCheck(nodeNum);
                        numPlayers = data.features[JVSFeatCheck.FeatType.Switch].param1;
                        numSwitches = data.features[JVSFeatCheck.FeatType.Switch].param2;
                        jvsConfig = new IOConfig(string.Format(@"bindings\{0}.bin"
                            , BitConverter.ToString(MD5.Create().ComputeHash(Encoding.ASCII.GetBytes(identifier))).Replace("-", "")));
                        UpdateBindings(jvsConfig);
                        btnImportBindings.Enabled = true;
                        btnExportBindings.Enabled = true;
                        jvs.wait = 1000 / config.pollingRate;
                        tmrInput.Start();
                        lblConnectionState.Text = string.Format("Connected to {0}", identifier);
                        btnGetFeatures.Enabled = true;
                    }
                    else
                    {
                        jvs.Close();
                        lblConnectionState.Text = "Connection error!";
                    }
                }
                else
                {
                    lblConnectionState.Text = "Connection error!";
                }
            }
        }

        void UpdateBindings(IOConfig config)
        {
            lstSwitchBindings.Items.Clear();
            foreach (var binding in config.switchBindings)
            {
                var data = binding.Key;
                var item = lstSwitchBindings.Items.Add(string.Format("Player{0} Switch{1}", data.Item1, data.Item2));
                item.SubItems.Add(Enum.GetName(typeof(Keys), binding.Value));
            }
            lstCoinBindings.Items.Clear();
            //TODO
        }

        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            btnImportBindings.Enabled = false;
            btnExportBindings.Enabled = false;
            tmrInput.Stop();
            if (jvs != null) { jvs.Close(); }
            if (jvsConfig != null) { jvsConfig.Close(); }
            btnGetFeatures.Enabled = false;
            lstSwitchBindings.Items.Clear();
            lstCoinBindings.Items.Clear();
            lblConnectionState.Text = "Disconnected";
        }

        private void btnGetFeatures_Click(object sender, EventArgs e)
        {
            try
            {
                jvs.wait = 10;
                var data = jvs.FeatureCheck(nodeNum);
                string message = "";
                var jvsRev = jvs.JVSRevision(nodeNum);
                var jvsCmdRev = jvs.CommandRevision(nodeNum);
                var jvsCommVer = jvs.CommunicationVersion(nodeNum);
                message += string.Format("JVS version: {0}.{1}\r\n", jvsRev.rev >> 4, jvsRev.rev & 0xF);
                message += string.Format("JVS cmd rev: {0}.{1}\r\n", jvsCmdRev.rev >> 4, jvsCmdRev.rev & 0xF);
                message += string.Format("JVS comm ver: {0}.{1}\r\n", jvsCommVer.ver >> 4, jvsCommVer.ver & 0xF);
                message += "\r\n";
                message += "List of features:\r\n";
                foreach (var feature in data.features)
                {
                    message += string.Format("{0}\r\n", Enum.GetName(typeof(JVSFeatCheck.FeatType), feature.Key));
                    message += string.Format("  Param 1: {0}\r\n", feature.Value.param1);
                    message += string.Format("  Param 2: {0}\r\n", feature.Value.param2);
                    message += string.Format("  Param 3: {0}\r\n", feature.Value.param3);
                }
                jvs.wait = 1000 / config.pollingRate;
                MessageBox.Show(message);
            }
            catch
            {
                MessageBox.Show("Error getting features");
            }
        }

        private void btnAddBinding_Click(object sender, EventArgs e)
        {
            if (tcBindings.SelectedTab == tabSwitchBindings)
            {
                var frm = new frmAddSwitchBinding(this);
                frm.ShowDialog();
                if (frm.confirmed)
                {
                    var data = new Tuple<byte, byte>(frm.switchBindingPlayer, frm.switchBindingSwitch);
                    if (!jvsConfig.switchBindings.ContainsKey(data))
                    {
                        jvsConfig.switchBindings.Add(data, frm.keyBinding);
                        jvsConfig.Save();
                        UpdateBindings(jvsConfig);
                    }
                    else
                    {
                        MessageBox.Show("This button is already binded!");
                    }
                }
            }
            else
            {
                var frm = new frmAddCoinBinding(this);
                frm.ShowDialog();
                if (frm.confirmed)
                {
                    //TODO
                    //var data = new Tuple<byte, byte>(frm.switchBindingPlayer, frm.switchBindingSwitch);
                    //if (!jvsConfig.switchBindings.ContainsKey(data))
                    //{
                    //    jvsConfig.switchBindings.Add(data, frm.keyBinding);
                    //    jvsConfig.Save();
                    //    UpdateBindings(jvsConfig);
                    //}
                    //else
                    //{
                    //    MessageBox.Show("This button is already binded!");
                    //}
                }
            }
        }

        private void btnRemoveBinding_Click(object sender, EventArgs e)
        {
            var bindingsToRemove = new List<Tuple<byte, byte>>();
            foreach (int index in lstSwitchBindings.SelectedIndices)
            {
                bindingsToRemove.Add(jvsConfig.switchBindings.ElementAt(index).Key);
            }
            foreach (var binding in bindingsToRemove)
            {
                jvsConfig.switchBindings.Remove(binding);
            }
            jvsConfig.Save();
            UpdateBindings(jvsConfig);
        }

        private void mnuBindings_Opening(object sender, CancelEventArgs e)
        {
            if (!jvs.connected)
            {
                e.Cancel = true;
                return;
            }
            if (lstSwitchBindings.SelectedItems.Count > 0)
            {
                btnAddBinding.Visible = false;
                btnRemoveBinding.Visible = true;
            }
            else
            {
                btnAddBinding.Visible = true;
                btnRemoveBinding.Visible = false;
            }
        }

        private void tmrInput_Tick(object sender, EventArgs e)
        {
            var data = jvs.SwitchInputs(nodeNum, numPlayers, numSwitches);
            if (data == null)
            {
                Application.DoEvents();
                tmrInput_Tick(sender, e);
            }
            systemInputs = data.systemInputs;
            inputs = data.inputs;
            if (prevSystemInputs == null) { prevSystemInputs = systemInputs; }
            if (prevInputs == null) { prevInputs = inputs; }
            //DebugPrint();
            for (byte s = 0; s < 8; s++)
            {
                if (systemInputs[s] != prevSystemInputs[s])
                {
                    var input = new Tuple<byte,byte>(0,s);
                    if (jvsConfig.switchBindings.ContainsKey(input))
                    {
                        var key = jvsConfig.switchBindings[input];
                        if (systemInputs[s])
                        {
                            keybd_event((byte)key, MapVirtualKeyEx((byte)key, 0x04, IntPtr.Zero), Output.IsExtendedKey(key), 0);
                        }
                        else
                        {
                            keybd_event((byte)key, MapVirtualKeyEx((byte)key, 0x04, IntPtr.Zero), Output.IsExtendedKey(key) | KEYEVENTF_KEYUP, 0);
                        }
                    }
                }
            }
            for (byte p = 0; p < numPlayers; p++)
            {
                for (byte s = 0; s < numSwitches; s++)
                {
                    if (inputs[p, s] != prevInputs[p, s])
                    {
                        var input = new Tuple<byte, byte>((byte)(p + 1), s);
                        if (jvsConfig.switchBindings.ContainsKey(input))
                        {
                            var key = jvsConfig.switchBindings[input];
                            if (inputs[p, s])
                            {
                                keybd_event((byte)key, MapVirtualKeyEx((byte)key, MAPVK_VK_TO_VSC_EX, IntPtr.Zero), Output.IsExtendedKey(key), 0);
                            }
                            else
                            {
                                keybd_event((byte)key, MapVirtualKeyEx((byte)key, MAPVK_VK_TO_VSC_EX, IntPtr.Zero), Output.IsExtendedKey(key) | KEYEVENTF_KEYUP, 0);
                            }
                        }
                    }
                }
            }
            prevSystemInputs = systemInputs;
            prevInputs = inputs;

            //var coinData = jvs.CoinInputs(nodeNum, 2);
            //label3.Text = "";
            //foreach (var slot in coinData.slots)
            //{
            //    label3.Text += Enum.GetName(typeof(JVSCoinInp.SlotCondition), slot.condition) + " ";
            //    label3.Text += slot.coinCount + "\r\n";
            //}
        }

        void DebugPrint()
        {
            label2.Text = "";
            foreach (var input in systemInputs)
            {
                label2.Text += input ? "1" : "0";
            }
            label1.Text = "";
            for (byte p = 0; p < numPlayers; p++)
            {
                for (byte s = 0; s < numSwitches; s++)
                {
                    label1.Text += inputs[p, s] ? "1" : "0";
                }
                label1.Text += "\r\n";
            }
        }

        private void btnhz_Click(object sender, EventArgs e)
        {
            var btn = (ToolStripMenuItem)sender;
            switch (btn.Name)
            {
                case "btn50hz":
                    config.pollingRate = 50;
                    break;
                case "btn100hz":
                    config.pollingRate = 100;
                    break;
                case "btn125hz":
                    config.pollingRate = 125;
                    break;
                case "btn200hz":
                    config.pollingRate = 200;
                    break;
                case "btn500hz":
                    config.pollingRate = 500;
                    break;
                case "btn1000hz":
                    config.pollingRate = 1000;
                    break;
            }
            btn50hz.Checked = false;
            btn100hz.Checked = false;
            btn200hz.Checked = false;
            btn500hz.Checked = false;
            btn1000hz.Checked = false;
            btn.Checked = true;
            config.Save();
            jvs.wait = 1000 / config.pollingRate;
        }

        private void btnDoNotReset_Click(object sender, EventArgs e)
        {
            config.noReset = btnDoNotReset.Checked;
            config.Save();
        }

        private void btnConnectOnStart_Click(object sender, EventArgs e)
        {
            config.connectOnStart = btnConnectOnStart.Checked;
            config.Save();
        }

        private void btnNoFormEvents_Click(object sender, EventArgs e)
        {
            config.doNotPrioritiseFormEvents = btnNoFormEvents.Checked;
            config.Save();
        }

        private void btnImportBindings_Click(object sender, EventArgs e)
        {
            var dlg = new OpenFileDialog();
            dlg.Title = "Open binding file";
            dlg.Filter = "Binding (*.bin)|*.bin";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                if (File.Exists(dlg.FileName))
                {
                    var config = new IOConfig(dlg.FileName);
                    jvsConfig.switchBindings = config.switchBindings;
                    config.Close();
                    UpdateBindings(jvsConfig);
                    MessageBox.Show("Import successful!");
                }
            }
        }

        private void btnExportBindings_Click(object sender, EventArgs e)
        {
            var dlg = new SaveFileDialog();
            dlg.Title = "Save binding file";
            dlg.Filter = "Binding (*.bin)|*.bin";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                var config = new IOConfig(dlg.FileName);
                config.switchBindings = jvsConfig.switchBindings;
                config.Close();
                MessageBox.Show("Export successful!");
            }
        }
    }
}
