using System;
using System.ServiceModel;
using System.ServiceModel.Web;
using log4net;
using Nancy.Hosting.Wcf;

namespace MK.Service.Web
{
    public class WebHost : IDisposable
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(WebHost));
        private WebServiceHost _webServiceHost;

        public void Start(Service service)
        {
            var uri = new Uri("http://localhost:1234");
            var bootstrapper = new Bootstrapper(service);

            Nancy.Json.JsonSettings.MaxJsonLength = Int32.MaxValue;

            Log.Info("Starting web service on port 1234");
            try
            {
                _webServiceHost = new WebServiceHost(new NancyWcfGenericService(bootstrapper), uri);
                _webServiceHost.AddServiceEndpoint(typeof(NancyWcfGenericService), new WebHttpBinding(), "");
                _webServiceHost.Open();
            }
            catch (Exception e)
            {
                Log.Fatal("Failed to start web service! Terminating.", e);
                throw;
            }

            Log.Info("Web host started!");
        }

        public void Dispose()
        {
            if (_webServiceHost != null)
            {
                _webServiceHost.Close();
                Log.Info("Web service shut down!");
            }
        }
    }
}
