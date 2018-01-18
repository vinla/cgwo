using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cogs.Data.Sqlite.Entities
{
    public static class EntityMapper
    {
        public static void Initialise()
        {
            AutoMapper.Mapper.Initialize(cfg =>
            {
                
            });
        }
    }    
}
