using System.Linq;
using AutoMapper;

namespace Cogs.Data.LiteDb
{
	public class AutoMapperLiteDbProfile : Profile
	{
		public AutoMapperLiteDbProfile()
		{

			CreateMap<Json.CardLayout, Common.CardLayout>()
				.ForMember
				(
					dest => dest.Elements,
					opt => opt.ResolveUsing(src => src.Elements.Select(el => ElementLayoutConverter.FromJson(el)))
				);

			CreateMap<Common.CardLayout, Json.CardLayout>()
				.ForMember(dest => dest.Id, opt => opt.Ignore())
				.ForMember(
				dest => dest.Elements,
				opt => opt.ResolveUsing(src => src.Elements.Select(el => new Json.ElementLayout
				{
					ElementType = el.GetType().Name,
					JsonData = Newtonsoft.Json.JsonConvert.SerializeObject(el)
				})));

			CreateMap<Json.CardAttribute, Common.CardAttribute>()
				.ForMember
				(
					dest => dest.Type,
					opt => opt.ResolveUsing(src => (Common.AttributeType)(int)src.Type)
				);
		}
	}
}
