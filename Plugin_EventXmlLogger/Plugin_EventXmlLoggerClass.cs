using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Plugin_EventXmlLogger
{
    public class Plugin_EventXmlLoggerClass : daedalusPluginBase.PluginBaseClass
    {
        XmlDocument xmldoc = null;
        XmlNode xmlroot = null;
        bool canSave = true;
        bool newDataToSave = false;

        public Plugin_EventXmlLoggerClass()
            : base("Plugin_EventXmlLogger", "Plugin welches alle Events in einer XML-Datei mitloggt")
        {
            if (!System.IO.File.Exists("log\\Plugin_EventXmlLogger.xml"))
            {
                // Datei existiert nicht, anlegen
                using (System.IO.StreamWriter sw = new System.IO.StreamWriter("log\\Plugin_EventXmlLogger.xml", true, Encoding.UTF8))
                {
                    sw.Write("<?xml version=\"1.0\" encoding=\"utf-8\" ?>\n\r<events>\n\r</events>\n\r");
                    sw.Flush();
                    sw.Close();
                }
            }
            this.xmldoc = new XmlDocument();
            try
            {
                this.xmldoc.Load("log\\Plugin_EventXmlLogger.xml");
            }
            catch (Exception ex)
            {
                // fehlerhaft beim laden
                System.IO.File.Move("log\\Plugin_EventXmlLogger.xml", "log\\Plugin_EventXmlLogger_corrupt_" + DateTime.Now.Ticks + ".xml");
                using (System.IO.StreamWriter sw = new System.IO.StreamWriter("log\\Plugin_EventXmlLogger.xml", true, Encoding.UTF8))
                {
                    sw.Write("<?xml version=\"1.0\" encoding=\"utf-8\" ?>\n\r<events>\n\r</events>\n\r");
                    sw.Flush();
                    sw.Close();
                }
            }
            this.xmlroot = xmldoc.DocumentElement;

            daedalusPluginBase.LogClass.evt_newLogMessage += new daedalusPluginBase.LogClass.evt_newLogMessageHandle(LogClass_evt_newLogMessage);
        }

        protected override void plugin_thread_go()
        {
            // empty
            while (SrvRunning)
            {
                if (this.canSave)
                {
                    try
                    {
                        this.xmldoc.Save("log\\Plugin_EventXmlLogger.xml");
                    }
                    catch (Exception ex)
                    {
                        // kann nicht speichern, erstmal egal
                    }
                    this.newDataToSave = false;
                }
                System.Threading.Thread.Sleep(1000);
            }
        }

        void LogClass_evt_newLogMessage(daedalusPluginBase.LogEntry le)
        {
            if (this.SrvRunning)
            {
                this.canSave = false;
                /*
                le.Level;
                le.Message;
                le.Timestamp;
                le.Title;
                */
                XmlNode newNode = xmldoc.CreateElement("entry");

                XmlNode node_timestamp = xmldoc.CreateElement("timestamp");
                XmlNode node_level = xmldoc.CreateElement("level");
                XmlNode node_title = xmldoc.CreateElement("title");
                XmlNode node_message = xmldoc.CreateElement("message");
                node_timestamp.InnerText = le.Timestamp.ToString();
                node_level.InnerText = le.Level.ToString();
                node_title.InnerText = le.Title;
                node_message.InnerText = le.Message;

                newNode.AppendChild(node_timestamp);
                newNode.AppendChild(node_level);
                newNode.AppendChild(node_timestamp);
                newNode.AppendChild(node_title);
                newNode.AppendChild(node_message);

                xmlroot.AppendChild(newNode);

                // Sicherstellen das die Log nicht zu vol wird. Daher auf maximal 100 Einträge begrenzen.
                while (xmlroot.ChildNodes.Count > 100)
                {
                    xmlroot.RemoveChild(xmlroot.FirstChild);
                }

                this.newDataToSave = true;
                this.canSave = true;                
            }
        }
    }
}
