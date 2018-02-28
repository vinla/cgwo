using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Cogs.Common;
using Cogs.Designer;

namespace cgwo.Wpf
{
    public class CardValueEditorDataTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (container is FrameworkElement element)
            {
                if (item is NamedValueViewModel valueViewModel)
                {
					return element.FindResource(valueViewModel.Editor) as DataTemplate;					
                }
            }

            throw new InvalidOperationException("Unable to resolve data template");
        }
    }
}
