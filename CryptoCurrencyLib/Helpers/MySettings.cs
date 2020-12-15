using System.Web;
using System.Configuration;
using System.IO;

namespace CryptoCurrency.Helpers
{
    public class MySettings
    {
        Logger _logger;
        public MySettings()
        {
            _logger = new Logger();
        }
        public string ReadSetting(string key)
        {
            try
            {
                var appSettings = ConfigurationManager.AppSettings;
                string result = appSettings[key] ?? "";
                return result;
            }
            catch (ConfigurationErrorsException ex)
            {
                _logger.LogFile(string.Format("Error: Something wrong with ReadSetting. Exception: {0}", ex.Message));
                throw;
            }
        }
        public void WriteSetting(string key, string value)
        {
            try
            {
                var appSettings = ConfigurationManager.AppSettings;
                string result = appSettings[key] ?? "";
                if (!string.IsNullOrEmpty(result)) appSettings[key] = value;
                else appSettings.Add(key, value);
            }
            catch (ConfigurationErrorsException ex)
            {
                _logger.LogFile(string.Format("Error: Something wrong with WriteSetting. Exception: {0}", ex.Message));
                throw;
            }
        }

        public void UpdateCookies(string cookies)
        {
            string strExeFilePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
            string privateSettingsPath = System.IO.Path.GetDirectoryName(strExeFilePath);
            var info = new DirectoryInfo(privateSettingsPath);
            privateSettingsPath = info.Parent.FullName + "/" + "AppSettingsSecrets.config";
            UpdateConfigFile(privateSettingsPath, "allCookies", cookies);

        }
        public void UpdateConfigFile(string appConfigPath, string key, string newValue)
        {
            var appConfigContent = File.ReadAllText(appConfigPath);
            var searchedString = $"<add key=\"{key}\" value=\"";
            var index = appConfigContent.IndexOf(searchedString) + searchedString.Length;
            var currentValue = appConfigContent.Substring(index, appConfigContent.IndexOf("\"", index) - index);
            var newContent = appConfigContent.Replace($"{searchedString}{currentValue}\"", $"{searchedString}{newValue}\"");
            File.WriteAllText(appConfigPath, newContent);
        }
        public bool isPriceUpdated()
        {
            bool exelDownloaded = false;
            var sEx = ReadSetting("excelDownload");
            if (!string.IsNullOrEmpty(sEx)) exelDownloaded = bool.Parse(sEx);
            return exelDownloaded;
        }
    }
}