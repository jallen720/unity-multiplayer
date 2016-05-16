using UnityEngine;

namespace UnityMultiplayer {
    public class ScreenConfigurer : MonoBehaviour {
        private void Awake() {
            Screen.autorotateToPortrait = false;
            Screen.autorotateToPortraitUpsideDown = false;
            Screen.orientation = ScreenOrientation.AutoRotation;
        }
    }
}