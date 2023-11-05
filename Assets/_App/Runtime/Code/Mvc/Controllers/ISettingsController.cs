using LB.Mvc.Runtime;

namespace LB.Prototype.Runtime.Mvc {

    public interface ISettingsController : IController<SettingsModel> {

        public void OnClickAppear();

        public void OnSoundsIsOnValueChanged(SettingsModel model);

        public void OnMusicIsOnValueChanged(SettingsModel model);

        public void OnClickSignOut(SettingsModel model);

        public void OnClickCancel(SettingsModel model);
    }
}