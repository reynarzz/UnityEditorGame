using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonInspector
{
    public class DTilemap : DBehavior
    {
        public override void OnStart()
        {

        }
        //public int GetTile(int x, int y, int zSorting)
        //{
        //    var layer = GetTileLayers(x, y);

        //    if (layer != null)
        //    {
        //        if (layer.Length > zSorting)
        //        {
        //            return layer[zSorting];
        //        }
        //        else
        //        {
        //            $"Cannot find tile in ZSorting layer: {zSorting}".LOGError();
        //        }
        //    }

        //    return default;
        //}

        //public int[] GetTileLayers(int x, int y)
        //{
        //    if (_data.TryGetValue(new DVector2(x, y), out var tilesLayers))
        //    {
        //        return tilesLayers.ToArray();
        //    }

        //    return default;
        //}


        public bool IsWalkable(int x, int y)
        {
            //var layers = GetTileLayers(x, y);

            //if (layers != null)
            //{
            //    var walkable = true;

            //    for (int i = 0; i < layers.Length; i++)
            //    {
            //        if (!layers[i].IsWalkable)
            //        {
            //            walkable = false;
            //            break;
            //        }
            //    }

            //    return walkable;
            //}
            //else
            //{
            //    return false;
            //}

            return false;

        }

    }
}
