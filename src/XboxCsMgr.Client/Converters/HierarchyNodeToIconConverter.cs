using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using XboxCsMgr.Client.ViewModels.Controls;

namespace XboxCsMgr.Client.Converters
{
    [ValueConversion(typeof(String), typeof(TemplateKey))]
    public class HierarchyNodeToIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((value is SavedAtomsViewModel))
            {
                return Application.Current.FindResource("HierarchySaveFile");
            }
            return Application.Current.FindResource("HierarchyClosedFolder");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
