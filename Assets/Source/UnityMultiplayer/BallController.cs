using UnityEngine;

namespace UnityMultiplayer {
    [RequireComponent(typeof(Rigidbody2D))]
    public class BallController : MonoBehaviour {

        [SerializeField]
        private float speed;

        private new Rigidbody2D rigidbody2D;

        private void Start() {
            rigidbody2D = GetComponent<Rigidbody2D>();
            Init();
        }

        private void Init() {
            rigidbody2D.velocity = Vector2.one * speed;
        }
    }
}