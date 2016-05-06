using UnityEngine;

namespace UnityMultiplayer {
    public class PlayerPaddleController : MonoBehaviour {

        [SerializeField]
        private float speed;

        private void Update() {
            CheckInput();
        }

        private void CheckInput() {
            if (Input.GetMouseButton(0)) {
                UpdatePosition();
            }
        }

        private void UpdatePosition() {
            Vector3 position = transform.position;
            position.x = LerpedX(position.x);
            transform.position = position;
        }

        private float LerpedX(float currentX) {
            return Mathf.Lerp(currentX, MouseWorldPositionX(), Time.deltaTime * speed);
        }

        private float MouseWorldPositionX() {
            return Camera.main.ScreenToWorldPoint(Input.mousePosition).x;
        }
    }
}
