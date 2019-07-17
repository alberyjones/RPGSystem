using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace RPGApp.ViewModel
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        private Dictionary<INotifyPropertyChanged, string> propNameMap = new Dictionary<INotifyPropertyChanged, string>();

        public event PropertyChangedEventHandler PropertyChanged;

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            INotifyPropertyChanged iNotify = field as INotifyPropertyChanged;
            if (iNotify != null && propNameMap.ContainsKey(iNotify))
            {
                iNotify.PropertyChanged -= nestedNotify_PropertyChanged;
                propNameMap.Remove(iNotify);
            }
            field = value;
            if (!String.IsNullOrEmpty(propertyName))
            {
                OnPropertyChanged(propertyName);
                iNotify = value as INotifyPropertyChanged;
                if (iNotify != null)
                {
                    iNotify.PropertyChanged += nestedNotify_PropertyChanged;
                    propNameMap.Add(iNotify, propertyName);
                }
            }
            return true;
        }

        private void nestedNotify_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            INotifyPropertyChanged iNotify = sender as INotifyPropertyChanged;
            if (iNotify != null && propNameMap.ContainsKey(iNotify))
            {
                OnPropertyChanged(propNameMap[iNotify]);
            }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class EditableViewModel : ViewModelBase
    {
        private bool canEdit;
        public bool CanEdit
        {
            get => canEdit;
            set
            {
                if (SetField(ref canEdit, value))
                {
                    // we can no longer edit, switch out of edit mode if needed
                    if (!canEdit) IsEditing = false;
                }
            }
        }

        private bool isEditing;
        public bool IsEditing
        {
            get => isEditing;
            set
            {
                if (SetField(ref isEditing, value))
                {
                    OnPropertyChanged(nameof(IsReadOnly));
                }
            }
        }

        public bool IsReadOnly
        {
            get { return !IsEditing; }
        }

        public ICommand BeginEdit { get; private set; }

        public ICommand EndEdit { get; private set; }

        public EditableViewModel()
        {
            BeginEdit = new CustomCommand(DoBeginEdit, CanBeginEdit);
            EndEdit = new CustomCommand(DoEndEdit, IsEditingItem);
        }

        protected virtual bool CanBeginEdit(object parameters)
        {
            return CanEdit && !IsEditing;
        }

        protected virtual bool IsEditingItem(object parameters)
        {
            return IsEditing;
        }

        protected virtual void DoBeginEdit(object parameters)
        {
            IsEditing = true;
        }

        protected virtual void DoEndEdit(object parameters)
        {
            IsEditing = false;
        }
    }
}
