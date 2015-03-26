using System;
using Nancy;
using Nancy.ModelBinding;

namespace MK.Service.Web.Modules
{
    public class AppModule : NancyModule
    {
        public AppModule(Service service) : base("/")
        {
            Get["/"] = _ => Response.AsRedirect("/index.html");
            Get["index.html"] = _ => View["Web/Views/App/index.html"];

            Get["/admin"] = _ => Response.AsRedirect("/admin.html");
            Get["admin.html"] = _ => View["Web/Views/App/admin.html"];

            Post["/question"] = _ =>
            {
                try
                {
                    var question = this.Bind<Question>();
                    service.SaveQuestion(question);
                    return HttpStatusCode.OK;
                }
                catch (Exception)
                {
                    return HttpStatusCode.InternalServerError;
                }
            };

            Get["/questions/{number}"] = p =>  service.GetQuestions(p.number);
        }
    }
}
