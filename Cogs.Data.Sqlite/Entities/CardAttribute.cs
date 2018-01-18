using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cogs.Common;

namespace Cogs.Data.Sqlite.Entities
{
    public class CardAttribute : BaseEntity
    {
        public Guid CardTypeId { get; set; }
        public String Name { get; set; }
        public AttributeType Type { get; set; }

        [ForeignKey(nameof(CardTypeId))]
        public CardType CardType { get; set; }
    }
}
