using FD.Simple.Utils.Agent;
using FD.Simple.Utils.Logging;
using FD.Simple.Utils.Serialize;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using S030011.SMS.Moudle;
using System.Linq;
using S030011.SMS.Interface;
using S030011.SMS.Function;

namespace S030011.SMS.Service
{
    public class SendEmsService : BaseFoo
    {
        private IConfiguration _configuration;
        private IFDLogger _fDLogger;
        private HttpContext _context;

        public SendEmsService(IFDLogger fDLogger, IConfiguration configuration, IHttpContextAccessor context)
        {
            this._fDLogger = fDLogger;
            this._configuration = configuration;
            this._context = context.HttpContext;
        }

        [Routing(EHttpMethod.HttpPost, "SMS/SendEms")]
        [MessageFormat(EMessageType.Original)]
        [Anonymous]
        public string SendEmsasync(string Emailaddress)
        {
            var request = _context.Request;
            System.Console.WriteLine("接收到 Webhook 请求。");
            var content = string.Empty;
            try
            {
                using (var reader = new StreamReader(request.Body))
                    content = reader.ReadToEnd();
                System.Console.WriteLine($"接收到 Webhook 请求：{content}");
                var con = content.Replace("{", "").Replace("}", "").Replace('\"', '"').Replace('"', '\"');
                SendEmail.SendEms(_configuration, Emailaddress, con);
            }
            catch (Exception e)
            {
                System.Console.WriteLine($"处理 Webhook 失败，内容：{content}。");
                throw;
            }
            return "1";
        }

        [Routing(EHttpMethod.All, "SMS/Err")]
        [MessageFormat(EMessageType.Original)]
        [Anonymous]
        public string SendErr(string Emailaddress)
        {
            // string con = "\"id\": \"5c614462cd46e125a4210e6e\",\"url\": \"http://10.50.132.108:50000/#/event/5c614462cd46e125a4210e6e\",\"occurrence_date\": \"2019-02-11T17:46:10.448783+08:00\",\"type\": \"error\",\"message\": \"Value cannot be null.Parameter name: source\",\"project_id\": \"5c3ea1a7cd46e120f8939485\",\"project_name\": \"BaseProject\",\"organization_id\": \"5c3ea1a7cd46e120f8939484\",\"organization_name\": \"NetCore\",\"stack_id\": \"5c614462cd46e125a4210e6d\",\"stack_url\": \"http://10.50.132.108:50000/#/stack/5c614462cd46e125a4210e6d\",\"stack_title\": \"Value cannot be null.Parameter name: source\",\"total_occurrences\": 1,\"first_occurrence\": \"2019-02-11T09:46:10.448783+00:00\",\"last_occurrence\": \"2019-02-11T09:46:10.448783+00:00\",\"is_new\": true,\"is_regression\": false,\"is_critical\": false";
            // var ret = con.Replace("\"", "");
            string ret = "\r\n  \"id\": \"5c6230d2cd46e111b4da9ff3\",\r\n  \"url\": \"http://10.50.132.108:50000/#/event/5c6230d2cd46e111b4da9ff3\",\r\n  \"occurrence_date\": \"2019-02-12T10:34:58.2428293+08:00\",\r\n  \"type\": \"error\",\r\n  \"message\": \"Cannot deserialize the current JSON object (e.g. \"name\":\"value\"}) into type 'System.Collections.Generic.List`1[M100011.LoanOrder.Model.CAVEHICLEEXEntity]' because the type requires a JSON array (e.g. [1,2,3]) to deserialize correctly.\r\nTo fix this error either change the JSON to a JSON array (e.g. [1,2,3]) or change the deserialized type so that it is a normal .NET type (e.g. not a primitive type like integer, not a collection type like an array or List<T>) that can be deserialized from a JSON object. JsonObjectAttribute can also be added to the type to force it to deserialize from a JSON object.\r\nPath 'data', line 1, position 8.\",\r\n  \"project_id\": \"5c3ea1a7cd46e120f8939485\",\r\n  \"project_name\": \"BaseProject\",\r\n  \"organization_id\": \"5c3ea1a7cd46e120f8939484\",\r\n  \"organization_name\": \"NetCore\",\r\n  \"stack_id\": \"5c623163cd46e111b4da9ff2\",\r\n  \"stack_url\": \"http://10.50.132.108:50000/#/stack/5c623163cd46e111b4da9ff2\",\r\n  \"stack_title\": \"Cannot deserialize the current JSON object (e.g. \"name\":\"value\"}) into type 'System.Collections.Generic.List`1[M100011.LoanOrder.Model.CAVEHICLEEXEntity]' because the type requires a JSON array (e.g. [1,2,3]) to deserialize correctly.\nTo fix this error either change the JSON to a JSON array (e.g. [1,2,3]) or change the deserialized type so that it is a normal .NET type (e.g. not a primitive type like integer, not a collection type like an array or List<T>) that can be deserialized from a JSON object. JsonObjectAttribute can also be added to the type to force it to deserialize from a JSON object.\nPath 'data', line 1, position 8.\",\r\n  \"total_occurrences\": 1,\r\n  \"first_occurrence\": \"2019-02-12T02:34:58.2428293+00:00\",\r\n  \"last_occurrence\": \"2019-02-12T02:34:58.2428293+00:00\",\r\n  \"is_new\": true,\r\n  \"is_regression\": false,\r\n  \"is_critical\": false\r\n";
            SendEmail.SendEms(_configuration, Emailaddress, ret);
            string a = null;
            //int b = a.Count();
            var i = new int[2];
            i[3] = 1;
            return null;
        }
    }
}
