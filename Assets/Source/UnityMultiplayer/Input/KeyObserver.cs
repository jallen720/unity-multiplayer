using System;
using UnityEngine;

namespace UnityMultiplayer {
    public sealed class KeyObserver {
        private KeyCode keyCode;
        private Action onKeyActiveCallback;
        private Predicate<KeyCode> isKeyActivePredicate;

        public KeyObserver(
            KeyCode keyCode,
            Action onKeyActiveCallback,
            Predicate<KeyCode> isKeyActivePredicate)
        {
            this.keyCode = keyCode;
            this.onKeyActiveCallback = onKeyActiveCallback;
            this.isKeyActivePredicate = isKeyActivePredicate;
        }

        public KeyObserver(KeyCode keyCode, Action onKeyActiveCallback)
            : this(keyCode, onKeyActiveCallback, Input.GetKeyDown)
        {}

        public void CheckInput() {
            if (isKeyActivePredicate(keyCode)) {
                onKeyActiveCallback();
            }
        }
    }
}