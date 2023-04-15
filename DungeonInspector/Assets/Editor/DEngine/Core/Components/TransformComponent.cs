using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DungeonInspector
{
    public class DTransformComponent : DComponent
    {
         public DVector2 Position { get; set; }
        //private DVector2 _position;
        //public DVector2 Position 
        //{
        //    get => _position; set => _position = value * DCamera.PixelsPerUnit; 
        //}

        public DVector2 Scale { get; set; } = new DVector2(1, 1);
    }
}