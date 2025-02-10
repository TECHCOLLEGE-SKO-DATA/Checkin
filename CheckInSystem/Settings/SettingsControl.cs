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
    public partial class SettingsControl
    {
        private readonly string _filePath;

        public SettingsControl()
        {
            string projectRoot = Path.Combine(Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.Parent.Parent.FullName);

            _filePath = Path.Combine(projectRoot, "CheckInSystem", "Settings", "Settings.xml");

            if (!File.Exists(_filePath))
                throw new FileNotFoundException("Settings file not found.");
        }
    }
}
