using CheckinLibrary.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CheckinLibrary.Settings
{
    public partial class SettingsControl
    {
        public List<AbsenceReason> GetAbsenceReasons()
        {
            XDocument xmlDoc = XDocument.Load(_filePath);

            AbsenceReason.Reasons.Clear();

            return AbsenceReason.Reasons = xmlDoc.Descendants("absence")
                  .Elements("type")
                  .Select(x => new AbsenceReason(
                      int.TryParse(x.Attribute("Id")?.Value, out int id) ? id : 0,
                      x.Attribute("reason")?.Value ?? "",
                      ColorTranslator.FromHtml(x.Attribute("hexColor")?.Value ?? "#000000")))
                .  ToList();
        }

        public void SetAbsenceReasons(List<AbsenceReason> reasons)
        {
            XDocument xmlDoc = XDocument.Load(_filePath);
            XElement? absenceElement = xmlDoc.Descendants("absence").FirstOrDefault();

            if (absenceElement == null)
                throw new Exception("Missing <absence> section in settings file.");

            // Clear existing <type> elements
            absenceElement.RemoveNodes();

            AbsenceReason.Reasons.Clear();

            AbsenceReason.Reasons = reasons;

            // Add updated list
            foreach (var reason in reasons)
            {
                absenceElement.Add(new XElement("type",
                    new XAttribute("Id", reason.Id),
                    new XAttribute("reason", reason.Reason),
                    new XAttribute("hexColor", $"#{reason.HexColor.Name}")));
            }

            xmlDoc.Save(_filePath);
        }

    }
}
