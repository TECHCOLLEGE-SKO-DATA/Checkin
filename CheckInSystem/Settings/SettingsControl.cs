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

        public string _DefaultSetting {  get; private set; }

        public SettingsControl()
        {
            DefaultSettingsxml();

            //string projectRoot = Path.GetFullPath("Settings\\Settings.xml");
            //_filePath = projectRoot.Replace("\\bin\\Debug\\net8.0-windows", "");

            //if (!File.Exists(_filePath))
            //    throw new FileNotFoundException("Settings file not found.");

            _filePath = Environment.ExpandEnvironmentVariables(@"%AppData%\checkInSystem");
            if (!Directory.Exists(_filePath))
            {
                Directory.CreateDirectory(_filePath);
            }
            _filePath += @"\Settings.xml";

            if (!File.Exists(_filePath))
            {
                File.WriteAllText(_filePath, _DefaultSetting);
            }
        }

        public void DefaultSettingsxml()
        {
            _DefaultSetting =
                @"<?xml version=""1.0"" encoding=""utf-8""?>
                <settings>
                    <screen name=""Screen Settings"">
                        <EmployeeOverviewScreenShow name=""Show Checked in screen"" type=""int"" value=""2""/>
                    </screen>

                    <TimeArival name=""Arrival time"" type=""Time"" value=""08:15:00""/>

                    <absence name=""Legal Reason for absence"">
                    </absence>

                    <MISC name=""Miscellaneous"">
                        <ShortName name=""ShortName On/Off"" Type=""bool"" value=""true""/>
                    </MISC>
                </settings>";
        }
    }
}
