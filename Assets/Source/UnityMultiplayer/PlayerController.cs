using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityMultiplayer {
    public class PlayerController : MonoBehaviour {

        [SerializeField]
        private float speed;

        [SerializeField]
        private float positionEmissionIntervalSecs;

        private WaitForSeconds positionEmissionIntervalWait;
        private List<byte> positionMessage;

        public float Speed {
            get {
                return speed;
            }
        }

        private void Start() {
            const int POSITION_MESSAGE_SIZE = 6;

            positionEmissionIntervalWait = new WaitForSeconds(positionEmissionIntervalSecs);
            positionMessage = new List<byte>(POSITION_MESSAGE_SIZE);
            StartCoroutine(PositionEmissionRoutine());
        }

        private IEnumerator PositionEmissionRoutine() {
            while (true) {
                yield return positionEmissionIntervalWait;
                EmitPosition();
            }
        }

        private void EmitPosition() {
            MessageUtil.InitMessage(positionMessage);
            positionMessage.AddRange(BitConverter.GetBytes(transform.position.x));
            MultiplayerManager.Client.SendMessageToAll(false, positionMessage.ToArray());
        }

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
