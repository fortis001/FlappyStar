using System.Collections.Generic;
using FlappyStar.Gameplay.Entities;
using UnityEngine;

namespace FlappyStar.Gameplay.InGame
{
    [CreateAssetMenu(
        fileName = "ObstacleSpawnSettings",
        menuName = "FlappyStar/Obstacle Spawn Settings")]
    public class ObstacleSpawnSettings : ScriptableObject
    {
        [Header("Prefab")]
        [SerializeField] private ObstaclePair _obstaclePairPrefab;
        [SerializeField] private Obstacle[] _obstaclePartPrefabs;

        [Header("Pool")]
        [SerializeField] private int _pairPrewarmCount = 4;
        [SerializeField] private int _partPrewarmCountPerPrefab = 4;

        [Header("Gap")]
        [SerializeField] private float _baseGapClearance = 2f;
        [SerializeField] private float _minGapClearance = 0.6f;
        [SerializeField] private float _gapClearanceDecreasePerSecond = 0.02f;
        [SerializeField] private float _verticalMargin = 0.2f;

        [Header("Spawn")]
        [SerializeField] private float _spawnInterval = 2.5f;

        [Header("Movement")]
        [SerializeField] private float _scrollSpeed = 3f;

        public ObstaclePair ObstaclePairPrefab => _obstaclePairPrefab;
        public IReadOnlyList<Obstacle> ObstaclePartPrefabs => _obstaclePartPrefabs;

        public int PairPrewarmCount => _pairPrewarmCount;
        public int PartPrewarmCountPerPrefab => _partPrewarmCountPerPrefab;

        public float BaseGapClearance => _baseGapClearance;
        public float MinGapClearance => _minGapClearance;
        public float GapClearanceDecreasePerSecond => _gapClearanceDecreasePerSecond;
        public float VerticalMargin => _verticalMargin;

        public float SpawnInterval => _spawnInterval;
        public float ScrollSpeed => _scrollSpeed;

        public Obstacle GetRandomPartPrefab()
        {
            if (_obstaclePartPrefabs == null || _obstaclePartPrefabs.Length == 0)
                return null;

            return _obstaclePartPrefabs[Random.Range(0, _obstaclePartPrefabs.Length)];
        }
    }
}