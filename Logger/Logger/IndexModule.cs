using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Nancy;

namespace Logger
{
    public class IndexModule : NancyModule
    {
        public IndexModule()
        {
            Get["/"] = parameters => View["index"];
            Get["/LogResx"] = parameters =>
            {
                
            };
            Post["/AddLogx"] = parameter =>
            {
                if (!VerifyData(Request)) return HttpStatusCode.BadRequest;
                Dictionary<string, string> dictionary =
                    LogEntry.MemberList.ToDictionary<string, string, string>(entry => entry,
                        entry => Request.Form[entry]);
                StreamWriter stream = File.CreateText(
                    HttpContext.Current.Server.MapPath(string.Format(@"~/Out/{0}_{1}.txt",
                        DateTime.Now.ToString("yyyyMMddHHmmssfff"), Guid.NewGuid().ToString())));
                stream.WriteLine(string.Join("~", dictionary.Select(t => t.Key + "-=-" + t.Value).ToArray()));
                stream.Close();
                return HttpStatusCode.Accepted;
            };
            Get["/schema"] = x => string.Join("\n", LogEntry.MemberList);
        }

        private static bool VerifyData(Request request)
        {
            DynamicDictionary reqDictionary = request.Form as DynamicDictionary;
            if (reqDictionary == null) return false;
            return LogEntry.MemberList.Aggregate(false, (current, entry) => current || reqDictionary.ContainsKey(entry));
        }

    }

    static class LogEntry
    {
        public static readonly string[] MemberList = {
            "Type","Data","TimeStamp","MachineId"
        };
    }
}