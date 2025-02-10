using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CheckInSystem.Settings
{
    public partial class SettingsControl
    {
        public int GetEmployeeOverViewSettings()
        {
            XDocument xmlDoc = XDocument.Load(_filePath);
            XElement? setting = xmlDoc.Descendants("EmployeeOverviewScreenShow").FirstOrDefault();

            if (int.TryParse(setting!.Attribute("value")?.Value, out int result))
            {
                return result;
            }

            throw new Exception("Invalid or missing setting value.");
        }
    }
}
