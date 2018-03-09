using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cogs.Json
{
	public class CardValue
	{
		public Guid CardAttributeId { get; set; }
		public string Value { get; set; }
		public AttributeType AttributeType { get; set; }
	}
}
