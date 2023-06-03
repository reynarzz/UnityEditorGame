using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DungeonInspector
{
    public class SewersWorld : WorldControllerBase
    {
        private BeatEnemiesPuzzle _beatEnemyPuzzle;

        public SewersWorld(WorldData worldData, PrefabInstantiator prefabInstantiator, TilesDatabase tileDatabase) 
            : base(worldData, prefabInstantiator, tileDatabase)
        {
            _beatEnemyPuzzle = new BeatEnemiesPuzzle();
            BackgroundMusic = "Sewers";
        }

        public override void Init()
        {
            base.Init();

            _beatEnemyPuzzle.InitPuzzle(FindWorldEntitiesOfType<EnemyBase>());
            _beatEnemyPuzzle.OnPuzzleCompleted += OnPuzzleCompleted;
        }

        private void OnPuzzleCompleted()
        {
            _beatEnemyPuzzle.OnPuzzleCompleted -= OnPuzzleCompleted;

            FindWorldEntity("Door").GetComp<Door>().SetDoorStatus(isOpen: true);
        }

        public override void Update()
        {

        }

        public override void OnExit()
        {
            base.OnExit();
        }
    }
}
