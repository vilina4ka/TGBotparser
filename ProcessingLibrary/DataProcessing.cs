namespace ProcessingLibrary
{
	public static class DataProcessing
	{
        /// <summary>
        /// Выбор значений для выборки из файла.
        /// </summary>
        /// <param name="electrocars"></param>
        /// <param name="field"></param>
        /// <returns></returns>
        public static List<string> SelectValuesForSample(List<MyElectrocarPower> electrocars, string field)
        {
            List<string> result = new List<string>();
            switch(field)
            {
                case "AdmArea":
                    result = (from value in electrocars
                              select value.AdmArea).Distinct().ToList();
                    break;
                case "District":
                    result = (from value in electrocars
                              select value.District).Distinct().ToList();
                    break;
                case "Longitude_WGS84":
                    result = (from value in electrocars
                                              select value.Longitude).Distinct().ToList();
                    break;
                case "Latitude_WGS84":
                    result = (from value in electrocars
                                              select value.Latitude).Distinct().ToList();
                    break;

            }
            return result;
        }
        /// <summary>
        /// Сортировка в двух направлениях.
        /// </summary>
        /// <param name="electrocars">Массив объектов.</param>
        /// <param name="reverse">Показывает, в какую сторону сортировка.</param>
        public static void Sorting(ref List<MyElectrocarPower> electrocars, bool reverse=false)
        {

            if (!reverse)
            {
                electrocars = (from electrocar in electrocars
                                   orderby electrocar.AdmArea ascending
                                   select electrocar).ToList();
            }
            else
            {
                electrocars = (from electrocar in electrocars
                                    orderby electrocar.AdmArea descending
                                    select electrocar).ToList();
            }

        }
        /// <summary>
        /// Выборка по AdmArea.
        /// </summary>
        /// <param name="electrocars"></param>
        /// <param name="value"></param>
        public static void SampleAdmArea(ref List<MyElectrocarPower> electrocars, string value)
        {
            electrocars = (from electrocar in electrocars
                               where (electrocar.AdmArea == value)
                               select electrocar).ToList();
        }
        /// <summary>
        /// Выборка по District.
        /// </summary>
        /// <param name="electrocars"></param>
        /// <param name="value"></param>
        public static void SampleDistrict(ref List<MyElectrocarPower> electrocars, string value)
        {
           
            electrocars = (from electrocar in electrocars
                               where (electrocar.District == value)
                               select electrocar).ToList();
        }
        /// <summary>
        /// Выборка по 3 параметрам.
        /// </summary>
        /// <param name="electrocars"></param>
        /// <param name="valuesForSample"></param>
        public static void SampleThreeParametres(ref List<MyElectrocarPower> electrocars, List<string> valuesForSample)
        { 
            electrocars = (from electrocar in electrocars
                               where (electrocar.AdmArea == valuesForSample[0]
                               && electrocar.Longitude == valuesForSample[1] && electrocar.Latitude == valuesForSample[2])
                               select electrocar).ToList();
        }
    }
}

