using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityMultiplayer {
    public class KeyObserver : MonoBehaviour {
        private List<Key> keys;

        private List<Key> Keys {
            get {
                return keys ?? LoadKeys();
            }
        }

        private List<Key> LoadKeys() {
            return keys = new List<Key>();
        }

        public void AddKey(Key key) {
            Keys.Add(key);
        }

        private void Update() {
            CheckForKeyInputs();
        }

        private void CheckForKeyInputs() {
            foreach (Key key in Keys) {
                if (key.isActivePredicate(key.code)) {
                    key.onActiveCallback();
                }
            }
        }

        public class Key {
            public KeyCode code;
            public Action onActiveCallback;
            public Predicate<KeyCode> isActivePredicate = Input.GetKeyDown;
        }
    }
}