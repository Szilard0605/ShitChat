using Avalonia.Controls;
using Avalonia.Controls.Templates;
using MvvmHelpers;
using SC_App.ViewModels;
using System;
using System.Runtime.CompilerServices;

namespace SC_App.Helpers
{
    public class ViewLocator : IDataTemplate
    {
        public bool SupportsRecycling => false;

        public Control Build(object data)
        {
            var name = data.GetType().FullName.Replace("ViewModel", "View");
            var type = Type.GetType(name);

            if (type != null)
            {
                return (Control)Activator.CreateInstance(type);
            }
            else
            {
                return new TextBlock { Text = "Not Found: " + name };
            }
        }

        public bool Match(object data)
        {
            return data is ViewModelBase;
        }

    }
}
