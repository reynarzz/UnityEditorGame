using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonInspector
{
    //TODO: refactor
    public class DAudio
    {
        public static void PlayAudio(string audioDPath)
        {
            DIEngineCoreServices.Get<DAudioSystem>().Play(audioDPath);
        }

        public static void StopAudio(string audioDPath)
        {
            DIEngineCoreServices.Get<DAudioSystem>().StopAudio(audioDPath);
        }
    }
}
