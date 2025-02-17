namespace ProcessingLibrary
{
    public static class DataCorrectness
    {
        /// <summary>
        /// Проверка корректности структуры из файла.
        /// </summary>
        /// <param name="strings"></param>
        /// <param name="defaultFirstString"></param>
        /// <param name="defaultSecondString"></param>
        /// <returns></returns>
        public static bool CorectnessOfStructure(string[] strings, string defaultFirstString, string defaultSecondString)
        {
            bool flag = true;
            // Совпадение заголовков 1 строки.
            if (strings[0] != defaultFirstString[..(defaultFirstString.Length - 2)] ||
                strings[1] != defaultSecondString[..(defaultSecondString.Length - 2)])
            {
                flag = false;
            }
            if (strings.Length > 2)
            {
                for (int i = 2; i < strings.Length - 1; i++)
                {
                    string str = strings[i];
                    string[] objectInfo = str.Split("\";\"");
                    objectInfo[0] = objectInfo[0].TrimStart('\"');
                    objectInfo[10] = objectInfo[10].TrimEnd('\"');
                    // Несовпадение колонок с числовыми типами.
                    if (!(objectInfo.Length == 11 && int.TryParse(objectInfo[1], out int a) &&
                        double.TryParse(objectInfo[6], out double b) &&
                        double.TryParse(objectInfo[7], out double c) &&
                        long.TryParse(objectInfo[8], out long d)))
                    {
                        flag = false;
                    }
                }
            }
            else
            {
                flag = false;
            }
            return flag;
        }
    }
}

