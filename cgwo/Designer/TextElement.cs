﻿using System.Windows.Media;
using GorgleDevs.Wpf.Mvvm;

namespace Cogs.Designer
{
    public class TextElement : CardElement
    {
        public float TextSize
        {
            get { return GetValue<float>(nameof(TextSize)); }
            set { SetValue(nameof(TextSize), value); }
        }

        public Color TextColor
        {
            get { return GetValue<Color>(nameof(TextColor)); }
            set { SetValue(nameof(TextColor), value); }
        }

        [CalculateFrom(nameof(TextColor))]
        public Brush TextBrush => new SolidColorBrush(TextColor);

        public string Text
        {
            get
            {
                return GetValue<string>(nameof(Text));
            }
            set
            {
                SetValue(nameof(Text), value);
            }
        }
    }
}
