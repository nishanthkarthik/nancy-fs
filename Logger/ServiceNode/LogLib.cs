using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using RestSharp.Portable;

namespace ServiceNode
{
    public class LogLib
    {
        public async static Task<bool> CreateLog(string url, Dictionary<string, string> dictionary)
        {
            IRestResponse response;
            try
            {
                using (RestClient restClient = new RestClient(url))
                {
                    RestRequest request = new RestRequest(Method.POST);
                    for (int i = 0; i < dictionary.Count; i++)
                    {
                        request.AddParameter(dictionary.ElementAt(i).Key, dictionary.ElementAt(i).Value,
                            ParameterType.GetOrPost);
                    }
                    response = await restClient.Execute(request);
                }
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.Message);
                throw;
            }
            return response.StatusCode == HttpStatusCode.Accepted;
        }

        public async static Task<string[]> EnumerateParams(string schemaUri)
        {
            using (RestClient client = new RestClient(schemaUri))
            {
                RestRequest request = new RestRequest(Method.GET);
                IRestResponse response = await client.Execute(request);
                if (response.IsSuccess)
                    return Encoding.UTF8.GetString(response.RawBytes, 0, response.RawBytes.Length).Split('\n');
            }
            return null;
        }
    }
}
