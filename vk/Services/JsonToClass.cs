using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using Newtonsoft.Json;
using System.Text;


namespace vk.Services
{
    // Сводка:
    //     Класс для получения JSON и преобразования его в класс.
    public class JsonToClass
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        public static string Cp1251ToUTF8(string str)
        {
            var cp1251 = System.Text.Encoding.GetEncoding(1251);
            var utf8 = new System.Text.UTF8Encoding();
            return utf8.GetString(cp1251.GetBytes(str));
        }
        public static T _download_serialized_json_data<T>(string url) where T : new() {
          using (var w = new WebClient()) {
            var json_data = string.Empty;
            // attempt to download JSON data as a string
            try {
                json_data = Cp1251ToUTF8(w.DownloadString(url));
                //logger.Info("Request response: "+json_data);
            }
            catch (Exception e) {
                logger.Error(e.Message);
            }
            // if string with JSON data is not empty, deserialize it to class and return its instance 
            return !string.IsNullOrEmpty(json_data) ? JsonConvert.DeserializeObject<T>(json_data) : new T();
          }
        }
    }
}