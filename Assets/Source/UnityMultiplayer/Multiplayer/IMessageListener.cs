namespace UnityMultiplayer {
    public interface IMessageListener {
        void OnReceivedMessage(byte[] message);
    }
}
