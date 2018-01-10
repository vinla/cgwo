using System;

namespace Cogs.Common
{
    public class ImageData
    {
        private readonly Guid _reference;
        private readonly IImageStore _imageStore;
        private byte[] _data;

        public ImageData(Guid reference, byte[] data)
        {
            _reference = reference;
            _data = data;
        }

        public ImageData(Guid reference, IImageStore imageStore)
        {
            _reference = reference;
            _imageStore = imageStore;
        }

        public Guid Reference => _reference;
        public byte[] RawBytes => _data ?? (_data = _imageStore.GetImageData(_reference));
    }
}
