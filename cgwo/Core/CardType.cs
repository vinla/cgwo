using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cgwo.Core
{
	public class CardType : Mvvm.ViewModel
	{
		public string Name
		{
			get { return GetValue<string>(nameof(Name)); }
			set { SetValue(nameof(Name), value); }
		}
	}
}
