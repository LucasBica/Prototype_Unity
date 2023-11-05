using System;

using LB.Http.Models;
using LB.MessageSystem.Runtime;

namespace LB.Mvc.Runtime {

    public abstract class Controller<T> : ControllerBase, IController<T> where T : ViewUpdaterModel {

        public event Action<T> OnPropertyChanged;

        protected virtual void CallOnPropertyChanged(T model) {
            OnPropertyChanged?.Invoke(model);
        }

        protected virtual void AppearError(HttpError httpError, AlertModel alertModel, IAlertController alertController, IMessenger messenger) {
            alertModel.shouldAppear = true;
            alertModel.animated = true;
            alertModel.title = httpError.error.name;
            alertModel.description = httpError.error.message;
            if (IsDevelopmentBuild) alertModel.description += $"\n{httpError.error.details}";
            alertModel.controllerDelegated = alertController;
            messenger.Send(MvcCallbacks.UpdateAlertView, alertModel);
        }
    }
}