using System;
using System.Collections.Generic;
using System.Linq;

namespace Cogs.Designer
{
	public static class CardElementClipboard
	{
		private static List<JsonObject> _objects;
		public static void SetElements(IEnumerable<CardElement> elements)
		{
			_objects = elements.Select(el => JsonObject.Serialize(el)).ToList();
		}

		public static IEnumerable<CardElement> GetElements()
		{
			return _objects.Select(obj => obj.Deserialize()).Cast<CardElement>();
		}
	}

	public class JsonObject
	{
		private readonly Type _type;
		private readonly string _json;

		private JsonObject(Type type, string json)
		{
			_type = type;
			_json = json;
		}

		public static JsonObject Serialize(object obj)
		{
			var json = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
			return new JsonObject(obj.GetType(), json);
		}

		public object Deserialize()
		{
			return Newtonsoft.Json.JsonConvert.DeserializeObject(_json, _type);
		}
	}
}
