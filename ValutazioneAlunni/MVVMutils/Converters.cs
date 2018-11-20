using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace ValutazioneAlunni.MVVMutils
{
  public class FromEditModeToBackgroundColor : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
      if (value is bool)
      {
        if ((bool)value == true)
        {
          // Edit mode
          return new SolidColorBrush(Color.FromRgb(0xFF, 0xFF, 0xE0));
        }
      }

      // Read only mode
      return new SolidColorBrush(Color.FromRgb(0xE0, 0xE0, 0xE0));
    }

    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
      return false;
    }
  }


}
