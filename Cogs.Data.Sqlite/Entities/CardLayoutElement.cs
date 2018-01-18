using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cogs.Data.Sqlite.Entities
{
    public class CardLayoutElement
    {
        public Guid CardLayoutId { get; set; }
        public string ElementType { get; set; }
        public string JsonData { get; set; }

        [ForeignKey(nameof(CardLayoutId))]
        public virtual CardLayout CardLayout { get; set; }
    }
}
