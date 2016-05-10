using GooglePlayGames;
using System;
using System.Collections.Generic;

namespace UnityMultiplayer {
    public class Authenticator {
        private PlayGamesPlatform playGamesPlatform;

        public List<IAuthStateListener> AuthStateListeners {
            get;
            private set;
        }

        public Authenticator(PlayGamesPlatform playGamesPlatform) {
            this.playGamesPlatform = playGamesPlatform;
            AuthStateListeners = new List<IAuthStateListener>();
        }

        public void CheckSignIn(Action onSignedIn) {
            if (!IsAuthenticated()) {
                SignIn(onSignedIn);
            }
            else {
                onSignedIn();
            }
        }

        private void SignIn(Action onSignedIn) {
            playGamesPlatform.localUser.Authenticate((bool signedInSuccessfully) => {
                if (signedInSuccessfully) {
                    DebugUtil.Log("We're signed in! Welcome " + playGamesPlatform.localUser.userName);
                    TriggerAuthStateListeners(IsAuthenticated());
                    onSignedIn();
                }
                else {
                    DebugUtil.Log("Authentication failed");
                }
            });
        }

        public bool IsAuthenticated() {
            return playGamesPlatform.localUser.authenticated;
        }

        public void SignOut() {
            playGamesPlatform.SignOut();
            TriggerAuthStateListeners(IsAuthenticated());
        }

        private void TriggerAuthStateListeners(bool isAuthenticated) {
            foreach (IAuthStateListener authStateListener in AuthStateListeners) {
                authStateListener.OnAuthStateUpdated(isAuthenticated);
            }
        }
    }
}