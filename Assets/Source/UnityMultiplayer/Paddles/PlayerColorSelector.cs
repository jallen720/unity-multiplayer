using UnityEngine;

namespace UnityMultiplayer {

    [RequireComponent(typeof(PaddleColorSelector))]
    public class PlayerColorSelector : MonoBehaviour {
        private void Start() {
            GetComponent<PaddleColorSelector>().SetColorFor(MultiplayerManager.Client.GetSelf());
        }
    }
}