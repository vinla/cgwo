using System;
using System.Collections.Generic;

namespace Cogs.Common
{
    public interface IImageStore
    {
        IEnumerable<Guid> GetImageReferences();
        byte[] GetImageData(Guid imageReference);
        void SaveImageData(Guid imageReference, byte[] imageData);
    }
}
