
using System.ComponentModel;
using System.Runtime.Serialization;

namespace CannaBe
{

    [DataContract]
    public class ViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void NotifyPropertyChanged(params string[] changedProperties)
        {
            if (changedProperties != null)
            {
                foreach (string property in changedProperties)
                {
                    OnPropertyChanged(property);
                }
            }
        }

        protected virtual bool SetValue<T>(ref T target, T value, params string[] changedProperties)
        {
            if (Equals(target, value))
            {
                return false;
            }

            target = value;

            foreach (string property in changedProperties)
            {
                OnPropertyChanged(property);
            }

            return true;
        }
    }
}