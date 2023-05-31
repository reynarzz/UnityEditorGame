using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DungeonInspector
{
    public class SmartEvent<T> where T : Delegate
    {
        private T _delegate;
        public void SubscribeRemoveOnFire(T callback)
        {

        }

        //public static T operator+=(SmartEvent<T> a, T callback)
        //{
        //    a._delegate. += callback;
        //}

    }
    public class SewersWorld : WorldControllerBase
    {
        private BeatEnemiesPuzzle _beatEnemyPuzzle;

        public SewersWorld(WorldData worldData, PrefabInstantiator prefabInstantiator, TilesDatabase tileDatabase) : base(worldData, prefabInstantiator, tileDatabase)
        {
            _beatEnemyPuzzle = new BeatEnemiesPuzzle();
        }

        public override void Init()
        {
            base.Init();

            _beatEnemyPuzzle.InitPuzzle(FindWorldEntitiesOfType<EnemyBase>());
            _beatEnemyPuzzle.OnPuzzleCompleted += OnPuzzleCompleted;

            DAudio.PlayAudio("Sewers");
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

            DAudio.StopAudio("Sewers");
        }
    }
}
