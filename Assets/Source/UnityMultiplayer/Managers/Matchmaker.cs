using GooglePlayGames;

namespace UnityMultiplayer {
    public class Matchmaker : IAuthStateListener {
        private PlayGamesPlatform playGamesPlatform;
        private RealtimeListener realtimeListener;

        public Matchmaker(PlayGamesPlatform playGamesPlatform) {
            this.playGamesPlatform = playGamesPlatform;
            realtimeListener = new RealtimeListener();
        }

        void IAuthStateListener.UpdateAuthState(bool isAuthenticated) {
            if (isAuthenticated) {
                StartMatchmaking();
            }
        }

        private void StartMatchmaking() {
            playGamesPlatform.RealTime.CreateQuickGame(
                minOpponents: 1,
                maxOpponents: 1,
                variant: 0,
                listener: realtimeListener
            );
        }
    }
}