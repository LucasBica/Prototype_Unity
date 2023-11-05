using System;

using LB.Authentication.Runtime.Mvc;
using LB.Core.Runtime;
using LB.MessageSystem.Runtime;
using LB.Mvc.Runtime;

namespace LB.Prototype.Runtime.Mvc {

    public class SettingsController : Controller<SettingsModel>, ISettingsController {

        private readonly IMessenger messenger;
        private readonly ISignOutController signOutController;
        private readonly ICacheAsTextService cacheAsTextService;
        private readonly Func<ChoiceConfirmationModel> funcChoiceConfirmationModel;

        public SettingsController(IMessenger messenger, ISignOutController signOutController, ICacheAsTextService cacheAsTextService, Func<ChoiceConfirmationModel> funcChoiceConfirmationModel) {
            this.messenger = messenger;
            this.signOutController = signOutController;
            this.cacheAsTextService = cacheAsTextService;
            this.funcChoiceConfirmationModel = funcChoiceConfirmationModel;
        }

        public void OnClickAppear() {
            SettingsCacheModel settingsCacheModel = cacheAsTextService.Get(nameof(SettingsCacheModel), new SettingsCacheModel {
                soundsIsOn = true,
                musicIsOn = true,
            });

            messenger.Send(PrototypeCallbacks.UpdateSettings, new SettingsModel {
                shouldAppear = true,
                animated = true,
                cacheModel = settingsCacheModel
            });
        }

        public void OnSoundsIsOnValueChanged(SettingsModel model) {

        }

        public void OnMusicIsOnValueChanged(SettingsModel model) {

        }

        public void OnClickSignOut(SettingsModel model) {
            ChoiceConfirmationModel choiceConfirmationModel = funcChoiceConfirmationModel.Invoke();
            choiceConfirmationModel.shouldAppear = true;
            choiceConfirmationModel.animated = true;
            choiceConfirmationModel.controllerDelegated = signOutController;

            messenger.Send(MvcCallbacks.UpdateChoiceConfirmationView, choiceConfirmationModel);
        }

        public void OnClickCancel(SettingsModel model) {
            cacheAsTextService.Set(nameof(SettingsCacheModel), model.cacheModel);
            model.shouldDisappear = true;
            CallOnPropertyChanged(model);
        }
    }
}