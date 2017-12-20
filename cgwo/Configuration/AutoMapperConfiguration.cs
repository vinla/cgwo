using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cogs.Common;
using Cogs.Designer;

namespace cgwo.Configuration
{
    public static class AutoMapperConfiguration
    {
        public static void ConfigureMaps()
        {
            AutoMapper.Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<TextElement, TextElementLayout>();
                cfg.CreateMap<TextElementLayout, TextElement>();
                cfg.CreateMap<RectangleElement, RectangleElementLayout>();
                cfg.CreateMap<RectangleElementLayout, RectangleElement>();
                cfg.CreateMap<EllipseElement, EllipseElementLayout>();
                cfg.CreateMap<EllipseElementLayout, EllipseElement>();
            });
        }
    }
}
