using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Plugin_SyslogListener
{
    public class Plugin_SyslogListener : daedalusPluginBase.PluginBaseClass
    {
        System.Net.IPEndPoint endpoint = null;
        System.Net.Sockets.UdpClient udp = null;
        static List<string> incMessages = new List<string>();
        public static string[] SyslogMessages { get { return incMessages.ToArray(); } }

        public Plugin_SyslogListener()
            : base("Plugin_SyslogListener", "Ein einfacher Syslog Listener")
        {
            this.endpoint = new System.Net.IPEndPoint(System.Net.IPAddress.Any, 514);            
        }
        public override void Dispose()
        {
            this.udp.Close();
            this.udp = null;
            base.Dispose();
        }
        public override void Start()
        {
            this.udp = new System.Net.Sockets.UdpClient(this.endpoint);
            base.Start();
        }
        public override void Stop()
        {
            base.Stop();
        }

        protected override void plugin_thread_go()
        {

            byte[] buffer = null;
            string msg = null;

            while (this.SrvRunning)
            {
                if (this.udp.Available > 0)
                {
                    buffer = udp.Receive(ref endpoint);
                    msg = this.ParseMessage(buffer);

                    incMessages.Add(msg);
                    this.Log(new daedalusPluginBase.LogEntry("Plugin_SyslogListener", msg, daedalusPluginBase.LogEntry.LogLevel.Information));
                }

                System.Threading.Thread.Sleep(100);
            }

            this.Stop();
        }

        private string ParseMessage(byte[] buffer)
        {
            string ret = Encoding.ASCII.GetString(buffer); 

            return ret;
        }
    }
}
