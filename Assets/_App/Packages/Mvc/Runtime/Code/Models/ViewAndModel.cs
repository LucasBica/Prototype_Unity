namespace LB.Mvc.Runtime {
    public class ViewAndModel {

        public UIBaseViewUpdater view;
        public ViewUpdaterModel model;

        public ViewAndModel(UIBaseViewUpdater view, ViewUpdaterModel model) {
            this.view = view;
            this.model = model;
        }
    }
}