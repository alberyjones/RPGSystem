using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGApp.ViewModel
{
    class BindingListWithSource<T> : BindingList<T>
    {
        private List<T> source;
        private bool internalUpdate;

        public List<T> SourceList
        {
            get { return source; }
            set
            {
                if (source != value && !internalUpdate)
                {
                    internalUpdate = true;
                    source = value;
                    this.Clear();
                    if (source != null)
                    {
                        foreach (T item in source)
                        {
                            this.Add(item);
                        }
                    }
                    internalUpdate = false;
                }
            }
        }

        protected override void OnListChanged(ListChangedEventArgs e)
        {
            if (!internalUpdate)
            {
                switch (e.ListChangedType)
                {
                    case ListChangedType.ItemAdded:
                        if (source != null)
                        {
                            source.Add(this[e.NewIndex]);
                        }
                        break;
                    case ListChangedType.Reset:
                    case ListChangedType.ItemDeleted:
                        if (source != null)
                        {
                            source.Clear();
                            source.AddRange(this);
                        }
                        break;
                }
            }
        }

        public static void SetSource(BindingList<T> target, List<T> source)
        {
            if (target is BindingListWithSource<T> listWithSource)
            {
                listWithSource.SourceList = source;
            }
        }
    }
}
