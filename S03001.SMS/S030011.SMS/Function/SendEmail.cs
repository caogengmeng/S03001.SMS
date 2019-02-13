using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace S030011.SMS.Function
{
    public class SendEmail
    {

        #region 加密 

        /// <summary>
        /// 加密 
        /// </summary>
        public static string Md5WeChat(string rawPass)
        {
            // 创建MD5类的默认实例：MD5CryptoServiceProvider  
            MD5 md5 = MD5.Create();
            char[] charArray = rawPass.ToCharArray();
            byte[] bs = new byte[charArray.Length];
            for (int i = 0; i < charArray.Length; i++)
            {
                bs[i] = (byte)charArray[i];
            }

            byte[] hs = md5.ComputeHash(bs);
            StringBuilder sb = new StringBuilder();
            foreach (byte b in hs)
            {
                // 以十六进制格式格式化  
                sb.Append(b.ToString("x2"));
            }

            return sb.ToString();
        }
        public static string Md5Encoding(string rawPass)
        {
            rawPass = rawPass + "tamboo";
            // 创建MD5类的默认实例：MD5CryptoServiceProvider  
            MD5 md5 = MD5.Create();
            char[] charArray = rawPass.ToCharArray();
            byte[] bs = new byte[charArray.Length];
            for (int i = 0; i < charArray.Length; i++)
            {
                bs[i] = (byte)charArray[i];
            }
            byte[] hs = md5.ComputeHash(bs);
            StringBuilder sb = new StringBuilder();
            foreach (byte b in hs)
            {
                // 以十六进制格式格式化  
                sb.Append(b.ToString("x2"));
            }
            return sb.ToString();
        }
        #endregion

        #region 时间戳

        /// <summary>
        /// 时间戳
        /// </summary>
        /// <param name="flag"></param>
        /// <returns></returns>
        public static string GetTimeStamp(int flag)
        {
            DateTime ts = DateTime.UtcNow;
            if (flag == 1)
            {
                return ts.ToString("yyyy-MM-dd HH:mm:ss");
            }
            else
            {
                return ts.ToString("yyyy/MM/dd HH:mm:ss");
            }
        }

        #endregion

        #region

        /// <summary>
        /// 发送
        /// </summary>
        /// <param name="url"></param>
        /// <param name="strContent"></param>
        public static string PostFunction(string url, string strContent)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/json";
            using (StreamWriter dataStream = new StreamWriter(request.GetRequestStream()))
            {
                dataStream.Write(strContent);
                dataStream.Close();
            }

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            string encoding = response.ContentEncoding;
            if (encoding == null || encoding.Length < 1)
            {
                encoding = "UTF-8"; //默认编码  
            }

            StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(encoding));
            string retString = reader.ReadToEnd();
            return retString;
        }

        #endregion

        public static string SendEms(IConfiguration configuration, string receiver, string emailContent)
        {

            try
            {
                string code = "0002";
                string serviceAddress =configuration["MCserviceAddress"];
                //总字符串
                string format = "{{\"uid\":\"{0}\",\"code\":\"{1}\",\"mode\":\"{2}\",\"timestamp\":\"{3}\",\"channel\":\"{4}\",\"data\": [{{\"id\":\"{5}\",\"receiver\":\"{6}\",\"emailContent\":\"{7}\",\"subject\":\"{8}\",\"priority\":\"{9}\",\"origin\":\"{10}\",\"emailId\":\"{11}\",\"deptId\":\"{12}\",\"sendTime\":\"{13}\",\"storageTime\":\"{14}\"}}],\"signCode\":\"{15}\"}}";
                string uid = "0002";
                string Mode = null;
                //string timestamp = "2019/02/12 05:05:58";
                string timestamp = GetTimeStamp(2);
                string SendTime = GetTimeStamp(1);
                // string SendTime = "2019-02-12 05:05:58";
                string key = "encrypt";
                string subject = "邮件测试";
                string priority = "5";
                string deptId = "0003";
                //date字符串
                string Format = "[{{id={0}, receiver={1}, emailContent={2}, subject={3}, priority={4}, origin={5}, emailId={6}, deptId={7}, sendTime={8}, storageTime={9}}}]";
                string data = string.Format(Format, "null", receiver, emailContent, subject, priority, "null", "null", deptId, SendTime,"null");
                string signCode = key + data + timestamp + key;
                string md5SignCode = Md5Encoding(signCode);
                //邮件内容信息，必须将换行改为\r\n，对面才能接收到\r\n。
                string emailContenttmp = emailContent.Replace("\r\n", "\\r\\n").Replace("\n", "\\n").Replace("\"", "\\\"");
                string strContent = string.Format(format, uid, code, Mode, timestamp, "null", "null", receiver, emailContenttmp, subject, priority, "null", "null", deptId, SendTime, "null", md5SignCode);
                string retString = PostFunction(serviceAddress, strContent);
                return retString;

            }
            catch (Exception ex)
            {
                throw;
            }


        }
    }
}
