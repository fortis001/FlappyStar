using System;
using LSH.Utils;
using LSH.Utils.Pooling;
using UnityEngine;

namespace FlappyStar.Gameplay.Entities
{
    public class Obstacle : MonoBehaviour, IPoolable
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private GameObject[] _scoreLights;
        [SerializeField] private TriggerRelay2D _triggerRelay;

        public float HalfHeight => _spriteRenderer.bounds.extents.y;
        public int Score => _scoreLights.Length;

        public event Action OnPlayerTouched;

        public void OnCreated()
        {
            _triggerRelay.OnTriggerEntered += HandlePlayerTouched;
        }

        public void OnGet()
        {
            TurnOffLights();
        }

        public void OnRelease()
        {
            TurnOffLights();
        }

        public void TurnOnLights()
        {
            SetLights(true);
        }

        public void TurnOffLights()
        {
            SetLights(false);
        }

        private void SetLights(bool isOn)
        {
            if (_scoreLights == null)
                return;

            for (int i = 0; i < _scoreLights.Length; i++)
            {
                if (_scoreLights[i] != null)
                    _scoreLights[i].SetActive(isOn);
            }
        }

        private void HandlePlayerTouched(Collider2D collider)
        {
            OnPlayerTouched?.Invoke();
        }

        private void OnDestroy()
        {
            _triggerRelay.OnTriggerEntered -= HandlePlayerTouched;
        }
    }
}

