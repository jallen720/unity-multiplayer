using UnityEngine;
using UnityEngine.UI;

namespace UnityMultiplayer {

    [RequireComponent(typeof(Button))]
    public class SignOutButton : MonoBehaviour, IAuthStateListener {
        private Button button;
        private Authenticator authenticator;

        private void Start() {
            button = GetComponent<Button>();
            authenticator = MultiplayerManager.Authenticator;
            Init();
        }

        private void Init() {
            authenticator.AuthStateListeners.Add(this);
            button.onClick.AddListener(authenticator.SignOut);
            button.interactable = authenticator.IsAuthenticated();
        }

        private void OnDestroy() {
            authenticator.AuthStateListeners.Remove(this);
        }

        void IAuthStateListener.OnAuthStateUpdated(bool isAuthenticated) {
            button.interactable = isAuthenticated;
        }
    }
}