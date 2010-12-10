using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using daedalusPluginBase;
using System.Reflection;

namespace daedalus
{
    class MainControlClass : ApplicationContext
    {
        List<PluginBaseClass> listOfPlugins = null;
        List<PluginBaseFormClass> listOfGuiPlugins = null;
        NotifyIcon nIcon = null;
        ContextMenuStrip nIconContextMenu = null;
        Queue<Icon> nextIcon = null;
        System.Threading.Timer icon_timer = null;
        PluginListForm pluginList = null;

        public MainControlClass()
        {
            this.Init();

            LogClass.LogIt(new LogEntry("Information", Application.ProductName + " is running...", LogEntry.LogLevel.Information));
        }

        private void Init()
        {
            // Vars und Obj erstellen
            this.nIcon = new NotifyIcon();
            this.nIcon.Icon = Properties.Resources.nIcon_icon2;            
            this.nIcon.Text = Application.ProductName + " is running...";
            this.nIcon.DoubleClick += new EventHandler(nIcon_DoubleClick);
            this.nIcon.Visible = true;            

            // Andere Events registrieren
            LogClass.evt_newLogMessage += new LogClass.evt_newLogMessageHandle(LogClass_evt_newLogMessage);

            // Plugins initialisieren und starten
            this.Init_Plugins();
            this.LoadPlugins_test();

            this.pluginList = new PluginListForm(this.listOfPlugins.ToArray(), this.listOfGuiPlugins.ToArray());
            
            this.CreateMenuEntriesForGuiPlugins();

            foreach (PluginBaseClass plugin in this.listOfPlugins)
            {
                plugin.Start();
            }

            this.Init_IconChanger();
        }

        private void Init_Plugins()
        {
            this.listOfPlugins = new List<PluginBaseClass>();
            this.listOfGuiPlugins = new List<PluginBaseFormClass>();

            foreach (string filePath in System.IO.Directory.GetFiles("plugins"))
            {
                System.IO.FileInfo fileInfo = new System.IO.FileInfo(filePath);
                if (fileInfo.Exists)
                {
                    if (fileInfo.Extension == ".dll")
                    {
                        Assembly assembly = Assembly.LoadFile(fileInfo.FullName);
                        
                        foreach (Type type in assembly.GetExportedTypes())
                        {
                            if (type.IsSubclassOf(typeof(PluginBaseClass)))
                            {
                                this.listOfPlugins.Add((PluginBaseClass)type.Assembly.CreateInstance(type.FullName));
                            }
                            else if (type.IsSubclassOf(typeof(PluginBaseFormClass)))
                            {
                                this.listOfGuiPlugins.Add((PluginBaseFormClass)type.Assembly.CreateInstance(type.FullName));
                            }
                        }
                    }
                }
            }
        }

        private void LoadPlugins_test()
        {
            // plugins die ich entwickle, werden erstmal direkt eingebunden. so ist es einfacher zu testen
            this.listOfPlugins.Add(new Plugin_EventHttpServer.Plugin_EventHttpServer());
            this.listOfPlugins.Add(new Plugin_SyslogListener.Plugin_SyslogListener());
            this.listOfPlugins.Add(new Plugin_EventXmlLogger.Plugin_EventXmlLoggerClass());
            this.listOfPlugins.Add(new Plugin_NetworkStatusCheck.Plugin_NetworkStatusCheckClass());
            this.listOfPlugins.Add(new Plugin_InformAboutProcesses.Plugin_InformAboutProcessesClass());
            this.listOfPlugins.Add(new Plugin_SimpleTcpServer.Plugin_SimpleTcpServerClass());

            this.listOfGuiPlugins.Add(new Plugin_SyslogListener.Plugin_SyslogListenerGui());
            this.listOfGuiPlugins.Add(new Plugin_UdpClient.Plugin_UdpClientGui());
        }

        private void CreateMenuEntriesForGuiPlugins()
        {
            this.nIconContextMenu = new ContextMenuStrip();
            this.nIcon.ContextMenuStrip = this.nIconContextMenu;

            for (int i = 0; i < this.listOfGuiPlugins.Count; i++)
            {
                PluginBaseFormClass p = this.listOfGuiPlugins[i]; // wird benötigt, da sonst eine foreach variable vo letzen stand genutzt wird. es muss eine interne zuweisung (in der anonymen methode) geben)
                this.nIconContextMenu.Items.Add(p.PluginName, Properties.Resources.toolbox, new EventHandler(delegate(object sender, EventArgs e) { p.Show(); }));
            }

            this.nIconContextMenu.Items.Add("---");
            this.nIconContextMenu.Items.Add("Plugin-list", Properties.Resources.wrench_screwdriver, new EventHandler(delegate(object sender, EventArgs e) { this.pluginList.Show(); }));
            this.nIconContextMenu.Items.Add("exit", Properties.Resources.yin_yang1, new EventHandler(delegate(object sender, EventArgs e) { this.Dispose(); }));
        }

