using UnityEngine;

namespace UnityMultiplayer {

    [RequireComponent(typeof(PaddleColorSelector))]
    public class OpponentColorSelector : MonoBehaviour {
        private void Start() {
            GetComponent<PaddleColorSelector>().SetColorFor(MultiplayerManager.GetOpponent());
        }
    }
}