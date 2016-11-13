using System.ComponentModel;

namespace RWP.App.Infrastructure.Mvvm
{
	public abstract class NotifierBase : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged = delegate { };

		protected virtual void OnPropertyChanged(string propertyName)
		{
			var args = new PropertyChangedEventArgs(propertyName);
			PropertyChanged(this, args);
		}
	}
}