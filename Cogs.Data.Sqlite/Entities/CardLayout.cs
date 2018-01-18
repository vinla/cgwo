using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cogs.Data.Sqlite.Entities
{
    public class CardLayout : BaseEntity
    {
        public Guid CardTypeId { get; set; }
        public string BackgroundColor { get; set; }
        public byte[] BackgroundImage { get; set; }

        [ForeignKey(nameof(CardTypeId))]
        public virtual CardType CardType { get; set; }
    }
}
