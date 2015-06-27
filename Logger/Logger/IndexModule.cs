using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using Ionic.Zip;
using Ionic.Zlib;
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
                string relPath = string.Format(@"~/Out/{0}.zip", Guid.NewGuid().ToString());
                try
                {
                    CleanupZip(HttpContext.Current.Server.MapPath(@"~/Out"), "*.zip");
                    using (ZipFile zipFile = new ZipFile())
                    {
                        zipFile.CompressionLevel = CompressionLevel.BestSpeed;
                        zipFile.CompressionMethod = CompressionMethod.BZip2;
                        zipFile.AddDirectory(HttpContext.Current.Server.MapPath(@"~/Out"));
                        zipFile.Save(HttpContext.Current.Server.MapPath(relPath));
                        return Response.AsFile(HttpContext.Current.Server.MapPath(relPath));
                    }
                }
                catch (Exception exception)
                {
                    Debug.WriteLine(exception.Message);
                    return HttpStatusCode.InternalServerError;
                }
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

        private void CleanupZip(string folderPath, string fileNameLike)
        {
            DirectoryInfo directory = new DirectoryInfo(folderPath);
            foreach (FileInfo file in directory.EnumerateFiles(fileNameLike))
            {
                file.Delete();
            }
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