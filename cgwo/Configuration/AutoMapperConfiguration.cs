using AutoMapper;
using Cogs.Common;
using Cogs.Designer;
using GorgleDevs.Wpf;

namespace cgwo.Configuration
{
	public class AutoMapperClientProfile : Profile
	{
		public AutoMapperClientProfile()
		{
			CreateMap<TextElement, TextElementLayout>()
				.ForMember(
					txt => txt.TextColor,
					opt => opt.ResolveUsing(src => src.TextColor.ToHex()));

			CreateMap<TextElementLayout, TextElement>()
				.ForMember(
					txt => txt.TextColor,
					opt => opt.ResolveUsing(src => ColorExtensionMethods.FromHex(src.TextColor)));

			CreateMap<RectangleElement, RectangleElementLayout>()
				.ForMember(
					rect => rect.BackgroundColor,
					opt => opt.ResolveUsing(src => src.BackgroundColor.ToHex()))
				.ForMember(
					rect => rect.BorderColor,
					opt => opt.ResolveUsing(src => src.BorderColor.ToHex()));

			CreateMap<RectangleElementLayout, RectangleElement>()
				.ForMember(
					rect => rect.BackgroundColor,
					opt => opt.ResolveUsing(src => ColorExtensionMethods.FromHex(src.BackgroundColor)))
				.ForMember(
					rect => rect.BorderColor,
					opt => opt.ResolveUsing(src => ColorExtensionMethods.FromHex(src.BorderColor)));

			CreateMap<EllipseElement, EllipseElementLayout>()
				.ForMember(
					ell => ell.BackgroundColor,
					opt => opt.ResolveUsing(src => src.BackgroundColor.ToHex()))
				.ForMember(
					ell => ell.BorderColor,
					opt => opt.ResolveUsing(src => src.BorderColor.ToHex()));

			CreateMap<EllipseElementLayout, EllipseElement>()
				.ForMember(
					ell => ell.BackgroundColor,
					opt => opt.ResolveUsing(src => ColorExtensionMethods.FromHex(src.BackgroundColor)))
				.ForMember(
					ell => ell.BorderColor,
					opt => opt.ResolveUsing(src => ColorExtensionMethods.FromHex(src.BorderColor)));

			CreateMap<ImageElement, ImageElementLayout>();
			CreateMap<ImageElementLayout, ImageElement>();
		}
	}
}
