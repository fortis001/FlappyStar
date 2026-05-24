using System.Collections;
using UnityEngine;

namespace FlappyStar.Presentation.StageProps
{
    public class CrowdFans : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private float _idleInterval = 0.4f;

        private CrowdSpriteSet _spriteSet;
        private Coroutine _idleRoutine;
        private bool _isGameOver = false;


        public void Activate(CrowdSpriteSet spriteSet)
        {
            _spriteSet = spriteSet;
            PlayIdle();
        }

        public void PlayIdle()
        {
            if (_spriteSet == null)
                return;

            _isGameOver = false;

            StopIdleRoutine();
            _idleRoutine = StartCoroutine(IdleRoutine());
        }

        public void PlayGameOver()
        {
            if (_spriteSet == null)
                return;

            _isGameOver = true;
            StopIdleRoutine();

            if (_spriteSet.GameOverSprite != null)
                _spriteRenderer.sprite = _spriteSet.GameOverSprite;
        }

        private IEnumerator IdleRoutine()
        {
            bool toggle = false;

            while (!_isGameOver)
            {
                _spriteRenderer.sprite = toggle
                    ? _spriteSet.IdleSpriteA
                    : _spriteSet.IdleSpriteB;

                toggle = !toggle;

                yield return new WaitForSecondsRealtime(_idleInterval);
            }
        }

        private void StopIdleRoutine()
        {
            if (_idleRoutine == null)
                return;

            StopCoroutine(_idleRoutine);
            _idleRoutine = null;
        }
    }
}

