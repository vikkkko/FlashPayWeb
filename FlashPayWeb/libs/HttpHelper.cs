﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace FlashPayWeb.libs
{
    public class HttpHelper
    {
        /// <summary>
        /// 同步get请求
        /// </summary>
        /// <param name="url">链接地址</param>    
        /// <param name="formData">写在header中的键值对</param>
        /// <returns></returns>
        public static string HttpGet(string url, List<KeyValuePair<string, string>> formData = null)

        {

            HttpClient httpClient = new HttpClient();
            var request = new HttpRequestMessage()
            {
                RequestUri = new Uri(url),
                Method = HttpMethod.Get,
            };

            var res = httpClient.SendAsync(request);
            res.Wait();
            var resp = res.Result;

            Task<string> temp = resp.Content.ReadAsStringAsync();

            temp.Wait();

            return temp.Result;
        }

        /// <summary>
        /// 同步请求post（键值对形式）
        /// </summary>
        /// <param name="uri">网络基址("http://localhost:59315")</param>
        /// <param name="url">网络的地址("/api/UMeng")</param>
        /// <param name="formData">键值对List<KeyValuePair<string, string>> formData = new List<KeyValuePair<string, string>>();formData.Add(new KeyValuePair<string, string>("userid", "29122"));formData.Add(new KeyValuePair<string, string>("umengids", "29122"));</param>
        /// <param name="charset">编码格式</param>
        /// <param name="mediaType">头媒体类型</param>
        /// <returns></returns>
        public static string HttpPost(string url, List<KeyValuePair<string, string>> formData = null, string charset = "UTF-8", string mediaType = "application/x-www-form-urlencoded")
        {
            string tokenUri = url;
            var client = new HttpClient();
            HttpContent content = new FormUrlEncodedContent(formData);
            content.Headers.ContentType = new MediaTypeHeaderValue(mediaType);
            content.Headers.ContentType.CharSet = charset;

            for (int i = 0; i < formData.Count; i++)
            {
                content.Headers.Add(formData[i].Key, formData[i].Value);
            }

            var res = client.PostAsync(tokenUri, content);
            res.Wait();
            HttpResponseMessage resp = res.Result;

            var res2 = resp.Content.ReadAsStringAsync();
            res2.Wait();

            string token = res2.Result;
            return token;
        }

        /// <summary>
        /// 异步请求get(UTF-8)
        /// </summary>
        /// <param name="url">链接地址</param>    
        /// <param name="formData">写在header中的内容</param>
        /// <returns></returns>
        public static async Task<string> HttpGetAsync(string url, List<KeyValuePair<string, string>> formData = null)
        {

            HttpClient httpClient = new HttpClient();

            var request = new HttpRequestMessage()
            {
                RequestUri = new Uri(url),
                Method = HttpMethod.Get,
            };

            var resp = await httpClient.SendAsync(request);
            resp.EnsureSuccessStatusCode();

            string token = await resp.Content.ReadAsStringAsync();

            return token;
        }

        /// <summary>
        /// 异步请求post（键值对形式,可等待的）
        /// </summary>
        /// <param name="uri">网络基址("http://localhost:59315")</param>
        /// <param name="url">网络的地址("/api/UMeng")</param>
        /// <param name="formData">键值对List<KeyValuePair<string, string>> formData = new List<KeyValuePair<string, string>>();formData.Add(new KeyValuePair<string, string>("userid", "29122"));formData.Add(new KeyValuePair<string, string>("umengids", "29122"));</param>
        /// <param name="charset">编码格式</param>
        /// <param name="mediaType">头媒体类型</param>
        /// <returns></returns>
        public static async Task<string> HttpPostAsync(string url, List<KeyValuePair<string, string>> formData = null, string charset = "UTF-8", string mediaType = "application/x-www-form-urlencoded")
        {
            string tokenUri = url;
            var client = new HttpClient();
            HttpContent content = new FormUrlEncodedContent(formData);
            content.Headers.ContentType = new MediaTypeHeaderValue(mediaType);
            content.Headers.ContentType.CharSet = charset;

            for (int i = 0; i < formData.Count; i++)
            {
                content.Headers.Add(formData[i].Key, formData[i].Value);
            }

            HttpResponseMessage resp = await client.PostAsync(tokenUri, content);
            resp.EnsureSuccessStatusCode();
            string token = await resp.Content.ReadAsStringAsync();

            return token;
        }

        //流模式post
        public static string Post(string url, string data, int type = 1)
        {
            Encoding encoding = Encoding.UTF8;

            HttpWebRequest req = null;
            HttpWebResponse rsp = null;
            Stream reqStream = null;

            try
            {
                req = WebRequest.CreateHttp(new Uri(url));
                if (type == 1)
                {
                    req.ContentType = "application/json;charset=utf-8";
                }
                else if (type == 2)
                {
                    req.ContentType = "application/xml;charset=utf-8";
                }
                else
                {
                    req.ContentType = "application/x-www-form-urlencoded;charset=utf-8";
                }

                req.Method = "POST";
                req.ContinueTimeout = 60000;

                byte[] postData = encoding.GetBytes(data);
                reqStream = req.GetRequestStreamAsync().Result;
                reqStream.Write(postData, 0, postData.Length);

                rsp = (HttpWebResponse)req.GetResponseAsync().Result;
                string result = GetResponseAsString(rsp, encoding);

                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                // 释放资源
                if (reqStream != null)
                {
                    reqStream.Close();
                    reqStream = null;
                }
                if (rsp != null)
                {
                    rsp.Close();
                    rsp = null;
                }
                if (req != null)
                {
                    req.Abort();

                    req = null;
                }
            }
        }

        private static string GetResponseAsString(HttpWebResponse rsp, Encoding encoding)
        {
            Stream stream = null;
            StreamReader reader = null;

            try
            {
                // 以字符流的方式读取HTTP响应
                stream = rsp.GetResponseStream();
                reader = new StreamReader(stream, encoding);

                return reader.ReadToEnd();
            }
            finally
            {
                // 释放资源
                if (reader != null)
                    reader.Close();
                if (stream != null)
                    stream.Close();

                reader = null;
                stream = null;

            }
        }
    }
}
