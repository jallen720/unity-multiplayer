using UnityEngine;
using UnityEngine.UI;

namespace UnityMultiplayer {

    [RequireComponent(typeof(Button))]
    public class SignOutButton : MonoBehaviour, IAuthStateListener {
        private Button button;

        private void Start() {
            button = GetComponent<Button>();
            Init();
        }

        private void Init() {
            MultiplayerManager.AddAuthStateListener(this);
            button.onClick.AddListener(MultiplayerManager.SignOut);
        }

        private void OnDestroy() {
            MultiplayerManager.RemoveAuthStateListener(this);
        }

        void IAuthStateListener.UpdateAuthState(bool isAuthenticated) {
            button.interactable = isAuthenticated;
        }
    }
}