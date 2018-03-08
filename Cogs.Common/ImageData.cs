using System.ComponentModel;

namespace Cogs.Common
{
	public class ImageData : INotifyPropertyChanged
	{
		private readonly string _key;
		private byte[] _data;
		private bool _hasChanged;

		public event PropertyChangedEventHandler PropertyChanged;

		public ImageData(string key)
		{
			_key = key;
		}

		public string Key => _key;

		public void LoadFromSource(byte[] sourceData)
		{
			_data = sourceData;
			_hasChanged = false;
		}

		public byte[] Data
		{
			get => _data;
			set
			{
				_data = value;
				_hasChanged = true;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Data)));
			}
		}

		public bool HasChanged => _hasChanged;
	}
}
