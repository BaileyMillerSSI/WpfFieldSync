using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MainApp.Models
{
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected bool SetProperty<T>(ref T field, T newValue, [CallerMemberName]string propertyName = null)
        {
            if (!EqualityComparer<T>.Default.Equals(field, newValue))
            {
                field = newValue;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
                EmitChange(propertyName, newValue);
                return true;
            }
            return false;
        }


        protected void EmitChange(String PropName, object Value)
        {
            if (MainWindow.BroadcastServer != null)
            {
                MainWindow.BroadcastServer.BroadcastChange(PropName, Value);
            }
        }
    }
}
