using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UnityMultiplayer {
    
    [RequireComponent(typeof(Button))]
    public class PlayButton : MonoBehaviour, ISignInListener {
        private Authenticator authenticator;

        private void Start() {
            authenticator = MultiplayerManager.Authenticator;
            Init();
        }

        private void Init() {
            authenticator.SignInListeners.Add(this);
            GetComponent<Button>().onClick.AddListener(authenticator.CheckSignIn);
        }

        private void OnDestroy() {
            authenticator.SignInListeners.Remove(this);
        }

        void ISignInListener.OnSignIn() {
            SceneManager.LoadScene("Lobby");
        }
    }
}