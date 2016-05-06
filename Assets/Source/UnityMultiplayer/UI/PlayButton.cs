using UnityEngine;
using UnityEngine.UI;
using UnityMultiplayer.Managers;

namespace UnityMultiplayer.UI {
    
    [RequireComponent(typeof(Button))]
    public class PlayButton : MonoBehaviour {
        private void Start() {
            GetComponent<Button>().onClick.AddListener(MultiplayerManager.SignInAndStartMPGame);
        }
    }
}