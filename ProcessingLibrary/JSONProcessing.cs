using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace ProcessingLibrary
{
    public class JSONProcessing
    {
        // Конструктор без параметров.
        public JSONProcessing() { }
        /// <summary>
        /// Чтение JSON.
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static List<MyElectrocarPower> Read(Stream stream)
        {
            try
            {
                List<MyElectrocarPower> result = new List<MyElectrocarPower>();
                stream.Seek(0, SeekOrigin.Begin);
                using (StreamReader sr = new StreamReader(stream))
                {
                    string text = sr.ReadToEnd().Replace('\n', ' ');
                    result = JsonSerializer.Deserialize<List<MyElectrocarPower>>(text);
                }

                return result;
            }
            catch(Exception)
            {
                throw new ArgumentException();
            }
           

        }
        /// <summary>
        /// Запись в JSON.
        /// </summary>
        /// <param name="electrocars"></param>
        /// <returns></returns>
        public static Stream Write(List<MyElectrocarPower> electrocars)
        {
            string text;
            using (StreamWriter sw = new StreamWriter("electrocarpower_result.json"))
            {
                text = JsonSerializer.Serialize(electrocars, new JsonSerializerOptions
                { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping });
                sw.Write(text);
            }
            byte[] bytes = Encoding.UTF8.GetBytes(text);
            MemoryStream stream = new MemoryStream(bytes);
            return stream;
        }
    }
}

