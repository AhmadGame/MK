using System;
using System.Linq;
using log4net;
using Nancy;
using Nancy.Bootstrapper;
using Nancy.Conventions;
using Nancy.TinyIoc;

namespace MK.Service.Web
{
    public class Bootstrapper : DefaultNancyBootstrapper
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(Bootstrapper));
        private readonly Service _service;

        public Bootstrapper(Service service)
        {
            _service = service;
        }

        protected override void ApplicationStartup(TinyIoCContainer container, IPipelines pipelines)
        {
            base.ApplicationStartup(container, pipelines);

            const string beforeHookName = "game-before";
            const string afterHookName = "game-after";

            var beforeHook = new PipelineItem<Func<NancyContext, Response>>(
                beforeHookName, BeforeRequest);

            if (pipelines.BeforeRequest.PipelineItems.SingleOrDefault(i => i.Name == beforeHookName) == null)
                pipelines.BeforeRequest.AddItemToStartOfPipeline(beforeHook);

            var afterHook = new PipelineItem<Action<NancyContext>>(
                afterHookName, AfterRequest);

            if (pipelines.AfterRequest.PipelineItems.SingleOrDefault(i => i.Name == afterHookName) == null)
                pipelines.AfterRequest.AddItemToStartOfPipeline(afterHook);
        }

        protected Response BeforeRequest(NancyContext context)
        {
            Log.InfoFormat("HTTP Request:  {0} {1} from {2}",
                context.Request.Method, context.Request.Path, context.Request.UserHostAddress);
            return null;
        }

        protected void AfterRequest(NancyContext context)
        {
            Log.InfoFormat("HTTP Response: {0} {1} = {2} from {3}",
                context.Request.Method, context.Request.Path, context.Response.StatusCode, context.Request.UserHostAddress);
        }

        protected override void ConfigureConventions(NancyConventions nancyConventions)
        {
            base.ConfigureConventions(nancyConventions);

            // Nancy can not figure out some extensions without a ".", so add that
            // to all supported extensions just to be safe.

            nancyConventions.StaticContentsConventions.Add(
                StaticContentConventionBuilder.AddDirectory(@"static/css", @"/Web/Views/static/css",
                    new[] { ".css", "css" })
                );

            nancyConventions.StaticContentsConventions.Add(
                StaticContentConventionBuilder.AddDirectory(@"static/img", @"/Web/Views/static/img",
                    new[] { ".png", "png", ".jpg", "jpg", ".gif", "gif", ".ico", "ico" })
                );

            nancyConventions.StaticContentsConventions.Add(
                StaticContentConventionBuilder.AddDirectory(@"static/js", @"/Web/Views/static/js",
                    new[] { ".js", "js" })
                );

            nancyConventions.StaticContentsConventions.Add(
                StaticContentConventionBuilder.AddDirectory(@"/static/font", @"/Web/Views/static/font",
                    new[] { ".css", "css", ".otf", "otf", ".eot", "eot", ".svg", "svg", ".ttf", "ttf", ".woff", "woff" })
                );
        }

        protected override void ConfigureApplicationContainer(TinyIoCContainer container)
        {
            base.ConfigureApplicationContainer(container);
            container.Register(_service);
        }
    }
}