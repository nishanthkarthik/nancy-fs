using System.IO;
using Nancy.Responses;

namespace NetServer
{
    using Nancy;

    public class IndexModule : NancyModule
    {
        public IndexModule()
        {
            Get["/"] = (parameters =>
            {
                return View["index"];
            });

            Get["/furmark"] = (parameters =>
            {
                GenericFileResponse genericFileResponse = new GenericFileResponse(@);
            });
        }
    }
}