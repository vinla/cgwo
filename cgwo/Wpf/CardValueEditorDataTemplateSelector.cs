using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Cogs.Common;

namespace cgwo.Wpf
{
    public class CardValueEditorDataTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (container is FrameworkElement element)
            {
                if (item is CardAttributeValue cardAttributeValue)
                {
                    switch (cardAttributeValue.CardAttribute.Type)
                    {
                        case AttributeType.Text:
                            return element.FindResource("TextEditor") as DataTemplate;
                        case AttributeType.Image:
                            return element.FindResource("ImageEditor") as DataTemplate;
                    }
                }
            }

            throw new InvalidOperationException("Unable to resolve data template");
        }
    }
}
