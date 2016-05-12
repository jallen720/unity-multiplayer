using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityMultiplayer {

    [RequireComponent(typeof(PositionLerper))]
    public class PlayerController : MonoBehaviour {

        [SerializeField]
        private float speed;

        [SerializeField]
        private int positionEmissionsPerSec;

        private PositionLerper positionLerper;
        private WaitForSeconds positionEmissionIntervalWait;
        private List<byte> positionMessage;

        private void Start() {
            const int POSITION_MESSAGE_SIZE = 6;

            ValidatePositionEmissionsPerSec();
            positionLerper = GetComponent<PositionLerper>();
            positionEmissionIntervalWait = new WaitForSeconds(1f / positionEmissionsPerSec);
            positionMessage = new List<byte>(POSITION_MESSAGE_SIZE);
            Init();
            StartCoroutine(PositionEmissionRoutine());
        }

        private void ValidatePositionEmissionsPerSec() {
            if (positionEmissionsPerSec < 1) {
                throw new Exception(string.Format(
                    "position emissions / second must be greater than 0; was {0}",
                    positionEmissionsPerSec
                ));
            }
        }

        private void Init() {
            positionLerper.PositionUpdateCondition = () => Input.GetMouseButton(0);
            positionLerper.XPositionGetter = MouseWorldXPosition;
            positionLerper.SpeedGetter = GetSpeed;
        }

        private float MouseWorldXPosition() {
            return Camera.main.ScreenToWorldPoint(Input.mousePosition).x;
        }

        public float GetSpeed() {
            return speed;
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
    }
}
