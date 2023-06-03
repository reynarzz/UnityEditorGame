using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DungeonInspector
{
    [CreateAssetMenu]
    public class DSpriteAtlasGroup : ScriptableObject, ISerializationCallbackReceiver
    {
        private Dictionary<string, DSpriteAtlas> _sprites;

        [Serializable]
        private struct AtlasInfo
        {
            public string Name;
            public DSpriteAtlas Atlas;
        }

        [SerializeField] private List<AtlasInfo> _atlasInfos;

        public DSpriteAtlasGroup() : base()
        {
            _atlasInfos = new List<AtlasInfo>();
            _sprites = new Dictionary<string, DSpriteAtlas>();
        }

        public void OnBeforeSerialize() { }

        public void OnAfterDeserialize()
        {
            _sprites.Clear();

            for (int i = 0; i < _atlasInfos.Count; i++)
            {
                _sprites.Add(_atlasInfos[i].Name, _atlasInfos[i].Atlas);
            }
        }

        public DSpriteAtlas GetAtlas(string name)
        {
            if (_sprites.TryGetValue(name, out var atlas))
            {
                return atlas;
            }

            Debug.LogError($"Cannot find atlas: {name}");

            return null;
        }
    }
}