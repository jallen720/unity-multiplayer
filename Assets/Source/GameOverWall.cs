using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverWall : MonoBehaviour {
    private void OnTriggerEnter2D(Collider2D collider) {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}