﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cogs.Common;

namespace Cogs.Designer
{
    public static class LayoutConverter
    {
        public static IEnumerable<CardElement> ToDesignerElements(IEnumerable<ElementLayout> layoutElements)
        {
            var results = new List<CardElement>();
            foreach (var element in layoutElements)
            {
                if (element is TextElementLayout text)
                    results.Add(AutoMapper.Mapper.Map<TextElement>(text));
                else if (element is RectangleElementLayout rect)
                    results.Add(AutoMapper.Mapper.Map<RectangleElement>(rect));
                else if (element is EllipseElementLayout ellipse)
                    results.Add(AutoMapper.Mapper.Map<EllipseElement>(ellipse));
                else if (element is ImageElementLayout image)
                    results.Add(AutoMapper.Mapper.Map<ImageElement>(image));
            }

            return results;
        }

        public static IEnumerable<ElementLayout> FromDesignerElements(IEnumerable<CardElement> designerElements)
        {
            var results = new List<ElementLayout>();

            foreach (var element in designerElements)
            {
                if (element is TextElement text)
                    results.Add(AutoMapper.Mapper.Map<TextElementLayout>(text));
                else if (element is RectangleElement rect)
                    results.Add(AutoMapper.Mapper.Map<RectangleElementLayout>(rect));
                else if (element is EllipseElement ellipse)
                    results.Add(AutoMapper.Mapper.Map<EllipseElementLayout>(ellipse));
                else if (element is ImageElement image)
                    results.Add(AutoMapper.Mapper.Map<ImageElementLayout>(image));
            }

            return results;
        }
    }
}
