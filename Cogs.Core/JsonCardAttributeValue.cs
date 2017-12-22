using System;

namespace Cogs.Core
{
    public class JsonCardAttributeValue
    {
        public Guid CardId { get; set; }
        public Guid CardAttributeId { get; set; }
        public string Value { get; set; }
    }
}
