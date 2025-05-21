using Newtonsoft.Json;

namespace FiveSQD.WebVerse.Handlers.Javascript.APIs.Data
{
    public class AsyncJSON
    {
        public static void Parse(string rawText, string onComplete)
        {
            System.Threading.Tasks.Task.Run(() =>
            {
                var result = JsonConvert.DeserializeObject<dynamic>(rawText);
                DataAPIHelper.QueueJavascript(onComplete, new object[] { result });
            });
        }

        public static void Stringify(dynamic jsonObject, string onComplete)
        {
            System.Threading.Tasks.Task.Run(() =>
            {
                string result = JsonConvert.SerializeObject(jsonObject);
                DataAPIHelper.QueueJavascript(onComplete, new object[] { result });
            });
        }
    }
}