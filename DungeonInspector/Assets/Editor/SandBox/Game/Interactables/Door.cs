using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace DungeonInspector
{
    public class Door : InteractableBase
    {
        private DRendererComponent _renderer;
        private DSpriteAtlas _atlas;

        protected override void OnAwake()
        {
            _renderer = GetComp<DRendererComponent>();
            _renderer.Sprite = _atlas.GetTexture(0);
        }

        public void SetAtlas(DSpriteAtlas atlas)
        {
            _atlas = atlas;
        }

        protected override void OnUpdate()
        {
            //GUI.DrawTexture(new Rect(200, 200, 100, 100), _renderer.Sprite);

        }
        public override void OnInteracted(InteractionType interaction, Actor actor)
        {
        }
    }
}
