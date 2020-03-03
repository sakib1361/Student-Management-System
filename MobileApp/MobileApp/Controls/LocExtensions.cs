using Mobile.Core.Models.Core;
using MobileApp.Helpers;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileApp.Controls
{
    public class IconFont : IMarkupExtension
    {
        public IconType IconType { get; set; }
        public double FontSize { get; set; } = 24;
        public Color Color { get; set; } = Color.White;

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            return GetSource(IconType, Color, FontSize);
        }

        internal static FontImageSource GetSource(IconType iconType, Color color, double size = 24)
        {
            return new FontImageSource()
            {
                FontFamily = IconHelper.FontResource,
                Glyph = IconHelper.KeyValues[iconType],
                Size = size,
                Color = color
            };
        }
    }
}
