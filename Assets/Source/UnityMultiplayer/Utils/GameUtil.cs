using UnityEngine;

#if UNITY_EDITOR
using UnityEngine.SceneManagement;
#endif

namespace UnityMultiplayer {
    public static class GameUtil {
        public static void Quit() {
#if UNITY_EDITOR
            SceneManager.LoadScene("MainMenu");
#else
            Application.Quit();
#endif
        }

        public static void Pause() {
            Time.timeScale = 0f;
        }

        public static void UnPause() {
            Time.timeScale = 1f;
        }
    }
}