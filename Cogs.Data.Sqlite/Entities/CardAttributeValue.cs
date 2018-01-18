using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cogs.Data.Sqlite.Entities
{
    public class CardAttributeValue : BaseEntity
    {
        public Guid CardId { get; set; }
        public Guid CardAttributeId { get; set; }
        public string Value { get; set; }

        [ForeignKey(nameof(CardId))]
        public virtual Card Card { get; set; }

        [ForeignKey(nameof(CardAttributeId))]
        public virtual CardAttribute Attribute { get; set; }
    }
}
