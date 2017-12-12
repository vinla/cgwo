using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cgwo.Core
{
	public abstract class CoreObject 
	{
		private Guid _id;

		protected CoreObject()
		{
			_id = Guid.NewGuid();
		}

		public Guid Id => _id;		
	}
}
