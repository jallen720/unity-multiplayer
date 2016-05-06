using UnityEngine;
using UnityEngine.SceneManagement;

namespace UnityMultiplayer {
    public class GameOverWall : MonoBehaviour {
        private void OnTriggerEnter2D(Collider2D collider) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}