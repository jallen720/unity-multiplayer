using UnityEngine;

namespace UnityMultiplayer {

    [RequireComponent(typeof(KeyObserver))]
    public class MainMenuInput : MonoBehaviour {
        private void Start() {
            GetComponent<KeyObserver>().AddKey(new KeyObserver.Key {
                code = KeyCode.Escape,
                onActiveCallback = GameUtil.Quit
            });
        }
    }
}