using System;
using System.Collections.Generic;
using FlappyStar.Actors;
using FlappyStar.Core;
using FlappyStar.Gameplay.Entities;
using LSH.Core;
using LSH.Utils.Pooling;
using UnityEngine;

namespace FlappyStar.Gameplay.InGame
{
    public class ObstacleManager : MonoBehaviour
    {
        [SerializeField] private ObstacleSpawnSettings _settings;

        [SerializeField] private List<SFXID> _crowdSounds;

        private readonly List<ObstaclePair> _activePairs = new();

        private SinglePrefabPool<ObstaclePair> _pairPool;
        private PrefabVariantPool<Obstacle> _partPool;

        private InGameTimeManager _timeManager;
        private PlayArea _playArea;

        private float _playerHeight;
        private float _spawnTimer;

        public event Action<int> OnPlayerPassed;
        public event Action OnPlayerTouched;

        public void Init(PlayerActor player, InGameTimeManager timeManager, PlayArea playArea)
        {
            _timeManager = timeManager;
            _playArea = playArea;

            _playerHeight = player.PlayerHeight;

            _pairPool = new SinglePrefabPool<ObstaclePair>(
                _settings.ObstaclePairPrefab,
                transform);

            _partPool = new PrefabVariantPool<Obstacle>(
                _settings.ObstaclePartPrefabs,
                transform);

            _pairPool.Prewarm(_settings.PairPrewarmCount);
            _partPool.Prewarm(_settings.PartPrewarmCountPerPrefab);

            _spawnTimer = _settings.SpawnInterval;

        }
        private void Update()
        {
            if (_timeManager == null)
                return;

            if (_timeManager.IsGamePaused) return;

            float deltaTime = _timeManager.GameDeltaTime;

            UpdateSpawn(deltaTime);
            UpdateMovement(deltaTime);
        }

        private void UpdateSpawn(float deltaTime)
        {
            _spawnTimer += deltaTime;

            if (_spawnTimer < _settings.SpawnInterval)
                return;

            _spawnTimer = 0f;
            SpawnPair();
        }

        private void SpawnPair()
        {
            Obstacle upperPrefab = _settings.GetRandomPartPrefab();
            Obstacle lowerPrefab = _settings.GetRandomPartPrefab();

            if (upperPrefab == null || lowerPrefab == null)
                return;

            ObstaclePair pair = _pairPool.Get();

            Obstacle upperObstacle = _partPool.Get(upperPrefab);
            Obstacle lowerObstacle = _partPool.Get(lowerPrefab);

            float gapHeight = CalculateGapHeight();
            float halfGapHeight = gapHeight * 0.5f;
            float gapCenterY = GetRandomGapCenterY(halfGapHeight);

            pair.transform.position = new Vector3(
                transform.position.x,
                gapCenterY,
                transform.position.z);

            pair.Activate(upperObstacle, lowerObstacle, gapHeight, _playArea.TotalY);

            pair.OnPlayerPassed += HandlePlayerPassed;
            pair.OnPlayerTouched += HandlePlayerTouched;
            _activePairs.Add(pair);
        }

        private void UpdateMovement(float deltaTime)
        {
            for (int i = _activePairs.Count - 1; i >= 0; i--)
            {
                ObstaclePair pair = _activePairs[i];

                pair.Move(deltaTime, _settings.ScrollSpeed);

                if (pair.transform.position.x > _playArea.DespawnX)
                    continue;

                ReleasePairAt(i);
            }
        }

        private void ReleasePairAt(int index)
        {
            ObstaclePair pair = _activePairs[index];

            pair.OnPlayerPassed -= HandlePlayerPassed;
            pair.OnPlayerTouched -= HandlePlayerTouched;

            _activePairs.RemoveAt(index);

            pair.DetachObstacles(
                out Obstacle upperObstacle,
                out Obstacle lowerObstacle);

            if (upperObstacle != null)
                _partPool.Release(upperObstacle);

            if (lowerObstacle != null)
                _partPool.Release(lowerObstacle);

            _pairPool.Release(pair);
        }

        private float CalculateGapHeight()
        {
            float clearance = Mathf.Max(
                _settings.BaseGapClearance
                - _settings.GapClearanceDecreasePerSecond * _timeManager.GameTime,
                _settings.MinGapClearance);

            return _playerHeight + clearance;
        }

        private float GetRandomGapCenterY(float halfGapHeight)
        {
            float minY = _playArea.FloorY
                + halfGapHeight
                + _settings.VerticalMargin;


            float maxY = _playArea.CeilingY
                - halfGapHeight
                - _settings.VerticalMargin;

            if (minY > maxY)
                return (_playArea.CeilingY + _playArea.FloorY) * 0.5f;


            return UnityEngine.Random.Range(minY, maxY);
        }

        private void HandlePlayerPassed(int score)
        {
            if (_crowdSounds.Count > 0)
            {
                int index = UnityEngine.Random.Range(0, _crowdSounds.Count);
                SoundManager.Instance.PlaySFX(SFXID.GamePlay_Switch);
                SoundManager.Instance.PlaySFX(_crowdSounds[index]);
            }

            OnPlayerPassed?.Invoke(score);
        }
        private void HandlePlayerTouched()
        {
            OnPlayerTouched?.Invoke();
        }
    }
}

