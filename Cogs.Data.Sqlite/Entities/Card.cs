using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cogs.Data.Sqlite.Entities
{
    public class Card : BaseEntity
    { 
        public Guid CardTypeId { get; set; }
        public string Name { get; set; }
        public byte[] Image { get; set; }

        [ForeignKey(nameof(CardTypeId))]
        public virtual CardType CardType { get; set; }
    }
}
