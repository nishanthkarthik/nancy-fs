using System;
using System.IO;
using Nancy;

namespace SimpleServer
{
    public class IndexModule : NancyModule
    {
        public IndexModule()
        {
            Get["/"] = parameters =>
            {
                return View["index"];
            };
            Get["/about"] = (parameters =>
            {
                return "<h3>Nancy self hosting server running at <b>@wpdevvy</b></h3>";
            });

            Get["/fur"] = (parameters =>
            {
                Console.WriteLine("fur file returned");
                Stream stream = File.OpenRead(@"E:\Downloads_FDM\FurMark.exe");
                return Response.FromStream(stream, "application/exe");
            });
        }
    }
}