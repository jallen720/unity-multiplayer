using UnityEngine;
using UnityEngine.UI;

namespace UnityMultiplayer {

    [RequireComponent(typeof(Button))]
    public class SignOutButton : MonoBehaviour {
        private Button button;
        private Authenticator authenticator;

        private void Start() {
            button = GetComponent<Button>();
            authenticator = MultiplayerManager.Authenticator;
            Init();
        }

        private void Init() {
            authenticator.AuthStateUpdatedEvent.Subscribe(OnAuthStateUpdated);
            button.onClick.AddListener(authenticator.SignOut);
            button.interactable = authenticator.IsAuthenticated();
        }

        private void OnDestroy() {
            authenticator.AuthStateUpdatedEvent.Unsubscribe(OnAuthStateUpdated);
        }

        private void OnAuthStateUpdated(bool isAuthenticated) {
            button.interactable = isAuthenticated;
        }
    }
}