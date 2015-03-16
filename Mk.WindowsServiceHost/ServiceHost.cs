using System;
using System.ServiceProcess;
using log4net;
using MK.Service;

namespace Mk.WindowsServiceHost
{
    public partial class ServiceHost : ServiceBase
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private Service _service;

        public ServiceHost()
        {
            InitializeComponent();
            AutoLog = true;
            CanPauseAndContinue = false;
            CanStop = true;

            // ReSharper disable DoNotCallOverridableMethodsInConstructor
            EventLog.Log = "MK";
            EventLog.Source = "MK.Service";
            // ReSharper restore DoNotCallOverridableMethodsInConstructor
        }

        protected override void OnStart(string[] args)
        {
            log4net.Config.XmlConfigurator.Configure();
            Log.Info("Starting up MK Service...");

            try
            {
                _service = new Service();
                _service.Start();

            }
            catch (Exception e)
            {
                Log.Fatal("Service start failed on exception!", e);
                throw;
            }
        }

        protected override void OnStop()
        {
            Log.Info("Stopping Service...");
            _service.Dispose();
        }
    }
}
