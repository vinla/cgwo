using System;
using Cogs.Common;

namespace Cogs.Core
{
    public class JsonCardAttribute
    {
        public Guid CardTypeId { get; set; }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public AttributeType Type { get; set; }
    }
}