        private void Init_IconChanger()
        {
            this.nextIcon = new Queue<Icon>();
            this.nextIcon.Enqueue(Properties.Resources.eye);
            this.nextIcon.Enqueue(Properties.Resources.eye);
            this.nextIcon.Enqueue(Properties.Resources.eye);
            this.nextIcon.Enqueue(Properties.Resources.eye);
            this.nextIcon.Enqueue(Properties.Resources.eye);
            this.nextIcon.Enqueue(Properties.Resources.eye);
            this.nextIcon.Enqueue(Properties.Resources.eye);
            this.nextIcon.Enqueue(Properties.Resources.eye_half);
            this.nextIcon.Enqueue(Properties.Resources.eye_half);
            this.nextIcon.Enqueue(Properties.Resources.eye_half);
            this.nextIcon.Enqueue(Properties.Resources.eye_close);
            this.nextIcon.Enqueue(Properties.Resources.eye_close);
            this.nextIcon.Enqueue(Properties.Resources.eye_close);
            this.nextIcon.Enqueue(Properties.Resources.eye_close);
            this.nextIcon.Enqueue(Properties.Resources.eye_close);
            this.nextIcon.Enqueue(Properties.Resources.eye_close);
            this.nextIcon.Enqueue(Properties.Resources.eye_close);
            this.nextIcon.Enqueue(Properties.Resources.eye_half);
            this.nextIcon.Enqueue(Properties.Resources.eye_half);
            this.nextIcon.Enqueue(Properties.Resources.eye_half);
            this.nextIcon.Enqueue(Properties.Resources.eye_red);
            this.nextIcon.Enqueue(Properties.Resources.eye_red);
            this.nextIcon.Enqueue(Properties.Resources.eye_red);
            this.nextIcon.Enqueue(Properties.Resources.eye_red);
            this.nextIcon.Enqueue(Properties.Resources.eye_red);

            this.icon_timer = new System.Threading.Timer(new System.Threading.TimerCallback(this.icon_timer_do), null, 0, 300);
        }

        void icon_timer_do(object state)
        {
            Icon ne = this.nextIcon.Dequeue();
            this.nextIcon.Enqueue(ne);
            if (this.nIcon != null)
            {
                this.nIcon.Icon = ne;
            }
            else
            {
                this.icon_timer.Dispose();
            }
        }

        void nIcon_DoubleClick(object sender, EventArgs e)
        {
            if (MessageBox.Show("Close " + Application.ProductName + "?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                // Yes gedrückt
                this.Dispose();
            }
        }

        public new void Dispose()
        {
            this.icon_timer.Dispose();
            this.nIcon.Visible = false;
            this.nIcon.Dispose();

            for(int i=0; i<this.listOfPlugins.Count;i++)
            {
                this.listOfPlugins[i].Dispose();
                this.listOfPlugins[i] = null;
            }

            for (int i = 0; i < this.listOfGuiPlugins.Count; i++)
            {
                this.listOfGuiPlugins[i].Dispose();
                this.listOfGuiPlugins[i] = null;
            }

            this.pluginList.Dispose();

            Application.Exit();
        }

        void LogClass_evt_newLogMessage(LogEntry le)
        {
            if (le.Message.Length > 64)
            {
                if (le.Level == LogEntry.LogLevel.Error)
                {
                    // Fehler Anzeigen!
                    this.nIcon.ShowBalloonTip(2000, le.Title, le.Message.Substring(0,63), ToolTipIcon.Error);
                }
                else if (le.Level == LogEntry.LogLevel.Information)
                {
                    // Informationen Anzeigen!
                    this.nIcon.ShowBalloonTip(2000, le.Title, le.Message.Substring(0, 63), ToolTipIcon.Info);
                }
                else if (le.Level == LogEntry.LogLevel.Warning)
                {
                    // Warnungen anzeigen!
                    this.nIcon.ShowBalloonTip(2000, le.Title, le.Message.Substring(0, 63), ToolTipIcon.Warning);
                }
                else if (le.Level == LogEntry.LogLevel.Verbose)
                {
                    // Verbose nicht anzeigen
                }
            }
            else
            {
                if (le.Level == LogEntry.LogLevel.Error)
                {
                    // Fehler Anzeigen!
                    this.nIcon.ShowBalloonTip(2000, le.Title, le.Message, ToolTipIcon.Error);
                }
                else if (le.Level == LogEntry.LogLevel.Information)
                {
                    // Informationen Anzeigen!
                    this.nIcon.ShowBalloonTip(2000, le.Title, le.Message, ToolTipIcon.Info);
                }
                else if (le.Level == LogEntry.LogLevel.Warning)
                {
                    // Warnungen anzeigen!
                    this.nIcon.ShowBalloonTip(2000, le.Title, le.Message, ToolTipIcon.Warning);
                }
                else if (le.Level == LogEntry.LogLevel.Verbose)
                {
                    // Verbose nicht anzeigen
                }
            }
        }
    }
}
