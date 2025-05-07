using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CheckinLib.Settings
{
    public partial class SettingsControl
    {
        public int GetEmployeeOverViewSettings()
        {
            XDocument xmlDoc = XDocument.Load(_filePath);
            XElement? setting = xmlDoc.Descendants("screen").FirstOrDefault();

            if (setting != null && int.TryParse(setting.Value, out int result))
            {
                return result;
            }

            throw new Exception("Invalid or missing setting value.");
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
