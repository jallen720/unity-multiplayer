namespace UnityMultiplayer {
    public interface IAuthStateListener {
        void OnAuthStateUpdated(bool isAuthenticated);
    }
}
