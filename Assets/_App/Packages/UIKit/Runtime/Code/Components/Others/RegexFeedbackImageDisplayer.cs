using UnityEngine.UI;
using UnityEngine;

namespace LB.UIKit.Runtime.Components {

    public class RegexFeedbackImageDisplayer : UIObserver<RegexFeedbackDisplayer> {

        [Header("Settings")]
        [SerializeField] private Sprite spriteDefault = default;
        [SerializeField] private Sprite spriteMatch = default;
        [SerializeField] private Sprite spriteNoMatch = default;

        [Header("References")]
        [SerializeField] private Image[] images = default;

        public override void OnValidate() {

        }

        private void Awake() {
            Subject.OnMatchChanged += Subject_OnMatchChanged;
        }

        private void Subject_OnMatchChanged(RegexFeedbackDisplayer regexFeedbackDisplayer, bool? isMatch) {
            for (int i = 0; i < images.Length; i++) {
                if (isMatch == null) {
                    images[i].sprite = spriteDefault;
                    continue;
                }

                images[i].sprite = isMatch.Value ? spriteMatch : spriteNoMatch;
            }
        }
    }
}