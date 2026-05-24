using System.Collections.Generic;
using FlappyStar.Gameplay.InGame;
using LSH.Utils.Pooling;
using UnityEngine;


namespace FlappyStar.Presentation.StageProps
{
    public class StagePropManager : MonoBehaviour
    {
        [SerializeField] private StagePropSettings _settings;
        [SerializeField] private Transform _crowdSpawnPoint;
        [SerializeField] private Transform _pamphletSpawnPoint;

        private InGameTimeManager _timeManager;
        private PlayArea _playArea;

        private float _crowdSpawnTimer;
        private float _pamphletSpawnTimer;
        private float _scrollSpeed;

        private SinglePrefabPool<CrowdFans> _crowdPool;
        private readonly List<CrowdFans> _activeCrowd = new();
        private SinglePrefabPool<SpriteRenderer> _pamphletPool;
        private readonly List<SpriteRenderer> _activePamphlet = new();

        public void Init(InGameTimeManager timeManager, PlayArea playArea)
        {
            _timeManager = timeManager;
            _playArea = playArea;

            _crowdPool = new SinglePrefabPool<CrowdFans>(_settings.CrowdFansPrefab, transform);
            _pamphletPool = new SinglePrefabPool<SpriteRenderer>(_settings.PamphletPrefab, transform);

            _crowdPool.Prewarm(_settings.CrowdFansPrewarmCount);
            _pamphletPool.Prewarm(_settings.PamphletPrewarmCount);

            _scrollSpeed = _settings.ScrollSpeed;

            _crowdSpawnTimer = 0f;
            _pamphletSpawnTimer = 0f;

            SetupProps();
        }

        private void SetupProps() 
        { 
            for (int i = 0; i < _settings.SetupPropsCount; i++) 
            { 
                CrowdFans crowd = SpawnCrowd();
                Move(crowd.transform, _settings.CrowdFansSpawnInterval * i);
            } 
        }
        private void Update()
        {
            if (_timeManager == null)
                return;

            if (_timeManager.IsGamePaused) return;

            float deltaTime = _timeManager.GameDeltaTime;

            UpdateCrowdSpawn(deltaTime);
            UpdatePamphletSpawn(deltaTime);
            UpdateMovement(deltaTime);
        }

        private void UpdateCrowdSpawn(float deltaTime)
        {
            _crowdSpawnTimer += deltaTime;

            if (_crowdSpawnTimer < _settings.CrowdFansSpawnInterval)
                return;

            _crowdSpawnTimer = 0f;
            SpawnCrowd();
        }

        private CrowdFans SpawnCrowd()
        {
            CrowdFans crowd = _crowdPool.Get();
            CrowdSpriteSet spriteSet = _settings.GetRandomCrowdSpriteSet();

            crowd.Activate(spriteSet);
            crowd.transform.position = _crowdSpawnPoint.position;

            _activeCrowd.Add(crowd);

            return crowd;
        }

        private void UpdatePamphletSpawn(float deltaTime)
        {
            _pamphletSpawnTimer += deltaTime;

            if (_pamphletSpawnTimer < _settings.PamphletSpawnInterval)
                return;

            _pamphletSpawnTimer = 0f;
            SpawnPamphlet();
        }

        private void SpawnPamphlet()
        {
            SpriteRenderer pamphlet = _pamphletPool.Get();
            Sprite sprite = _settings.GetRandomPamphletSprite();

            pamphlet.sprite = sprite;
            pamphlet.transform.position = _pamphletSpawnPoint.position;

            _activePamphlet.Add(pamphlet);
        }

        private void UpdateMovement(float deltaTime)
        {
            for (int i = _activeCrowd.Count - 1; i >= 0; i--)
            {
                CrowdFans crowd = _activeCrowd[i];

                Move(crowd.transform, deltaTime);

                if (crowd.transform.position.x > _playArea.DespawnX)
                    continue;

                _activeCrowd.RemoveAt(i);
                _crowdPool.Release(crowd);
            }

            for (int i = _activePamphlet.Count - 1; i >= 0; i--)
            {
                SpriteRenderer pamphlet = _activePamphlet[i];

                Move(pamphlet.transform, deltaTime);

                if (pamphlet.transform.position.x > _playArea.DespawnX)
                    continue;

                _activePamphlet.RemoveAt(i);
                _pamphletPool.Release(pamphlet);
            }

        }

        public void PlayGameOver()
        {
            foreach(CrowdFans crowd in _activeCrowd)
            {
                crowd.PlayGameOver();
            }
        }

        private void Move(Transform transform, float deltaTime)
        {
            transform.position += Vector3.left * _scrollSpeed * deltaTime;
        }
    }
}

