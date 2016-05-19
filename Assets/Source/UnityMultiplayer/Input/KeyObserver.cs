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

        public void RemoveKey(KeyCode keyCode) {
            Keys.Remove(Keys.Find((Key key) => key.code == keyCode));
        }

        private void Update() {
            CheckForKeyInputs();
        }

        private void CheckForKeyInputs() {
            foreach (Key key in GetActiveKeys()) {
                key.onActiveCallback();
            }
        }

        private List<Key> GetActiveKeys() {
            return Keys.FindAll((Key key) => key.isActivePredicate(key.code));
        }

        public class Key {
            public KeyCode code;
            public Action onActiveCallback;
            public Predicate<KeyCode> isActivePredicate = Input.GetKeyDown;
        }
    }
}