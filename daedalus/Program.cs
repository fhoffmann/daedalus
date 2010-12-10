using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace daedalus
{
    class Program
    {
        static void Main(string[] args)
        {
            daedalusPluginBase.LogClass.evt_newLogMessage += new daedalusPluginBase.LogClass.evt_newLogMessageHandle(LogClass_evt_newLogMessage);

            Application.EnableVisualStyles();
            Application.Run(new MainControlClass());
        }

        static void LogClass_evt_newLogMessage(daedalusPluginBase.LogEntry le)
        {
            Console.WriteLine(le.Title + "\n" + le.Message + "\n");
        }
    }
}
