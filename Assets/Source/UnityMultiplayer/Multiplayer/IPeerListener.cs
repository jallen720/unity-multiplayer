namespace UnityMultiplayer {
    public interface IPeerListener {
        void OnPeerConnected(string participantID);
        void OnPeerDisconnected(string participantID);
    }
}
