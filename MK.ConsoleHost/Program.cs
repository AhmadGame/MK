using System;
using log4net;

namespace MK.ConsoleHost
{
    class Program
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(Program));
        static void Main()
        {
            log4net.Config.XmlConfigurator.Configure();

            Log.Info("Starting up Service");
            using (var service = new Service.Service())
            {
                service.Start();
                Console.WriteLine(@"Press any key to quit...");
                Console.ReadKey();
            }
        }
    }
}
