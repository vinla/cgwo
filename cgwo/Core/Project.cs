using System.Collections.Generic;
using System.Linq;


namespace cgwo.Core
{
	public class Project : Mvvm.ViewModel
	{
		private readonly List<CardType> _cardTypes;
		public Project()
		{
			_cardTypes = new List<CardType>();
		}
		public string Name { get; set; }

		public List<CardType> CardTypes => _cardTypes;		
	}
}
