using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DungeonInspector
{
    public class BeatEnemiesPuzzle : IPuzzle
    {
        public bool IsCompleted { get; private set; }

        public event Action OnPuzzleCompleted;

        private int _enemiesAlive = 0;


        public void InitPuzzle(params EnemyBase[] enemiesToBeat)
        {
            _enemiesAlive = enemiesToBeat.Length;

            for (int i = 0; i < enemiesToBeat.Length; i++)
            {
                enemiesToBeat[i].OnEnemyBeaten += OnEnemyBeaten;
            }
        }

        private void OnEnemyBeaten(EnemyBase enemy)
        {
            enemy.OnEnemyBeaten -= OnEnemyBeaten;

            _enemiesAlive--;

            if (_enemiesAlive <= 0)
            {
                OnPuzzleCompleted?.Invoke();
            }
        }
    }
}
