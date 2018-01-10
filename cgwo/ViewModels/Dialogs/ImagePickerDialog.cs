using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Cogs.Common;
using GorgleDevs.Wpf.Mvvm;

namespace cgwo.ViewModels.Dialogs
{
    public class ImagePickerDialog : DialogViewModel
    {
        private readonly IImageStore _imageStore;
        private readonly IDialogService _dialogService;
        private readonly ObservableCollection<ImageData> _images;

        public ImagePickerDialog(IDialogService dialogService, IImageStore imageStore)
        {
            _dialogService = dialogService ?? throw new ArgumentNullException(nameof(dialogService));
            //_imageStore = imageStore ?? throw new ArgumentNullException(nameof(imageStore));
            _images = new ObservableCollection<ImageData>();
        }

        public ImageData SelectedImage { get; set; }

        public ICommand AddImage => new DelegateCommand(() =>
        {
            var (result, path) = _dialogService.ChooseFile(String.Empty, "Image files|*.png;*.jpg;*.jpeg;*.bmp;*.gif");
            if(result == DialogResult.Accept)
            {
                var imageData = new ImageData(Guid.NewGuid(), System.IO.File.ReadAllBytes(path));
                //_imageStore.SaveImageData(imageData.Reference, imageData.RawBytes);
                _images.Add(imageData);
            }
        });

        public ICommand SelectCommand => new DelegateCommand(() =>
        {
            if(SelectedImage != null)
                SetResult(DialogResult.Accept);
        });

        public ICommand CancelCommand => new DelegateCommand(() =>
        {
            SelectedImage = null;
            SetResult(DialogResult.Reject);
        });

        public ObservableCollection<ImageData> Images => _images;        
    }

    public class ImageData
    {
        private readonly Guid _reference;
        private readonly IImageStore _imageStore;
        private byte[] _data;
        private ImageSource _imageSource;        

        public ImageData(Guid reference, byte[] data)
        {
            _reference = reference;
            _data = data;
        }

        public Guid Reference => _reference;
        public ImageSource Source
        {
            get
            {
                if(_imageSource == null)
                {
                    _imageSource = (ImageSource)new ImageSourceConverter().ConvertFrom(_data);                    
                }

                return _imageSource;
            }
        }

        public byte[] RawBytes => _data;        
    }
}
