using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DungeonInspector
{
    public class DRendererComponent : DTransformableComponent
    {
        public int ZSorting { get; set; } = 0;
        public Texture2D Texture { get; set; }

        public override DTransformComponent Transform 
        {
            get 
            {
                // below is the fix to the texture ratio
                //TODO: _playerAnimator.CurrentTex.width / _playerAnimator.CurrentTex.height

                return base.Transform; 
            }
            
            set => base.Transform = value; 
        }
    }
}