using System;
using System.Collections.Generic;
using System.Linq;
using sim2kid.Package.SerializedDictionary.Runtime.Parsers;
using UnityEngine;
using Object = UnityEngine.Object;

namespace sim2kid.Package.SerializedDictionary.Runtime
{
    
    /// <summary>
    /// This serializable dictionary utilizes a parser via the <see cref="IParser"/> interface allowing for more complicated items to be serialized.
    /// In addition, Unity Object references are also properly tracked.
    /// </summary>
    /// <typeparam name="TKey">They key type. Must not be a Unity Object. Must be serializable.</typeparam>
    /// <typeparam name="TValue">The value type. Can be any serializable object or Unity Reference Object.</typeparam>
    [Serializable]
    public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
    {
        /// <summary>
        /// The parser to use when serializing the keys and values.
        /// </summary>
        protected IParser Parser;
        
        /// <summary>
        /// Holds all serialized items in the dictionary.
        /// </summary>
        [SerializeField, HideInInspector]
        protected List<SaveItem> saveItems;
        
        /// <summary>
        /// Holds all unity object items in the dictionary.
        /// </summary>
        [SerializeField, HideInInspector]
        protected List<UnitySaveItem> unitySaveItems;

        #region Constructors 
        public SerializableDictionary(IParser parser) : 
            base()
        {
            Parser = parser;
        }
        public SerializableDictionary(IDictionary<TKey, TValue> dictionary, IParser parser) : 
            base(dictionary)
        {
            Parser = parser;
        }
        public SerializableDictionary(IDictionary<TKey, TValue> dictionary, IEqualityComparer<TKey> comparer, IParser parser) : base(dictionary, comparer)
        {
            Parser = parser;
        }

        public SerializableDictionary(IEnumerable<KeyValuePair<TKey, TValue>> collection, IParser parser) : base(collection)
        {
            Parser = parser;
        }

        public SerializableDictionary(IEnumerable<KeyValuePair<TKey, TValue>> collection,
            IEqualityComparer<TKey> comparer, IParser parser) : base(collection, comparer)
        {
            Parser = parser;
        }

        public SerializableDictionary(IEqualityComparer<TKey> comparer, IParser parser) : base(comparer)
        {
            Parser = parser;
        }

        public SerializableDictionary(int capacity, IParser parser) : base(capacity)
        {
            Parser = parser;
        }

        public SerializableDictionary(int capacity, IEqualityComparer<TKey> comparer, IParser parser) : base(capacity,
            comparer)
        {
            Parser = parser;
        }
        #endregion
        

        public void OnBeforeSerialize()
        {
            saveItems ??= new List<SaveItem>();
            unitySaveItems ??= new List<UnitySaveItem>();
            
            saveItems.Clear();
            unitySaveItems.Clear();
            
            int index = 0;
            foreach (var keyValues in this)
            {
                string key = Parser.ObjectToString(keyValues.Key);
                if (keyValues.Value.GetType().IsSubclassOf(typeof(UnityEngine.Object)))
                {
                    UnityEngine.Object value = keyValues.Value as UnityEngine.Object;
                    unitySaveItems.Add(new UnitySaveItem(key, value, index));
                }
                else
                {
                    string value = Parser.ObjectToString(keyValues.Value);
                    saveItems.Add(new SaveItem(key, value, index));
                }
                index++;
            }
        }

        public void OnAfterDeserialize()
        {
            this.Clear();
            
            List<ISaveItem> items = new List<ISaveItem>();
            foreach (var saveItem in saveItems)
            {
                items.Add(saveItem);
            }
            foreach (var unitySaveItem in unitySaveItems)
            {
                items.Add(unitySaveItem);
            }

            foreach (var item in items.OrderBy(item => item.Index))
            {
                if (item is SaveItem saveItem)
                {
                    TKey key = (TKey)Parser.StringToObject(saveItem.key);
                    TValue value = (TValue)Parser.StringToObject(saveItem.value);
                    
                    this.Add(key, value);
                }

                /*
                 // TODO: Figure out how to convert back to TValue 
                if (typeof(TValue).IsAssignableFrom(typeof(UnityEngine.Object)) && 
                    item is UnitySaveItem unitySaveItem)
                {
                    TKey key = (TKey)Parser.StringToObject(unitySaveItem.key);
                    TValue value =  unitySaveItem.value as TValue;
                    this.Add(key, value);
                }
                */
            }
        }
        
        protected interface ISaveItem
        {
            public int Index { get; }
        }
        
        [Serializable]
        protected struct SaveItem : ISaveItem
        {
            public string key;
            public string value;
            public int index;
            public int Index => index;
            
            public SaveItem(string key, string value, int index)
            {
                this.key = key;
                this.value = value;
                this.index = index;
            }
        }
        [Serializable]
        protected struct UnitySaveItem : ISaveItem
        {
            public string key;
            public UnityEngine.Object value;
            public int index;
            public int Index => index;

            public UnitySaveItem(string key, UnityEngine.Object value, int index)
            {
                this.key = key;
                this.value = value;
                this.index = index;
            }
        }
    }

    
}
