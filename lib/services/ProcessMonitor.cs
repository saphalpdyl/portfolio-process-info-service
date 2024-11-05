using _02_PingProcessInformationToWebsiteService.lib.structs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _02_PingProcessInformationToWebsiteService.lib.services
{
    internal class ProcessMonitor
    {
        private readonly ProcessTarget[] ProcessTargets;

        public ProcessMonitor(ProcessTarget[] processTargets)
        {
            ProcessTargets = processTargets;
        }

        private bool IsProcessRunning(String processName)
        {
            Process[] processes = Process.GetProcessesByName(processName);
            return processes.Length > 0;
        }

        public ProcessTargetData[] GetProcessInformation()
        {
            ProcessTargetData[] targetDatas = new ProcessTargetData[ProcessTargets.Length];
            for ( int i = 0; i < ProcessTargets.Length; i++ )
            {
                ProcessTarget target = ProcessTargets[i];
                targetDatas[i] = new ProcessTargetData
                {
                    ApplicationName = target.ApplicationName,
                    IsRunning = IsProcessRunning(target.ProcessName),
                };
            }
            return targetDatas;
        }
    }
}
