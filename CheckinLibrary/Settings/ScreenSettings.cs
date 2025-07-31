using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CheckinLibrary.Settings
{
    public partial class SettingsControl
    {
        public bool GetDarkMode()
        {
            XDocument xmlDoc = XDocument.Load(_filePath);
            XElement? setting = xmlDoc.Descendants("DarkMode").FirstOrDefault();

            if(setting != null && bool.TryParse(setting.Value, out bool result))
            {
                return result;
            }

            throw new Exception("Invalid or missing setting value darkmode.");
        }

        public void SetDarkMode(bool value)
        {
            XDocument xmlDoc = XDocument.Load(_filePath);
            XElement? setting = xmlDoc.Descendants("DarkMode").FirstOrDefault();

            try { setting.Value = value.ToString(); }
            catch 
            {
                throw new Exception("Invalid or missing setting value for DarkMode.");
            }
            xmlDoc.Save(_filePath);
        }

        public int GetEmployeeOverViewSettings()
        {
            XDocument xmlDoc = XDocument.Load(_filePath);
            XElement? setting = xmlDoc.Descendants("screen").FirstOrDefault();

            if (setting != null && int.TryParse(setting.Value, out int result))
            {
                return result;
            }

            throw new Exception("Invalid or missing setting value employeeOverview.");
        }

        public void SetEmployeeOverViewSettings(int value) 
        {
            XDocument xmlDoc = XDocument.Load(_filePath);
            XElement? setting = xmlDoc.Descendants("screen").FirstOrDefault();

            try { setting.Value=value.ToString();}
            catch
            {
                throw new Exception("Invalid or missing setting value.");
            }
            xmlDoc.Save(_filePath);
        }
    }
}
