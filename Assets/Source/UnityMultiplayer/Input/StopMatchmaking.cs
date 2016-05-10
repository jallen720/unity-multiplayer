using UnityEngine;

namespace UnityMultiplayer {

    [RequireComponent(typeof(KeyObserver))]
    public class StopMatchmaking : MonoBehaviour {
        private void Start() {
            GetComponent<KeyObserver>().AddKey(new KeyObserver.Key {
                code = KeyCode.Escape,
                onActiveCallback = MultiplayerManager.StopMatchmaking
            });
        }
    }
}