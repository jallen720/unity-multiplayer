using GooglePlayGames;
using System.Collections.Generic;

namespace UnityMultiplayer {
    public class Authenticator {
        private PlayGamesPlatform playGamesPlatform;

        public List<ISignInListener> SignInListeners {
            get;
            private set;
        }

        public List<ISignOutListener> SignOutListeners {
            get;
            private set;
        }

        public Authenticator(PlayGamesPlatform playGamesPlatform) {
            this.playGamesPlatform = playGamesPlatform;
            SignInListeners = new List<ISignInListener>();
            SignOutListeners= new List<ISignOutListener>();
        }

        public void CheckSignIn() {
            if (!IsAuthenticated()) {
                SignIn();
            }
            else {
                TriggerSignInListeners();
            }
        }

        private void SignIn() {
            playGamesPlatform.localUser.Authenticate(HandleSignIn);
        }

        private void HandleSignIn(bool signedInSuccessfully) {
            if (signedInSuccessfully) {
                DebugUtil.Log("We're signed in! Welcome " + playGamesPlatform.localUser.userName);
                TriggerSignInListeners();
            }
            else {
                DebugUtil.Log("Authentication failed");
            }
        }

        public bool IsAuthenticated() {
            return playGamesPlatform.localUser.authenticated;
        }

        public void SignOut() {
            playGamesPlatform.SignOut();
            TriggerSignOutListeners();
        }

        private void TriggerSignInListeners() {
            foreach (ISignInListener signInListener in SignInListeners) {
                signInListener.OnSignIn();
            }
        }

        private void TriggerSignOutListeners() {
            foreach (ISignOutListener signOutListener in SignOutListeners) {
                signOutListener.OnSignOut();
            }
        }
    }
}