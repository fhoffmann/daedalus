using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading;

namespace Plugin_InformAboutProcesses
{
    public class Plugin_InformAboutProcessesClass : daedalusPluginBase.PluginBaseClass
    {
        private class KnownProcess
        {
            Process proc = null;
            bool known = false;
            public Process ProcessObj { get { return this.proc; } }
            public bool Known { get { return this.known; } set { this.known = value; } }

            public KnownProcess(Process _proc, bool _known)
            {
                this.proc = _proc;
                this.known = _known;
            }
        }

        private List<Process> listOfProcesses = null;

        public Plugin_InformAboutProcessesClass()
            : base("Plugin_InformAboutProcesses", "Scant das System auf Prozesse und meldet Veränderungen")
        {
            this.listOfProcesses = new List<Process>();

            // Initiale Liste erstellen
            this.listOfProcesses.AddRange(Process.GetProcesses());
        }

        protected override void plugin_thread_go()
        {
            Process[] currentProcesses = null;
            Queue<Process> processToKill = new Queue<Process>();
            bool inList = false;
            string msgText = "";

            while (SrvRunning)
            {
                currentProcesses = Process.GetProcesses();
                inList = false;

                foreach (Process procNew in currentProcesses)
                {
                    inList = false;

                    foreach (Process procOld in this.listOfProcesses)
                    {
                        if (procNew.Id == procOld.Id)
                        {
                            inList = true;
                            break;
                        }
                    }

                    if (!inList)
                    {
                        // Nicht in Liste, hinzufügen
                        msgText = "new process: " + procNew.ProcessName;
                        try
                        {
                            if (procNew.MainModule != null)
                            {
                                msgText += " [" + procNew.MainModule.ModuleName + "]";
                            }
                        }
                        catch (Exception) { }
                        msgText += " (Id: " + procNew.Id + ")";
                        this.Log(new daedalusPluginBase.LogEntry(this.Name, msgText, daedalusPluginBase.LogEntry.LogLevel.Information));
                        this.listOfProcesses.Add(procNew);
                    }
                }

                foreach (Process procOld in this.listOfProcesses)
                {
                    inList = false;
                    foreach (Process procNew in currentProcesses)
                    {
                        if (procNew.Id == procOld.Id)
                        {
                            inList = true;
                            break;
                        }
                    }

                    if (!inList)
                    {
                        // Kein aktueller Prozess, löschen lassen
                        msgText = "killed process: " + procOld.ProcessName;
                        try
                        {
                            if (procOld.MainModule != null)
                            {
                                msgText += " [" + procOld.MainModule.ModuleName + "]";
                            }
                        }
                        catch (Exception) { }
                        msgText += " (Id: " + procOld.Id + ")";
                        this.Log(new daedalusPluginBase.LogEntry(this.Name, msgText, daedalusPluginBase.LogEntry.LogLevel.Information));

                        processToKill.Enqueue(procOld);
                    }
                }

                while (processToKill.Count > 0)
                {
                    this.listOfProcesses.Remove(processToKill.Dequeue());
                }


                // Alle 10 Sekunden
                Thread.Sleep(10000);
            }
        }
    }
}
