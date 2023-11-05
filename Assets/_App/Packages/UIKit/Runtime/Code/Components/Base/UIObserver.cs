namespace LB.UIKit.Runtime.Components {

    public abstract class UIObserver<T> : UIView where T : UIView {

        private T subject;
        public T Subject {
            get {
                if (subject == null) {
                    subject = GetComponent<T>();
                }
                return subject;
            }
        }

        public abstract void OnValidate();
    }
}