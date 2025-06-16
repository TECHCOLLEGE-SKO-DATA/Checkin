using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using CheckinLibrary.Settings;


namespace CheckinLibrary.Settings
{
    public partial class SettingsControl
    {
        private readonly string _filePath;

        private string _DefaultSetting;

        public SettingsControl()
        {
            DefaultSettingsxml();

            _filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "checkInSystem");
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
                    <screen>2</screen>
                    <absence>
                        <type Id=""0"" reason=""Syg"" hexColor=""#ffffb900"" />
                        <type Id=""1"" reason=""Skole"" hexColor=""#ffffb900"" />
                        <type Id=""2"" reason=""Ferie"" hexColor=""#ffffb900"" />
                        <type Id=""3"" reason=""SøgeDag"" hexColor=""#ffffb900"" />
                        <type Id=""4"" reason=""VirksomhedSamtale"" hexColor=""#ffffb900"" />
                        <type Id=""5"" reason=""Læge"" hexColor=""#ffffb900"" />
                        <type Id=""6"" reason=""Miscellaneous"" hexColor=""#ffffb900"" />
                    </absence>
                </settings>";
        }
    }
}
