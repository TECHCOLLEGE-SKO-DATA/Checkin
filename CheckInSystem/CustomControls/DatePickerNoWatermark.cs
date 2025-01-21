using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace CheckInSystem.CustomControls;

public class DatePickerNoWatermark : DatePicker
{
    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        DatePickerTextBox box = base.GetTemplateChild("PART_TextBox") as DatePickerTextBox;
        box.ApplyTemplate();

        ContentControl watermark = box.Template.FindName("PART_Watermark", box) as ContentControl;
        watermark.Content = "";
    }
}