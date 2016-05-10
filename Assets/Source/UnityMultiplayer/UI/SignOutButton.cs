using UnityEngine;
using UnityEngine.UI;

namespace UnityMultiplayer {

    [RequireComponent(typeof(Button))]
    public class SignOutButton : MonoBehaviour, ISignInListener, ISignOutListener {
        private Button button;
        private Authenticator authenticator;

        private void Start() {
            button = GetComponent<Button>();
            authenticator = MultiplayerManager.Authenticator;
            Init();
        }

        private void Init() {
            authenticator.SignInListeners.Add(this);
            authenticator.SignOutListeners.Add(this);
            button.onClick.AddListener(authenticator.SignOut);
            button.interactable = authenticator.IsAuthenticated();
        }

        private void OnDestroy() {
            authenticator.SignInListeners.Remove(this);
            authenticator.SignOutListeners.Remove(this);
        }

        void ISignInListener.OnSignIn() {
            button.interactable = true;
        }

        void ISignOutListener.OnSignOut() {
            button.interactable = false;
        }
    }
}