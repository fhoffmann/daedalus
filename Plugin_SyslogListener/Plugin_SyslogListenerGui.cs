using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Plugin_SyslogListener
{
    public partial class Plugin_SyslogListenerGui : daedalusPluginBase.PluginBaseFormClass
    {
        int current_msg_count = 0;
        Timer check_timer = null;

        public Plugin_SyslogListenerGui()
            : base("Plugin_SyslogListenerGui", "GUI zum Syslog-Listener")
        {
            InitializeComponent();

            this.Disposed += new EventHandler(Plugin_SyslogListenerGui_Disposed);

            this.check_timer = new Timer();
            this.check_timer.Interval = 1000;
            this.check_timer.Tick += new EventHandler(check_timer_Tick);
            this.check_timer.Start();
        }

        void Plugin_SyslogListenerGui_Disposed(object sender, EventArgs e)
        {
            this.check_timer.Stop();
        }

        void check_timer_Tick(object sender, EventArgs e)
        {
            int msg_count = Plugin_SyslogListener.SyslogMessages.Length;
            if (msg_count > this.current_msg_count)
            {
                while (this.current_msg_count < msg_count)
                {
                    this.listView1.Items.Add(new ListViewItem(new string[]{DateTime.Now.ToString(),Plugin_SyslogListener.SyslogMessages[this.current_msg_count]}));
                    this.current_msg_count++;
                }
            }
        }
    }
}
