using GooglePlayGames;
using System;
using UnityUtils.EventUtils;

namespace UnityMultiplayer {
    public class Authenticator {
        private PlayGamesPlatform playGamesPlatform;

        public Event<bool> AuthStateUpdatedEvent {
            get;
            private set;
        }

        public Authenticator(PlayGamesPlatform playGamesPlatform) {
            this.playGamesPlatform = playGamesPlatform;
            AuthStateUpdatedEvent = new Event<bool>();
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
                    AuthStateUpdatedEvent.Trigger(IsAuthenticated());
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
            AuthStateUpdatedEvent.Trigger(IsAuthenticated());
        }
    }
}