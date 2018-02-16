using System.Collections.Generic;
using System.Linq;

namespace Cogs.Designer
{
	public static class CardElementClipboard
	{
		private static readonly List<CardElement> _elements = new List<CardElement>();

		public static void SetElements(IEnumerable<CardElement> elements)
		{
			_elements.Clear();
			_elements.AddRange(elements.Select(el => el.Clone(false)).Cast<CardElement>());
		}

		public static IEnumerable<CardElement> GetElements()
		{
			return _elements.Select(el => el.Clone(false)).Cast<CardElement>();
		}
	}
}
