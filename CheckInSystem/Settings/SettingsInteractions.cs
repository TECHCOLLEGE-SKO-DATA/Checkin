using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using CheckInSystem.Settings;

/*Settting's that are needed or could be good
 * Time arival("when they have to be checkedin")
 * legal absence reason's
 * color for legal absence ("not important in anyway")
 * shorten middle name Y/N
 * screen to show EmployeeOverView on  
*/
namespace CheckInSystem.Settings
{
    public class SettingsInteractions
    {
        private readonly string _filePath;

        public SettingsInteractions()
        {
            string projectRoot = Path.Combine(Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.Parent.Parent.FullName);

            _filePath = Path.Combine(projectRoot, "CheckInSystem", "Settings", "Settings.xml");

            if (!File.Exists(_filePath))
                throw new FileNotFoundException("Settings file not found.");
        }

        public int GetEmployeeOverViewSettings()
        {
            XDocument xmlDoc = XDocument.Load(_filePath);
            XElement? setting = xmlDoc.Descendants("EmployeeOverviewScreenShow").FirstOrDefault();

            if (setting != null && setting.Attribute("value") != null)
            {
                if (int.TryParse(setting.Attribute("value")?.Value, out int result))
                {
                    return result;
                }
            }

            throw new Exception("Invalid or missing setting value.");
        }


    }
}
