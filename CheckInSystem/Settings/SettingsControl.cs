using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using CheckInSystem.Settings;


namespace CheckInSystem.Settings
{
    public partial class SettingsControl
    {
        private readonly string _filePath;

        private string _DefaultSetting;

        public SettingsControl()
        {
            DefaultSettingsxml();

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
                        <EmployeeOverviewScreenShow name=""Screen to show checkedin"" type=""int"" value=""2""/>
                    </screen>
                </settings>";
        }
    }
}
