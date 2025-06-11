using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace SAE201.Model
{
    public class MontantToColor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return Brushes.Black;
           
            if (value is string stringValue)
            {
                switch (stringValue.ToLower())
                {
                    case "accepté":
                    case "accepte":
                    case "oui":
                    case "validé":
                    case "valide":
                        return Brushes.Green;
                    case "refusé":
                    case "refuse":
                    case "non":
                    case "rejeté":
                    case "rejete":
                        return Brushes.Red;
                    case "en attente":
                    case "attente":
                    case "pending":
                        return Brushes.Orange;
                    default:
                        return Brushes.Black;
                }
            }

            return Brushes.Black;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}