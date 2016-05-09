using UnityEngine;
using UnityEngine.UI;

namespace UnityMultiplayer {
    
    [RequireComponent(typeof(Button))]
    public class PlayButton : MonoBehaviour {
        private void Start() {
            GetComponent<Button>().onClick.AddListener(MultiplayerManager.Authenticator.CheckSignIn);
        }
    }
}