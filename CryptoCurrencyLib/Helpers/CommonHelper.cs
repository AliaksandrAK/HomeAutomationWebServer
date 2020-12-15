using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CryptoCurrency.Helpers
{
    public class CommonHelper
    {
        public static double GetDouble(string value, double defaultValue)
        {
            double result;

            // Try parsing in the current culture
            if (!double.TryParse(value, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.CurrentCulture, out result) &&
                // Then try in US english
                !double.TryParse(value, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.GetCultureInfo("en-US"), out result) &&
                // Then in neutral language
                !double.TryParse(value, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out result))
            {
                result = defaultValue;
            }
            return result;
        }
        public static string GetPublicIP()
        {
            string url = "http://checkip.dyndns.org";
            System.Net.WebRequest req = System.Net.WebRequest.Create(url);
            System.Net.WebResponse resp = req.GetResponse();
            System.IO.StreamReader sr = new System.IO.StreamReader(resp.GetResponseStream());
            string response = sr.ReadToEnd().Trim();
            string[] a = response.Split(':');
            string a2 = a[1].Substring(1);
            string[] a3 = a2.Split('<');
            string a4 = a3[0];
            return a4;
        }

    }

    public class Inputitem
    {
        /// <summary>
        /// Тип элемента формы
        /// </summary>
        public string InputType { get; set; }
        /// <summary>
        /// Имя элемента формы
        /// </summary>
        public string InputName { get; set; }
        /// <summary>
        /// Значение элемента формы
        /// </summary>
        public string InputValue { get; set; }
        /// <summary>
        /// Форма в которой содержатся элементы
        /// </summary>
        public HtmlFormValue FormAtribute { get; set; }
    }
    public class HtmlFormValue
    {
        public HtmlFormValue()
        {
            ListInputs = new List<Inputitem>();
        }
        /// <summary>
        /// Имя вебформы, может отсутствовать
        /// </summary>
        public string NameForm { get; set; }
        /// <summary>
        /// Oбработчик, к которому обращаются данные формы при их отправке на сервер
        /// </summary>
        public string ActionForm { get; set; }
        /// <summary>
        /// Метод который использует форма для отправки на сервер Post или Get
        /// </summary>
        public string MethodForm { get; set; }
        /// <summary>
        /// Список всех всех input Html элементов в вебформе
        /// </summary>
        public List<Inputitem> ListInputs { get; set; }
        /// <summary>
        /// Список элементов в формате 'Имя=Значение'
        /// </summary>
        /// <returns></returns>
        public List<string> GetInputsNameValue() => ListInputs
        .Select(x => $"{x.InputName}={x.InputValue}")
        .ToList();
    }

}