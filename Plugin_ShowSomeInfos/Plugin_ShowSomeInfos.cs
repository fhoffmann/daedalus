using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Plugin_ShowSomeInfos
{
    public class ShowSomeInfosClass : daedalusPluginBase.PluginBaseClass
    {
        //Timer timer = null;

        public override void Start()
        {
            
        }
        public override void Stop()
        {
            base.Stop();
        }
        public override void Dispose()
        {
            base.Dispose();
        }

        public ShowSomeInfosClass()
            : base("ShowSomeInfos", "Zeigt einfach nur ein paar Systeminformationen periodisch an.")
        {
        }
        protected override void plugin_thread_go()
        {
            while (this.SrvRunning)
            {
                this.Log(new daedalusPluginBase.LogEntry("ShowSomeInfos", "running on " + Environment.MachineName + "\nas " + Environment.UserDomainName + "\\" + Environment.UserName + ""));

                Thread.Sleep(5000);
            }
        }
    }
}
