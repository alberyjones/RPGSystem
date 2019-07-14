using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace RPGSystem
{
    public class IdentifiableItem : NotifyPropertyChangedBase
    {
        private string identifier;
        [XmlAttribute]
        public string Identifier
        {
            get => identifier;
            set => SetField(ref identifier, value);
        }

        private string explicitDisplayName;
        [XmlAttribute]
        public string DisplayName
        {
            get { return String.IsNullOrEmpty(explicitDisplayName) ? Identifier : explicitDisplayName; }
            set { SetField(ref explicitDisplayName, value); }
        }

        // Convenience for use with data-binding
        [XmlIgnore]
        public IdentifiableItem Self
        {
            get { return this; }
        }

        public override string ToString()
        {
            return DisplayName;
        }

        public override int GetHashCode()
        {
            return Identifier?.GetHashCode() ?? 0;
        }
    }

    public class NotifyPropertyChangedBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
