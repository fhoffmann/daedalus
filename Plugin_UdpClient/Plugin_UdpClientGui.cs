using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Plugin_UdpClient
{
    public partial class Plugin_UdpClientGui : daedalusPluginBase.PluginBaseFormClass
    {
        public Plugin_UdpClientGui():base("Plugin_UdpClient", "Gui zum versenden von UDP-Paketen")
        {
            InitializeComponent();

            this.udp = new System.Net.Sockets.UdpClient();
        }

        System.Net.Sockets.UdpClient udp = null;

        private void button_send_Click(object sender, EventArgs e)
        {
            string srv = this.textBox_srv.Text;
            int port = int.Parse(this.textBox_port.Text);
            byte[] msg = Encoding.ASCII.GetBytes(this.textBox_msg.Text);

            int sent = this.udp.Send(msg, msg.Length, srv, port);
            if (sent == msg.Length)
            {
                MessageBox.Show("Es wurden alle Bytes versendet! (" + sent + " von " + msg.Length + " (" + (double)sent * (100.0 / (double)msg.Length) + "%) )\n\nDas heißt NICHT, dass die Nachricht angekommen ist!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Es wurden nicht alle Bytes versendet!! (" + sent + " von " + msg.Length + " (" + (double)sent * (100.0 / (double)msg.Length) + "%)", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
