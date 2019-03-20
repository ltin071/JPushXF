using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace JPushWebAPI.Controllers
{
    public class DefaultController : ApiController
    {
        [System.Web.Http.Route("api/notification/all")]
        [System.Web.Http.HttpPost]
        public async Task<IHttpActionResult> All()
        {
            NameValueCollection requestBody = await Request.Content.ReadAsFormDataAsync();
            string message = requestBody["message"];
            string returnMessage = SendAction(message);
            return Ok(returnMessage);
        }
        [System.Web.Http.Route("api/notification/single")]
        [System.Web.Http.HttpPost]
        public async Task<IHttpActionResult> Single()
        {
            NameValueCollection requestBody = await Request.Content.ReadAsFormDataAsync();
            string message = requestBody["message"];
            string deviceId = requestBody["device_id"];
            string returnMessage = SendAction(message, deviceId);
            return Ok(returnMessage);
        }

        public static string GetCID()
        {
            string APIKey = ConfigurationManager.AppSettings["APIKey"];
            string SecretKey = ConfigurationManager.AppSettings["SecretKey"];

            HttpWebRequest httpWebRequest = (HttpWebRequest)HttpWebRequest.Create("https://api.jpush.cn/v3/push/cid");
            httpWebRequest.Method = "get";
            httpWebRequest.Headers.Add(string.Format("Authorization: Basic {0}", Base64Encode(APIKey + ":" + SecretKey)));
            using (WebResponse tResponse = httpWebRequest.GetResponse())
            {
                using (Stream dataStreamResponse = tResponse.GetResponseStream())
                {
                    using (StreamReader tReader = new StreamReader(dataStreamResponse))
                    {
                        String sResponseFromServer = tReader.ReadToEnd();
                        string str = sResponseFromServer;
                        CIDReturn cidReturn = JsonConvert.DeserializeObject<CIDReturn>(str);
                        if (cidReturn != null && cidReturn.cidlist != null && cidReturn.cidlist.Count > 0)
                        {
                            return cidReturn.cidlist[0];
                        }
                        else
                        {
                            return string.Empty;
                        }
                    }
                }
            }
        }
        public static string SendAction(string pushMessage, string deviceId = "all")
        {
            string cid = GetCID();
            string APIKey = ConfigurationManager.AppSettings["APIKey"];
            string SecretKey = ConfigurationManager.AppSettings["SecretKey"];

            HttpWebRequest httpWebRequest = (HttpWebRequest)HttpWebRequest.Create("https://api.jpush.cn/v3/push");
            httpWebRequest.Method = "post";
            httpWebRequest.ContentType = "application/json";
            var data = new
            {
                cid = cid,
                platform = "all",
                audience = deviceId,
                notification = new {
                    android = new {
                         alert = pushMessage, 
                         title = "JPush test", 
                         builder_id = 3, 
                         style=1 ,
                         alert_type=1 ,
                         big_text="big text content",
                         big_pic_path="picture url",
                         priority=0, 
                         category="category str",
                         large_icon= "http://www.jiguang.cn/largeIcon.jpg"
                    },
                    ios = new {
                        alert = pushMessage, 
                        sound = "sound.caf", 
                        badge = 1, 
                        extras = new {
                            news_id = 134, 
                            my_key = "a value"
                        }
                    }
                }
            };
            var serializer = new JavaScriptSerializer();
            var json = serializer.Serialize(data);
            Byte[] byteArray = Encoding.UTF8.GetBytes(json);
            httpWebRequest.Headers.Add(string.Format("Authorization: Basic {0}", Base64Encode(APIKey+":"+SecretKey)));
            httpWebRequest.ContentLength = byteArray.Length;
            using (Stream dataStream = httpWebRequest.GetRequestStream())
            {
                dataStream.Write(byteArray, 0, byteArray.Length);
                using (WebResponse tResponse = httpWebRequest.GetResponse())
                {
                    using (Stream dataStreamResponse = tResponse.GetResponseStream())
                    {
                        using (StreamReader tReader = new StreamReader(dataStreamResponse))
                        {
                            String sResponseFromServer = tReader.ReadToEnd();
                            string str = sResponseFromServer;
                            return str;
                        }
                    }
                }
            }
        }
        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public class CIDReturn
        {

            public List<string> cidlist;
        }
    }
}