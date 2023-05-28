using System;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonInspector
{
    public class DTilemapRendererComponent : DRendererComponent
    {
        public List<Texture2D> Textures { get; private set; }

        public DTilemapRendererComponent() : base()
        {
            Textures = new List<Texture2D>();
        }
    }
}