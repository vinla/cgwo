using System;

namespace Cogs.Core
{
    public class JsonCard
    {
        public Guid Id { get; set; }
        public Guid CardTypeId { get; set; }
        public string Name { get; set; }

        public string ImageData { get; set; }
    }
}
