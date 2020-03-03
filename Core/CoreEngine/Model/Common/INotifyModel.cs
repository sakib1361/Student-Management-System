using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace CoreEngine.Model.Common
{
    public  class DBNotifyModel:INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyChange(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}
