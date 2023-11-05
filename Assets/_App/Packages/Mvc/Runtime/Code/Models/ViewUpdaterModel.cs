using LB.Core.Runtime.Utilities;

namespace LB.Mvc.Runtime {

    public class ViewUpdaterModel {

        public bool shouldAppear;
        public bool shouldDisappear;
        public bool animated;
        public bool isLoading;

        public override string ToString() {
            return StaticUtilities.ToString(this);
        }
    }
}