using MagicTheGatheringArenaDeckMaster.ViewModels;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace MagicTheGatheringArenaDeckMaster.Converters
{
    internal class CardImageSourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not UniqueArtTypeViewModel uavm) return value;

            if (!string.IsNullOrWhiteSpace(uavm.ImagePathPng)) return uavm.ImagePathPng;
            if (!string.IsNullOrWhiteSpace(uavm.ImagePathSmall)) return uavm.ImagePathSmall;
            if (!string.IsNullOrWhiteSpace(uavm.ImagePathNormal)) return uavm.ImagePathNormal;
            if (!string.IsNullOrWhiteSpace(uavm.ImagePathLarge)) return uavm.ImagePathLarge;

            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return string.Empty;
        }
    }
}
