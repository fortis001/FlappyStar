using System.Collections.Generic;
using UnityEngine;

namespace FlappyStar.Presentation.StageProps
{
    [System.Serializable]
    public class CrowdSpriteSet
    {
        [SerializeField] private Sprite _idleSpriteA;
        [SerializeField] private Sprite _idleSpriteB;
        [SerializeField] private Sprite _gameOverSprite;

        public Sprite IdleSpriteA => _idleSpriteA;
        public Sprite IdleSpriteB => _idleSpriteB;
        public Sprite GameOverSprite => _gameOverSprite;
    }

    [CreateAssetMenu(
        fileName = "StagePropSettings",
        menuName = "FlappyStar/Stage Prop Settings")]
    public class StagePropSettings : ScriptableObject
    {
        [Header("CrowdFans")]
        [SerializeField] private CrowdFans _crowdFansPrefab;
        [SerializeField] private CrowdSpriteSet[] _crowdSpriteSets;

        [Header("Pamphlet")]
        [SerializeField] private SpriteRenderer _pamphletPrefab;
        [SerializeField] private Sprite[] _pamphletSprites;

        [Header("Pool")]
        [SerializeField] private int _crowdFansPrewarmCount = 8;
        [SerializeField] private int _pamphletPrewarmCount = 4;

        [Header("Spawn")]
        [SerializeField] private float _crowdFansSpawnInterval = 1.2f;
        [SerializeField] private float _pamphletSpawnInterval = 2.0f;
        [SerializeField] private int _setupPropsCount = 4;

        [Header("Movement")]
        [SerializeField] private float _scrollSpeed = 2.5f;

        public CrowdFans CrowdFansPrefab => _crowdFansPrefab;
        public SpriteRenderer PamphletPrefab => _pamphletPrefab;
        public IReadOnlyList<CrowdSpriteSet> CrowdSpriteSets => _crowdSpriteSets;
        public int CrowdFansPrewarmCount => _crowdFansPrewarmCount;
        public int PamphletPrewarmCount => _pamphletPrewarmCount;

        public float CrowdFansSpawnInterval => _crowdFansSpawnInterval;
        public float PamphletSpawnInterval => _pamphletSpawnInterval;
        public int SetupPropsCount => _setupPropsCount;
        public float ScrollSpeed => _scrollSpeed;

        public CrowdSpriteSet GetRandomCrowdSpriteSet()
        {
            if (_crowdSpriteSets == null || _crowdSpriteSets.Length == 0)
                return null;

            return _crowdSpriteSets[Random.Range(0, _crowdSpriteSets.Length)];
        }

        public Sprite GetRandomPamphletSprite()
        {
            if (_pamphletSprites == null || _pamphletSprites.Length == 0)
                return null;

            return _pamphletSprites[Random.Range(0, _pamphletSprites.Length)];
        }
    }
}