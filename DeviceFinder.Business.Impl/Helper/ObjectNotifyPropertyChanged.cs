using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using DeviceFinder.Business.Impl.Annotations;

namespace DeviceFinder.Business.Impl.Helper
{
    public class ObjectNotifyPropertyChanged : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        [DebuggerStepThrough]
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        [DebuggerStepThrough]
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged<TProperty>(Expression<Func<TProperty>> property)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(property.Name));
        }
    }
}