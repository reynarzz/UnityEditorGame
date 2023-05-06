using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonInspector
{
    public class GameInput
    {
        public bool Enabled { get; set; } = true;

        public Action OnFace1 { get; set; }
        public Action OnFace2 { get; set; }
        public Action OnFace3 { get; set; }
        public Action OnFace4 { get; set; }

        public Action<DVec2> OnLeftStick { get; set; }
        public Action<DVec2> OnRightStick { get; set; }

        
        public void Update()
        {

        }
    }
}
