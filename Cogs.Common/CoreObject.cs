using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cogs.Common
{
    public abstract class CoreObject
    {
        public Guid Id { get; set; }

        public CoreObject()
        {
            Id = Guid.NewGuid();
        }
    }
}
