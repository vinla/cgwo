using System.Linq;
using AutoMapper;

namespace Cogs.Data.LiteDb
{
	public class AutoMapperLiteDbProfile : Profile
	{
		public AutoMapperLiteDbProfile()
		{
			CreateMap<Json.CardLayout, Common.CardLayout>()
				.ForMember(dest => dest.BackgroundImage, opt => opt.Ignore())
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
				.ForMember(dest => dest.Type, opt => opt.ResolveUsing(src => (Common.AttributeType)(int)src.Type));

			CreateMap<Common.Card, Json.Card>()
				.ForMember(dest => dest.CardTypeId, opt => opt.MapFrom(src => src.CardType.Id))
				.ForMember(dest => dest.Values, opt => opt.ResolveUsing(src => src.AttributeValues.Select(Mapper.Map<Json.CardValue>).ToArray()));

			CreateMap<Common.CardAttributeValue, Json.CardValue>()
				.ForMember(dest => dest.AttributeType, opt => opt.ResolveUsing(src => (Json.AttributeType)(int)src.CardAttribute.Type))
				.ForMember(dest => dest.CardAttributeId, opt => opt.MapFrom(src => src.CardAttribute.Id));			
		}
	}
}
