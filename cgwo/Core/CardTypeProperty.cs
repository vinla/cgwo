using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cgwo.Core
{
	public class CardTypeProperty : Mvvm.ViewModel
	{
		public string Name
		{
			get { return GetValue<string>(nameof(Name)); }
			set { SetValue(nameof(Name), value); }			
		}

		public string Type
		{
			get { return GetValue<string>(nameof(Type)); }
			set { SetValue(nameof(Type), value); }
		}
	}
}
