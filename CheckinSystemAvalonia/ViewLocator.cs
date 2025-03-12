using System;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using CheckinSystemAvalonia.ViewModels;

namespace CheckinSystemAvalonia
{
    public class ViewLocator : IDataTemplate
    {
        public Control Build(object data)
        {
            if (data == null) return new TextBlock { Text = "No ViewModel provided" };

            var viewModelName = data.GetType().FullName!;

            var name = viewModelName.Replace("ViewModels", "Views.UserControls");
            name = name.Replace("ViewModel", "View");

            var type = Type.GetType(name);

            if (type != null)
            {
                try
                {
                    var view = (Control)Activator.CreateInstance(type)!;
                    return view;
                }
                catch (Exception ex)
                {
                    return new TextBlock { Text = "Error creating view: " + ex.Message };
                }
            }

            return new TextBlock { Text = "View Not Found: " + name };
        }

        public bool Match(object data) => data is ViewModelBase;
    }
}
