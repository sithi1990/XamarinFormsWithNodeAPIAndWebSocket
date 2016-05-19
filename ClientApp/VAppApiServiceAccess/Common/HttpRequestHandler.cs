using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using Newtonsoft.Json;

namespace VAppApiServiceAccess.Common
{
    public class HttpRequestHandler
    {
        public string AccessToken { get; set; }
        public string Url { get; set; }
        public string Method { get; set; }
        public TResult SendRequest<T,TResult>(T t)
        {
            
            HttpWebRequest request = WebRequest.Create(Url) as HttpWebRequest;

            if(!String.IsNullOrEmpty(AccessToken))
            {
                request.Headers.Add("x-access-token", AccessToken);
            }          
            request.Method = Method;
            request.ContentType = "application/json";

            if(t!=null)
            {
                string sb = JsonConvert.SerializeObject(t, Formatting.Indented);
                Byte[] bt = Encoding.UTF8.GetBytes(sb);
                Stream st = request.GetRequestStream();
                st.Write(bt, 0, bt.Length);
                st.Close();
            }
            

            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    throw new Exception(String.Format("Server error (HTTP {0}: {1}).", response.StatusCode, response.StatusDescription));
                }
                Stream stream1 = response.GetResponseStream();
                StreamReader sr = new StreamReader(stream1);
                string result = sr.ReadToEnd();
                return JsonConvert.DeserializeObject<TResult>(result);

            }
        }

        public TResult SendRequest<TResult>()
        {
            return SendRequest<object,TResult>(null);
        }
    }
}
