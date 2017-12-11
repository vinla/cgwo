using System.Collections.Generic;
using System.Linq;


namespace cgwo.Core
{
	public class Project : Mvvm.ViewModel
	{
		public Project()
		{
			
		}
		public string Name { get; set; }

		public IEnumerable<CardType> CardTypes
		{
			get;
			set;
		}
		
	}
}
