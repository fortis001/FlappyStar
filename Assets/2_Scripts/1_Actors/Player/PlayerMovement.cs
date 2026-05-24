using System.Collections;
using FlappyStar.Core;
using LSH.Core;
using UnityEngine;

namespace FlappyStar.Actors.Player
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] Rigidbody2D _rigidbody;
        [SerializeField] float _jumpForce = 5f;
        [SerializeField] float _deathBounceDelay = 1f;
        [SerializeField] float _deathBounceForce = 15f;


        private Vector2 _cachedVelocity;
        private float _cachedGravityScale;


        public bool TryJump()
        {
            _rigidbody.linearVelocity = new Vector2(_rigidbody.linearVelocity.x, 0f);

            _rigidbody.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);

            SoundManager.Instance.PlaySFX(SFXID.Player_Jump);

            return true;
        }

        public void ApplyDeathBounce()
        {
            StartCoroutine(DeathBounce());
        }

        private IEnumerator DeathBounce()
        {
            FreezePhysics();

            yield return new WaitForSecondsRealtime(_deathBounceDelay);

            RestorePhysics();

            _rigidbody.linearVelocity = Vector2.zero;
            _rigidbody.AddForce(Vector2.up * _deathBounceForce, ForceMode2D.Impulse);

        }

        public void FreezePhysics()
        {
            _cachedVelocity = _rigidbody.linearVelocity;
            _cachedGravityScale = _rigidbody.gravityScale;

            _rigidbody.linearVelocity = Vector2.zero;
            _rigidbody.gravityScale = 0f;
        }

        public void RestorePhysics()
        {
            _rigidbody.gravityScale = _cachedGravityScale;
            _rigidbody.linearVelocity = _cachedVelocity;
        }

    }
}

