using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace sim2kid.Package.SerializedDictionary.Runtime
{
    /// <summary>
    /// Allows for natively serializable items to be serialized and deserialize from the dictionary.
    /// </summary>
    /// <typeparam name="TKey">The key type for the dictionary.</typeparam>
    /// <typeparam name="TValue">The value type for the dictionary.</typeparam>
    [Serializable]
    public class SimpleSerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
    {
        /// <summary>
        /// Holds all serialized items in the dictionary.
        /// </summary>
        [SerializeField, HideInInspector]
        protected List<SaveItem> saveItems;

        
        public virtual void OnBeforeSerialize()
        {
            if (saveItems == null)
            {
                saveItems = new List<SaveItem>();
            }
            else
            {
                saveItems.Clear();
            }

            int index = 0;
            foreach (var keyValues in this)
            {
                var key = keyValues.Key;
                var value = keyValues.Value;
                
                saveItems.Add(new SaveItem(key, value, index));
                index++;
            }
        }

        public virtual void OnAfterDeserialize()
        {
            this.Clear();
            foreach (var saveItem in saveItems.OrderBy(x => x.index))
            {
                var key = saveItem.key;
                var value = saveItem.value;
                
                this.Add(key, value);
            }
        }
        
        /// <summary>
        /// This struct represents the string key, and the serialized value from the dictionary.
        /// </summary>
        [Serializable]
        protected struct SaveItem
        {
            public TKey key;
            public TValue value;
            public int index;

            public SaveItem(TKey key, TValue value, int index)
            {
                this.key = key;
                this.value = value;
                this.index = index;
            }
        }
        
    }
}
