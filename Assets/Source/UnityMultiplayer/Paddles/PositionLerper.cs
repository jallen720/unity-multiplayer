﻿using System;
using UnityEngine;

namespace UnityMultiplayer {
    public class PositionLerper : MonoBehaviour {
        public Func<bool> PositionUpdateCondition {
            get;
            set;
        }

        public Func<float> XPositionGetter {
            get;
            set;
        }

        public Func<float> SpeedGetter {
            get;
            set;
        }

        private void Update() {
            CheckUpdatePosition();
        }

        private void CheckUpdatePosition() {
            if (CanUpdatePosition() && ShouldUpdatePosition()) {
                UpdatePosition();
            }
        }

        private bool CanUpdatePosition() {
            return XPositionGetter != null && SpeedGetter != null;
        }

        private bool ShouldUpdatePosition() {
            return PositionUpdateCondition == null || PositionUpdateCondition();
        }

        private void UpdatePosition() {
            Vector3 position = transform.position;
            position.x = LerpedX(position.x);
            transform.position = position;
        }

        private float LerpedX(float currentX) {
            return Mathf.Lerp(currentX, XPositionGetter(), Time.deltaTime * SpeedGetter());
        }
    }
}