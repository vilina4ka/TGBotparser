using System.Text;

namespace ProcessingLibrary
{
    public class CSVProcessing
    {
        // Первая дефолтная строка CSV.
        private static string _defaultFirstString = "\"object_category_Id\";\"ID\";" +
            "\"Name\";\"AdmArea\";\"District\";\"Address\";\"Longitude_WGS84\";" +
            "\"Latitude_WGS84\";\"global_id\";\"geodata_center\";\"geoarea\";\n";
        // Вторая дефолтная строка CSV (на русском).
        private static string _defaultSecondString = "\"object_category_Id\";\"Код\";\"Наименование\";" +
            "\"Административный округ\";\"Район\";\"Адрес\";\"Долгота в WGS-84\";" +
            "\"Широта в WGS-84\";\"global_id\";\"geodata_center\";\"geoarea\";\n";
        // Конструктор без параметров.
        public CSVProcessing() { }
        // Чтение файла.
        public static List<MyElectrocarPower> Read(Stream stream)
        {
            try
            {
                string[] info;
                List<MyElectrocarPower> electrocars = new List<MyElectrocarPower>();
                // Курсор в начало.
                stream.Seek(0, SeekOrigin.Begin);
                // Чтение файла и деление по строкам.
                using (StreamReader streamReader = new StreamReader(stream))
                {
                    string text = streamReader.ReadToEnd();
                    info = text.Split(";\n");
                }
                // Проверка на корретность.
                if (DataCorrectness.CorectnessOfStructure(info, _defaultFirstString, _defaultSecondString))
                {
                    string[] objectInfo;
                    // Деление по полям и инициализация объектов.
                    for (int i = 2; i < info.Length - 1; ++i)
                    {
                        objectInfo = info[i].Split("\";\"");
                        objectInfo[0] = objectInfo[0].TrimStart('\"');
                        objectInfo[10] = objectInfo[10].TrimEnd('\"');
                        electrocars.Add(new MyElectrocarPower(objectInfo[0], objectInfo[1],
                            objectInfo[2], objectInfo[3], objectInfo[4], objectInfo[5],
                            objectInfo[6], objectInfo[7], objectInfo[8], objectInfo[9], objectInfo[10]));
                    }
                }
                else
                {
                    throw new ArgumentException();
                }

                return electrocars;
            }
            catch(Exception)
            {
                throw new ArgumentException();
            }
            
        }
        /// <summary>
        /// Запись в файл CSV.
        /// </summary>
        /// <param name="electrocars"></param>
        /// <returns></returns>
        public static Stream Write(List<MyElectrocarPower> electrocars)
        {
            
            StringBuilder sb = new StringBuilder();
            sb.Append(_defaultFirstString);
            sb.Append(_defaultSecondString + '\n');
            foreach (var line in electrocars)
            {
                sb.Append(line.ToStringCsv() + '\n');
            }
            byte[] bytes = Encoding.UTF8.GetBytes(sb.ToString());
            MemoryStream stream = new MemoryStream(bytes);
            using (StreamWriter sw = new StreamWriter("result.csv"))
            {
                sw.WriteLine(sb.ToString());
            }
            return stream;
        }
    }
}


