using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace SquareMinecraftLauncher.Core
{
    internal class youdao
    {
        /// <summary>
        /// 指定Url地址使用Get 方式获取全部字符串
        /// </summary>
        /// <param name="url">请求链接地址</param>
        /// <returns></returns>
        public static string Get(string url)
        {
            string result = "";
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
            Stream stream = resp.GetResponseStream();
            try
            {
                //获取内容
                using (StreamReader reader = new StreamReader(stream))
                {
                    result = reader.ReadToEnd();
                }
            }
            finally
            {
                stream.Close();
            }
            return result;
        }
        Download web = new Download();
        public string GetChinese(string English)
        {
            Dictionary<String, String> dic = new Dictionary<String, String>();
            string url = "https://openapi.youdao.com/api";
            string q = ReplaceSpecialCharacterV2(English);
            string appKey = "609ac097267a879e";
            string appSecret = "0bvfv3jJyELJP6ls95DtVUuJFUE71ho7";
            string salt = DateTime.Now.Millisecond.ToString();
            dic.Add("from", "auto");
            dic.Add("to", "auto");
            dic.Add("signType", "v3");
            TimeSpan ts = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc));
            long millis = (long)ts.TotalMilliseconds;
            string curtime = Convert.ToString(millis / 1000);
            dic.Add("curtime", curtime);
            string signStr = appKey + Truncate(q) + salt + curtime + appSecret; ;
            string sign = ComputeHash(signStr, new SHA256CryptoServiceProvider());
            dic.Add("q", System.Web.HttpUtility.UrlEncode(q));
            dic.Add("appKey", appKey);
            dic.Add("salt", salt);
            dic.Add("sign", sign);
            string json = Post(url, dic);
            var obj = JsonConvert.DeserializeObject<ydjson.Root>(json);
            var jarray = (JObject)JsonConvert.DeserializeObject(json);

            return obj.translation[0].ToString();
        }

        protected string Post(string url, Dictionary<String, String> dic)
        {
            string result = "";
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            req.Method = "POST";
            req.ContentType = "application/x-www-form-urlencoded";
            StringBuilder builder = new StringBuilder();
            int i = 0;
            foreach (var item in dic)
            {
                if (i > 0)
                    builder.Append("&");
                builder.AppendFormat("{0}={1}", item.Key, item.Value);
                i++;
            }
            byte[] data = Encoding.UTF8.GetBytes(builder.ToString());
            req.ContentLength = data.Length;
            using (Stream reqStream = req.GetRequestStream())
            {
                reqStream.Write(data, 0, data.Length);
                reqStream.Close();
            }
            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
            Stream stream = resp.GetResponseStream();
            using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
            {
                result = reader.ReadToEnd();
            }
            return result;
        }


        public string ReplaceSpecialCharacterV2(string str)
        {
            //List<string> charArr = new List<string>() { "\\", "/", "*", "?", "<", ">", "|", ":", "\"", "[", "]","-","&" };
            string s = str;//charArr.Aggregate(str, (current, c) => current.Replace(c, ""));
            for (int i = 1; i < s.Length; i++)
            {
                for (int j = 65; j <= 90; j++)
                {
                    if (s[i] == (char)j)
                    {
                        s.Insert(i - 1, " ");
                    }
                }
            }
            return s;
        }

        protected static string ComputeHash(string input, HashAlgorithm algorithm)
        {
            Byte[] inputBytes = Encoding.UTF8.GetBytes(input);
            Byte[] hashedBytes = algorithm.ComputeHash(inputBytes);
            return BitConverter.ToString(hashedBytes).Replace("-", "");
        }

        protected static string Truncate(string q)
        {
            if (q == null)
            {
                return null;
            }
            int len = q.Length;
            return len <= 20 ? q : (q.Substring(0, 10) + len + q.Substring(len - 10, 10));
        }
    }
}
