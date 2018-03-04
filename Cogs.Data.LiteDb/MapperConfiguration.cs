using System.Linq;
using AutoMapper;

namespace Cogs.Data.LiteDb
{
	public static class MapperConfiguration
	{
		public static void Configure()
		{
			Mapper.Initialize(cfg => 
			{
				cfg.CreateMap<Json.CardLayout, Common.CardLayout>()
					.ForMember
					(
						dest => dest.Elements,
						opt => opt.ResolveUsing(src => src.Elements.Select(el => ElementLayoutConverter.FromJson(el)))
					);

				cfg.CreateMap<Json.CardAttribute, Common.CardAttribute>()
					.ForMember
					(
						dest => dest.Type,
						opt => opt.ResolveUsing(src => (Common.AttributeType)(int)src.Type)
					);
			});			
		}
	}
}
