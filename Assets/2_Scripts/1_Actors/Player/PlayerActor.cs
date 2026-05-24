using FlappyStar.Actors.Player;
using FlappyStar.Gameplay.InGame;
using UnityEngine;

namespace FlappyStar.Actors
{
    public class PlayerActor : MonoBehaviour
    {
        [SerializeField] PlayerController _controller;
        [SerializeField] PlayerMovement _movement;
        [SerializeField] Collider2D _collider;
        public float PlayerHeight => _collider.bounds.size.y;

        public void Init(InGameTimeManager timeManager)
        {
            _controller.Init(_movement, timeManager);
        }

        public void FreezePhysics()
        {
            _movement.FreezePhysics();
        }

        public void RestorePhysics()
        {
            _movement.RestorePhysics();
        }

        public void Die()
        {
            _movement.ApplyDeathBounce();
        }
    }
}

