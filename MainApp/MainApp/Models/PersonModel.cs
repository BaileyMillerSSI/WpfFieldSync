using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace MainApp.Models
{
    public class PersonModel: ViewModelBase
    {
        private int _personId;
        public int PersonId
        {
            get => _personId;
            private set => SetProperty(ref _personId, value);
        }

        private string _firstName;
        public string FirstName
        {
            get => _firstName;
            set => SetProperty(ref _firstName, value);
        }

        private string _lastName;
        public string LastName
        {
            get => _lastName;
            set => SetProperty(ref _lastName, value);
        }

        private int _age;
        public int Age
        {
            get => _age;
            set => SetProperty(ref _age, value);
        }

        public void RemotePropertyChanged(String PropName, object Value)
        {
            PropertyInfo prop = this.GetType().GetProperty(PropName, BindingFlags.Public | BindingFlags.Instance);
            if (null != prop && prop.CanWrite)
            {
                prop.SetValue(this, Convert.ChangeType(Value, prop.PropertyType), null);
            }
        }
        
    }
}
