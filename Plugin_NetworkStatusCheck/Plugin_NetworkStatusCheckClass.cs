using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.NetworkInformation;
using System.Windows.Forms;

namespace Plugin_NetworkStatusCheck
{
    public class Plugin_NetworkStatusCheckClass : daedalusPluginBase.PluginBaseClass
    {
        Ping ping = null;
        Queue<string> listOfLanChecks = null;
        Queue<string> listOfWanChecks = null;
        bool lanOk = false;
        bool wanOk = false;

        NotifyIcon nIcon = null;

        public Plugin_NetworkStatusCheckClass()
            : base("Plugin_NetworkStatusCheckClass", "Plugin welches die Reichweiter des Netzes testet")
        {
            this.ping = new System.Net.NetworkInformation.Ping();

            this.listOfLanChecks = new Queue<string>();
            this.listOfWanChecks = new Queue<string>();

            if (!System.IO.File.Exists("config\\Plugin_NetworkStatusCheck.xml"))
            {
                // Datei existiert nicht, anlegen
                using (System.IO.StreamWriter sw = new System.IO.StreamWriter("config\\Plugin_NetworkStatusCheck.xml", true, Encoding.UTF8))
                {
                    sw.Write("<?xml version=\"1.0\" encoding=\"utf-8\" ?>\n\r<config>\n\r</config>\n\r");
                    sw.Flush();
                    sw.Close();
                }
            }
            System.Xml.XmlDocument xmldoc = new System.Xml.XmlDocument();
            xmldoc.Load("config\\Plugin_NetworkStatusCheck.xml");
            System.Xml.XmlNode xmlroot = xmldoc.DocumentElement;

            if (xmlroot["lanNodes"] != null)
            {
                foreach (System.Xml.XmlNode lanNode in xmlroot["lanNodes"])
                {
                    if (!string.IsNullOrEmpty(lanNode.InnerText))
                    {
                        this.listOfLanChecks.Enqueue(lanNode.InnerText);
                    }
                }
            }
            if (xmlroot["wanNodes"] != null)
            {
                foreach (System.Xml.XmlNode wanNode in xmlroot["wanNodes"])
                {
                    if (!string.IsNullOrEmpty(wanNode.InnerText))
                    {
                        this.listOfWanChecks.Enqueue(wanNode.InnerText);
                    }
                }
            }
            this.nIcon = new NotifyIcon();
            this.nIcon.Icon = Properties.Resources.noNetwork2;
            this.nIcon.Text = "no network reachable";
            this.nIcon.Visible = true;
        }

        public override void Stop()
        {
            this.nIcon.Visible = false;

            base.Stop();
        }

        protected override void plugin_thread_go()
        {
            bool currentCheckLan = false;
            bool currentCheckWan = false;
            int currentLanMax = this.listOfLanChecks.Count;
            int currentWanMax = this.listOfWanChecks.Count;
            string srv = "";
            string lanTxt = "";
            string wanTxt = "";
            PingReply reply = null;
            string displayText = "";

            while (this.SrvRunning)
            {
                for(int i=0; i<currentLanMax;i++)
                {
                    srv = listOfLanChecks.Dequeue();
                    this.listOfLanChecks.Enqueue(srv);
                    reply = this.ping.Send(srv);
                    if (reply.Status == IPStatus.Success)
                    {
                        currentCheckLan = true;
                        lanTxt = "lan(" + srv + " in " + reply.RoundtripTime + "ms)";
                        break;
                    }
                }
                for (int i = 0; i < currentWanMax; i++)
                {
                    srv = listOfWanChecks.Dequeue();
                    this.listOfWanChecks.Enqueue(srv);
                    reply = this.ping.Send(srv);
                    if (reply.Status == IPStatus.Success)
                    {
                        currentCheckWan = true;
                        wanTxt = "wan(" + srv + " in " + reply.RoundtripTime + "ms)";
                        break;
                    }
                }
                this.lanOk = currentCheckLan;
                this.wanOk = currentCheckWan;

                if (lanOk && wanOk)
                {
                    displayText = "lan/wan ok\n" + lanTxt + "\n" + wanTxt;
                    if (displayText.Length > 64) { displayText = displayText.Substring(0, 63); }
                    this.nIcon.Text = displayText;
                    this.nIcon.Icon = Properties.Resources.LanAndWan2;
                }
                else if (lanOk)
                {
                    displayText = "lan ok\n" + lanTxt + "\n" + wanTxt;
                    if (displayText.Length > 64) { displayText = displayText.Substring(0, 63); }
                    this.nIcon.Text = displayText;
                    this.nIcon.Icon = Properties.Resources.justLan2;
                }
                else if (wanOk)
                {
                    displayText = "wan ok\n" + lanTxt + "\n" + wanTxt;
                    if (displayText.Length > 64) { displayText = displayText.Substring(0, 63); }
                    this.nIcon.Text = displayText;
                    this.nIcon.Icon = Properties.Resources.justWan2;
                }
                else
                {
                    displayText = "no network\n" + lanTxt + "\n" + wanTxt;
                    if (displayText.Length > 64) { displayText = displayText.Substring(0, 63); }
                    this.nIcon.Text = displayText;
                    this.nIcon.Icon = Properties.Resources.noNetwork2;
                }

                currentCheckLan = false;
                currentCheckWan = false;
                System.Threading.Thread.Sleep(60000);
            }
        }
    }
}
