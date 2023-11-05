using System;

namespace LB.Mvc.Runtime {

    public interface IController<T> where T : ViewUpdaterModel {

        public event Action<T> OnPropertyChanged;
    }
}