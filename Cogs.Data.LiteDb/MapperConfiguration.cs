using System.Linq;
using AutoMapper;

namespace Cogs.Data.LiteDb
{
	public class AutoMapperLiteDbProfile : Profile
	{
		public AutoMapperLiteDbProfile()
		{
			CreateMap<Json.CardLayout, Common.CardLayout>()
				.ForMember(
					dest => dest.BackgroundImage,
					opt => opt.Ignore()
				)
				.ForMember(
					dest => dest.Elements,
					opt => opt.ResolveUsing(src => src.Elements.Select(el => ElementLayoutConverter.FromJson(el)))
				);

			CreateMap<Common.CardLayout, Json.CardLayout>()
				.ForMember(dest => dest.Id, opt => opt.Ignore())				
				.ForMember(
					dest => dest.Elements,
					opt => opt.ResolveUsing(src => src.Elements.Select(el => ElementLayoutConverter.ToJson(el)))
				);

			CreateMap<Json.CardAttribute, Common.CardAttribute>()
				.ForMember
				(
					dest => dest.Type,
					opt => opt.ResolveUsing(src => (Common.AttributeType)(int)src.Type)
				);
		}
	}
}
