using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;

namespace Plugin_SimpleTcpServer
{
    public class Plugin_SimpleTcpServerClass : daedalusPluginBase.PluginBaseClass
    {
        System.Net.Sockets.TcpListener tcplistener = null;

        public Plugin_SimpleTcpServerClass():base("Plugin_SimpleTcpServerClass", "Einfacher TCP-Server")
        {
        }

        protected override void plugin_thread_go()
        {
            if (this.tcplistener != null)
            {
                if (this.tcplistener.Server.IsBound)
                {
                    this.tcplistener.Server.Close();
                    this.tcplistener.Stop();
                    this.tcplistener = null;
                }
            }

            this.tcplistener = new System.Net.Sockets.TcpListener(888);
            this.tcplistener.Start();

            while (this.SrvRunning)
            {
                TcpClient client = this.tcplistener.AcceptTcpClient();

                NetworkStream ns = client.GetStream();

                byte[] buffer = new byte[512];
                List<byte> recData = new List<byte>();
                string received = null;
                /*
                if (ns.DataAvailable)
                {
                    while (ns.Read(buffer,0,512) > 0)
                    {
                        recData.AddRange(buffer);
                    }

                    received = Encoding.UTF8.GetString(recData.ToArray());
                }
                */
                byte[] towrite = Encoding.UTF8.GetBytes("du doof!");
                ns.Write(towrite, towrite.Length, 0);

                client.Close();
            }
        }
    }
}
