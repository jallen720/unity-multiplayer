using GooglePlayGames;

namespace UnityMultiplayer {
    public class Matchmaker {
        private PlayGamesPlatform playGamesPlatform;

        public RealtimeListener RealtimeListener {
            get;
            private set;
        }

        public Matchmaker(PlayGamesPlatform playGamesPlatform) {
            this.playGamesPlatform = playGamesPlatform;
            RealtimeListener = new RealtimeListener();
        }

        public void StartMatchmaking() {
            playGamesPlatform.RealTime.CreateQuickGame(
                minOpponents: 1,
                maxOpponents: 1,
                variant: 0,
                listener: RealtimeListener
            );
        }
    }
}