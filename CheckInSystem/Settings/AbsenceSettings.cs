using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CheckInSystem.Models.Absence;
using System.Xml.Linq;

namespace CheckInSystem.Settings
{
    public partial class SettingsControl
    {
        /*
        public List<string> LoadAbsenceReasons(string filePath)
        {
            AbsenceReasons = new List<string>();

            XDocument doc = XDocument.Load(filePath);
            var absenceTypes = doc.Descendants("absence").Elements("type");

            foreach (var type in absenceTypes)
            {
                AbsenceReasons.Add(type.Value);
            }
        }
        */
    }
}
