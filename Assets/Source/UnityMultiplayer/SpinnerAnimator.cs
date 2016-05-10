using System.Collections;
using UnityEngine;

namespace UnityMultiplayer {

    [RequireComponent(typeof(RectTransform))]
    public class SpinnerAnimator : MonoBehaviour {

        [SerializeField]
        private float rotateIntervalSecs;

        private RectTransform rectTransform;
        private WaitForSeconds rotateIntervalWait;

        private void Start() {
            rectTransform = GetComponent<RectTransform>();
            rotateIntervalWait = new WaitForSeconds(rotateIntervalSecs);
            StartCoroutine(AnimationRoutine());
        }

        private IEnumerator AnimationRoutine() {
            while (true) {
                Rotate();
                yield return rotateIntervalWait;
            }
        }

        private void Rotate() {
            rectTransform.Rotate(Vector3.forward * 45f);
        }
    }
}