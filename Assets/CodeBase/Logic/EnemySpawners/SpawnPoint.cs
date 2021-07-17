using CodeBase.Data;
using CodeBase.Enemy;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.StaticData;
using UnityEngine;

namespace CodeBase.Logic.EnemySpawners
{
    public class SpawnPoint : MonoBehaviour, ISavedProgress
    {
        public MonsterTypeId monsterType;
        public string Id { get; set; }
        
        public bool slain;
        
        private IGameFactory _gameFactory;
        private EnemyDeath _enemyDeath;

        public void Construct(IGameFactory factory) => 
            _gameFactory = factory;

        public void LoadProgress(PlayerProgress progress)
        {
            if (progress.KillData.ClearedSpawners.Contains(Id))
                slain = true;
            else
                Spawn();
        }

        private async void Spawn()
        {
            GameObject monster = await _gameFactory.CreateMonster(monsterType, transform);
            _enemyDeath = monster.GetComponent<EnemyDeath>();
            _enemyDeath.Happened += Slay;
        }

        private void Slay()
        {
            if (_enemyDeath != null) 
                _enemyDeath.Happened -= Slay;
            
            slain = true;
        }

        public void UpdateProgress(PlayerProgress progress)
        {
            if (slain) 
                progress.KillData.ClearedSpawners.Add(Id);
        }
    }
}