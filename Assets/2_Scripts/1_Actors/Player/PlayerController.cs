using FlappyStar.Core;
using FlappyStar.Gameplay.InGame;
using LSH.Core;
using UnityEngine;
using UnityEngine.InputSystem;

namespace FlappyStar.Actors.Player
{
    public class PlayerController : MonoBehaviour
    {
        
        private PlayerMovement _movement;
        private InGameTimeManager _timeManager;
        private InputAction _jumpAction;

        public void Init(PlayerMovement movement, InGameTimeManager timeManager)
        {
            _movement = movement;
            _timeManager = timeManager;

            _jumpAction = InputManager.Instance.GetAction(InputActionName.Jump);

            _jumpAction.performed += HandleJumpPerformed;

        }

        private void HandleJumpPerformed(InputAction.CallbackContext context)
        {
            if (_timeManager.IsGamePaused) return;

            if (_movement.TryJump())
            {
                //sound and effect
            }
        }

        private void OnDestroy()
        {
            _jumpAction.performed -= HandleJumpPerformed;
        }
    }
}
