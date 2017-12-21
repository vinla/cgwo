using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cogs.Common;
using Cogs.Designer;
using Cogs.Mvvm;

namespace cgwo.Configuration
{
    public static class AutoMapperConfiguration
    {
        public static void ConfigureMaps()
        {
            AutoMapper.Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<TextElement, TextElementLayout>()
                    .ForMember(
                        txt => txt.TextColor,
                        opt => opt.ResolveUsing(src => src.TextColor.ToHex()));


                cfg.CreateMap<TextElementLayout, TextElement>()
                    .ForMember(
                        txt => txt.TextColor,
                        opt => opt.ResolveUsing(src => ColorExtensionMethods.FromHex(src.TextColor)));

                cfg.CreateMap<RectangleElement, RectangleElementLayout>()
                    .ForMember(
                        rect => rect.BackgroundColor,
                        opt => opt.ResolveUsing(src => src.BackgroundColor.ToHex()))
                    .ForMember(
                        rect => rect.BorderColor,
                        opt => opt.ResolveUsing(src => src.BorderColor.ToHex()));
                

                cfg.CreateMap<RectangleElementLayout, RectangleElement>()
                    .ForMember(
                        rect => rect.BackgroundColor,
                        opt => opt.ResolveUsing(src => ColorExtensionMethods.FromHex(src.BackgroundColor)))
                    .ForMember(
                        rect => rect.BorderColor,
                        opt => opt.ResolveUsing(src => ColorExtensionMethods.FromHex(src.BorderColor)));

                cfg.CreateMap<EllipseElement, EllipseElementLayout>()
                    .ForMember(
                        ell => ell.BackgroundColor,
                        opt => opt.ResolveUsing(src => src.BackgroundColor.ToHex()))
                    .ForMember(
                        ell => ell.BorderColor,
                        opt => opt.ResolveUsing(src => src.BorderColor.ToHex()));

                cfg.CreateMap<EllipseElementLayout, EllipseElement>()
                    .ForMember(
                        ell => ell.BackgroundColor,
                        opt => opt.ResolveUsing(src => ColorExtensionMethods.FromHex(src.BackgroundColor)))
                    .ForMember(
                        ell => ell.BorderColor,
                        opt => opt.ResolveUsing(src => ColorExtensionMethods.FromHex(src.BorderColor)));
            });
        }
    }
}
