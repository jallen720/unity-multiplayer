using GooglePlayGames;
using System;
using System.Collections.Generic;

namespace UnityMultiplayer {
    public class Authenticator {
        private PlayGamesPlatform playGamesPlatform;
        private List<IAuthStateListener> authStateListeners;

        public Authenticator(PlayGamesPlatform playGamesPlatform) {
            this.playGamesPlatform = playGamesPlatform;
            authStateListeners = new List<IAuthStateListener>();
        }

        public void CheckSignIn() {
            if (!IsAuthenticated()) {
                SignIn();
            }
        }

        private void SignIn() {
            playGamesPlatform.localUser.Authenticate(HandleAuthentication);
        }

        private void HandleAuthentication(bool authenticatedSuccessfully) {
            if (authenticatedSuccessfully) {
                DebugUtil.Log("We're signed in! Welcome " + playGamesPlatform.localUser.userName);
                UpdateAuthStateListeners(IsAuthenticated());
            }
            else {
                DebugUtil.Log("Authentication failed");
            }
        }

        public void AddAuthStateListener(IAuthStateListener authStateListener) {
            authStateListeners.Add(authStateListener);
            authStateListener.UpdateAuthState(IsAuthenticated());
        }

        public void RemoveAuthStateListener(IAuthStateListener authStateListener) {
            authStateListeners.Remove(authStateListener);
        }

        private bool IsAuthenticated() {
            return playGamesPlatform.localUser.authenticated;
        }

        private void UpdateAuthStateListeners(bool isAuthenticated) {
            foreach (IAuthStateListener authStateListener in authStateListeners) {
                ValidateAuthStateListener(authStateListener);
                authStateListener.UpdateAuthState(isAuthenticated);
            }
        }

        private void ValidateAuthStateListener(IAuthStateListener authStateListener) {
            if (authStateListener == null) {
                throw new Exception("MultiplayerManager contains a null authStateListener");
            }
        }

        public void SignOut() {
            playGamesPlatform.SignOut();
            UpdateAuthStateListeners(IsAuthenticated());
        }
    }
}