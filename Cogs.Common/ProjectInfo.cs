﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cogs.Common
{
    public class ProjectInfo
    {
		public ProjectInfo()
		{
			Id = Guid.NewGuid();
		}

		public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
