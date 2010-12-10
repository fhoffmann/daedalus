using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace daedalusPluginBase
{
    public abstract class PluginBaseClass
    {
        private string name = null, description = null;
        public string Name { get { return this.name; } }
        public string Description { get { return this.description; } }
        public Thread plugin_thread = null;
        private bool srvRunning = false;
        public bool SrvRunning { get { return this.srvRunning; } }

        public PluginBaseClass(string _name, string _description)
        {
            this.name = _name;
            this.description = _description;

            this.Log(new LogEntry("plugin initialized", this.name));

            this.plugin_thread = new Thread(new ThreadStart(this.plugin_thread_go));
        }

        public virtual void Start() 
        {
            this.srvRunning = true;
            this.plugin_thread = new Thread(new ThreadStart(this.plugin_thread_go));
            this.plugin_thread.Start();
        }
        public virtual void Stop()
        {
            this.srvRunning = false;
            if (this.plugin_thread != null)
            {
                this.plugin_thread.Abort("Programmende");

                //this.plugin_thread = null;
            }
        }
        public virtual void Dispose()
        {
            this.Stop();
        }
        protected abstract void plugin_thread_go();

        protected void Log(LogEntry le)
        {
            LogClass.LogIt(le);
        }
    }
}
