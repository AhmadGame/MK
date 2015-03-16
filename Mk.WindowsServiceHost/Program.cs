using System;
using System.ServiceProcess;
using Mk.WindowsServiceHost;

namespace MK.WindowsServiceHost
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            var servicesToRun = new ServiceBase[] {
				new ServiceHost()
			};
            ServiceBase.Run(servicesToRun);
        }
    }
}
