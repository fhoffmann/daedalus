using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;

namespace daedalus
{
    public partial class PluginListForm : Form
    {
        public PluginListForm(daedalusPluginBase.PluginBaseClass[] plugins, daedalusPluginBase.PluginBaseFormClass[] pluginGuis)
        {
            InitializeComponent();

            foreach (daedalusPluginBase.PluginBaseClass plugin in plugins)
            {
                Assembly a = Assembly.GetAssembly(plugin.GetType());
                this.listView1.Items.Add(new ListViewItem(new string[] { a.FullName, a.Location }));
            }

            foreach (daedalusPluginBase.PluginBaseFormClass plugin in pluginGuis)
            {
                Assembly a = Assembly.GetAssembly(plugin.GetType());
                this.listView1.Items.Add(new ListViewItem(new string[] { a.FullName, a.Location }));
            }
        }

        private void PluginListForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Visible = false;
        }
    }
}
