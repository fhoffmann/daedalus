using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace daedalusPluginBase
{
    public partial class PluginBaseFormClass : Form
    {
        private string pluginName = null, description = null;
        public string PluginName { get { return this.pluginName; } }
        public string Description { get { return this.description; } }
        
        public PluginBaseFormClass(string _name, string _description)
        {
            this.pluginName = _name;
            this.Name = _name;
            this.description = _description;

            this.Log(new LogEntry("plugin initialized", this.pluginName));

            InitializeComponent();
        }

        public PluginBaseFormClass()
        {
            this.pluginName = "unnamed plugin";
            this.Name = "unnamed plugin";
            this.description = "";

            this.Log(new LogEntry("plugin initialized", this.pluginName));

            InitializeComponent();
        }

        protected void Log(LogEntry le)
        {
            LogClass.LogIt(le);
        }

        private void PluginBaseFormClass_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Visible = false;
        }
    }
}
