using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Threading;

namespace Plugin_EventHttpServer
{
    public class Plugin_EventHttpServer : daedalusPluginBase.PluginBaseClass
    {
        System.Net.IPEndPoint endpoint = null;
        System.Net.Sockets.TcpListener listener = null;
        //System.Net.HttpListener httpListener = null; // später implementieren. erstmal via tcplistener

        public Plugin_EventHttpServer()
            : base("Plugin_EventHttpServer", "Ein einfacher HttpServer welcher alle Events auflistet")
        {
            this.endpoint = new System.Net.IPEndPoint(System.Net.IPAddress.Any, 55555);
            this.Log(new daedalusPluginBase.LogEntry("Plugin_EventHttpServer", "listening on port " + this.endpoint.Port));
        }

        public override void Start()
        {
            this.listener = new System.Net.Sockets.TcpListener(endpoint);
            this.listener.Start();

            //this.listener.BeginAcceptTcpClient(new AsyncCallback(DoAcceptTcpClientCallback), listener);
            base.Start();

        }

        public override void Stop()
        {
            this.listener.Stop();
            this.listener.Server.Close();

            base.Stop();
        }

        protected override void plugin_thread_go()
        {
            while (this.SrvRunning)
            {
                try
                {
                    TcpClient client = listener.AcceptTcpClient();

                    // Daten sammeln (auf xml umbauen)
                    //string data_send = "<html>\n<head>\n<title>deadalus - EventHttpServer</title>\n</head>\n<body>\n";
                    //data_send += "<table border=\"1\">\n<tr>\n<th>title</th><th>message</th>\n</tr>\n";
                    string data_send = "<?xml version=\"1.0\" encoding=\"utf-8\" ?>\n\r<events>\n\r";
                    foreach (daedalusPluginBase.LogEntry le in daedalusPluginBase.LogClass.GetAllLogEntries())
                    {
                        //data_send += "<tr>\n<td>" + le.Title + "</td><td>" + le.Message + "</td>\n</tr>";
                        data_send += "<entry>\n\r";
                        data_send += "<title>" + le.Title + "</title>\n\r";
                        data_send += "<message>" + le.Message + "</message>\n\r";
                        data_send += "<level>" + le.Level.ToString() + "</level>\n\r";
                        data_send += "</entry>\n\r";
                    }

                    //data_send += "</table>\n</body>\n</html>";
                    data_send += "</events>\n\r";

                    NetworkStream sw = client.GetStream();
                    byte[] buffer = Encoding.ASCII.GetBytes(data_send);
                    sw.Write(buffer, 0, buffer.Length);
                    sw.Flush();
                    client.Close();
                }
                catch (Exception)
                {
                }

                //Thread.Sleep(100);
            }
        }
    }
}
