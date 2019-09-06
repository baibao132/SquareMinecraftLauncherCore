using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.ComponentModel;
using System.Diagnostics;
using System.Web;
using System.Net.Security;
using Newtonsoft.Json;

namespace SikaDeerLauncher
{
    internal sealed class Download
    {
        //获取源码
        internal string getHtml(string url)
        {
            WebClient wc = new WebClient();//创建WebClient对象
            Stream s;
            try
            {
                
                s = wc.OpenRead(url);//访问网址并用一个流对象
                s.ReadTimeout = 3000;
            }
            catch (WebException ex)
            {
                string p = ex.Message;
                return null;
            }
            StreamReader sr = new StreamReader(s, Encoding.UTF8);//把流对象转换
            try
            {
                return sr.ReadToEnd();//返回流转换为字符串
            }
            catch(Exception ex)
            {
                return null;
            }
        }
        internal string Post(string URL,string jsonParas)
        {
            string strURL = URL;
            //创建一个HTTP请求  
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(strURL);
            //Post请求方式  
            request.Method = "POST";
            //内容类型
            request.ContentType = "application/json";

            //设置参数，并进行URL编码 

            string paraUrlCoded = jsonParas;//System.Web.HttpUtility.UrlEncode(jsonParas);   

            byte[] payload;
            //将Json字符串转化为字节  
            payload = System.Text.Encoding.UTF8.GetBytes(paraUrlCoded);
            //设置请求的ContentLength   
            request.ContentLength = payload.Length;
            //发送请求，获得请求流 

            Stream writer;
            try
            {
                writer = request.GetRequestStream();//获取用于写入请求数据的Stream对象
            }
            catch (Exception)
            {
                writer = null;
                return "";
            }
            //将请求参数写入流
            writer.Write(payload, 0, payload.Length);
            writer.Close();//关闭请求流
                           // String strValue = "";//strValue为http响应所返回的字符流
            HttpWebResponse response;
            try
            {
                //获得响应流
                response = (HttpWebResponse)request.GetResponse();
            }
            catch (WebException ex)
            {
                response = ex.Response as HttpWebResponse;
            }
            Stream s = response.GetResponseStream();
            //  Stream postData = Request.InputStream;
            StreamReader sRead = new StreamReader(s);
            string postContent = sRead.ReadToEnd();
            sRead.Close();
            return postContent;//返回Json数据
        }
        internal string GetWebRequest(string url)
        {
            Uri uri = new Uri(url);
            WebRequest myReq = WebRequest.Create(uri);
            myReq.Timeout = 1000;
            WebResponse result = myReq.GetResponse();
            Stream receviceStream = result.GetResponseStream();
            StreamReader readerOfStream = new StreamReader(receviceStream, System.Text.Encoding.GetEncoding("utf-8"));
            string strHTML = readerOfStream.ReadToEnd();
            readerOfStream.Close();
            receviceStream.Close();
            result.Close();
            return strHTML;
        }
    }
}
