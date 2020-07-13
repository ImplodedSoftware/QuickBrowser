using System;
using System.Globalization;
using System.Text;
using System.Windows.Data;
using NeonScripting;
using NeonScripting.Models;

namespace QuickBrowse
{
    public class TitleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return string.Empty;
            }
            var track = (NeonScriptTrack) value;
            var sb = new StringBuilder();
            if (string.IsNullOrEmpty(track.Title))
            {
                sb.Append("Untitled");
            }
            else
            {
                sb.Append(track.Title);
            }
            if (!string.IsNullOrEmpty(track.Subtitle))
            {
                sb.AppendFormat("[{0}]", track.Subtitle);
            }
            if (!string.IsNullOrEmpty(track.Remix))
            {
                sb.AppendFormat("({0})", track.Remix);
            }
            sb.Append(" by ");
            if (string.IsNullOrEmpty(track.Artist))
            {
                sb.Append("Unknown artist");
            }
            else
            {
                sb.Append(track.Artist);
            }
            sb.AppendFormat(" ({0})", QuickBrowseHelpers.FormatTime(track.Songlength));
            return sb.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
