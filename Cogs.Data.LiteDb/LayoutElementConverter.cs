﻿using System;
using System.Reflection;
using Cogs.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Cogs.Data.LiteDb
{
	public static class ElementLayoutConverter
	{
		public static ElementLayout FromJson(Json.ElementLayout json)
		{
			if (json.ElementType == typeof(TextElementLayout).Name)
				return Newtonsoft.Json.JsonConvert.DeserializeObject<TextElementLayout>(json.JsonData);
			else if (json.ElementType == typeof(RectangleElementLayout).Name)
				return Newtonsoft.Json.JsonConvert.DeserializeObject<RectangleElementLayout>(json.JsonData);
			else if (json.ElementType == typeof(EllipseElementLayout).Name)
				return Newtonsoft.Json.JsonConvert.DeserializeObject<EllipseElementLayout>(json.JsonData);
			else if (json.ElementType == typeof(ImageElementLayout).Name)
				return Newtonsoft.Json.JsonConvert.DeserializeObject<ImageElementLayout>(json.JsonData);

			throw new InvalidOperationException("");
		}

		public static Json.ElementLayout ToJson(ElementLayout layout)
		{
			var settings = new JsonSerializerSettings
			{
				ContractResolver = new ElementLayoutContractResolver()
			};

			return new Json.ElementLayout
			{
				ElementType = layout.GetType().Name,
				JsonData = JsonConvert.SerializeObject(layout, settings)
			};
		}
	}

	public class ElementLayoutContractResolver : DefaultContractResolver
	{
		protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
		{
			JsonProperty property = base.CreateProperty(member, memberSerialization);
			if (property.PropertyName == nameof(ImageElementLayout.ImageData))
			{
				property.Ignored = true;
			}
			return property;
		}
	}
}