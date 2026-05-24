using System.Collections;
using FlappyStar.Actors;
using FlappyStar.Core;
using FlappyStar.Presentation.StageProps;
using LSH.Core;
using UnityEngine;
using UnityEngine.InputSystem;

namespace FlappyStar.Gameplay.InGame
{
    public class InGameSceneManager : MonoBehaviour
    {
        [SerializeField] private InGameTimeManager _timeManager;
        [SerializeField] private InGameUIManager _uiManager;
        [SerializeField] private PlayerActor _player;
        [SerializeField] private ObstacleManager _obstacleManager;
        [SerializeField] private PlayArea _playArea;
        [SerializeField] private StagePropManager _stagePropManager;

        private bool _isGameOver = false;
        private bool _isGamePaused = false;
        private float _gameoverDelayTime = 2f;

        private InputAction _pauseAction;

        public bool IsGameOver => _isGameOver;

        private void Start()
        {
            InputManager.Instance.SetActionMap(InputMapName.InGame);
            _pauseAction = InputManager.Instance.GetAction(InputActionName.Pause);

            SoundManager.Instance.PlayBGM(BGMID.InGame);



            _playArea.Init();
            _player.Init(_timeManager);
            _stagePropManager.Init(_timeManager, _playArea);
            _obstacleManager.Init(_player, _timeManager, _playArea);
            _uiManager.Init(_obstacleManager);

            _timeManager.Init();

            _pauseAction.performed += HandleESCPressed;
            _playArea.OnOutOfBounds += HandleGameOver;
            _obstacleManager.OnPlayerTouched += HandleGameOver;

        }

        private void HandleESCPressed(InputAction.CallbackContext context)
        {
            if(_isGamePaused)
            {
                _player.RestorePhysics();
                _timeManager.ResumeGame();

                _uiManager.HidePauseUI();

                _isGamePaused = false;
            }
            else
            {
                _player.FreezePhysics();
                _timeManager.PauseGame();

                _uiManager.ShowPauseUI();

                _isGamePaused = true;
            }
        }

        private void HandleGameOver()
        {
            if (_isGameOver)
                return;

            _isGameOver = true;
            StartCoroutine(GameOverSequence());
        }

        private IEnumerator GameOverSequence()
        {
            SoundManager.Instance.StopBGM();
            
            _timeManager.PauseGame();
            _player.Die();
            _stagePropManager.PlayGameOver();
            yield return new WaitForSecondsRealtime(_gameoverDelayTime);

            SoundManager.Instance.PlaySFX(SFXID.Gameplay_GameOver);
            _uiManager.ShowGameOverUI();
        }

        private void OnDestroy()
        {
            _pauseAction.performed -= HandleESCPressed;
            _playArea.OnOutOfBounds -= HandleGameOver;
            _obstacleManager.OnPlayerTouched -= HandleGameOver;
        }
    }
}

