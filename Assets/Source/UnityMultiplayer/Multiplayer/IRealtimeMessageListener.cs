namespace UnityMultiplayer {
    public interface IRealtimeMessageListener {
        void OnReceivedRealtimeMessage(bool isReliable, string participantID, byte[] message);
    }
}
