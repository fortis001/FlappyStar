using FlappyStar.Core;
using LSH.Core;
using TMPro;
using UnityEngine;

namespace FlappyStar.Gameplay.InGame
{
    public class InGameUIManager : MonoBehaviour
    {
        [SerializeField] GameObject _gameoverUI;
        [SerializeField] GameObject _pauseUI;
        [SerializeField] TextMeshProUGUI _scoreText;

        private ObstacleManager _obstacleManager;
        private int _currentScore;

        public void ShowGameOverUI() => _gameoverUI.SetActive(true);
        public void HideGameOverUI() => _gameoverUI.SetActive(false);

        public void ShowPauseUI() => _pauseUI.SetActive(true);
        public void HidePauseUI() => _pauseUI.SetActive(false);

        public void Init(ObstacleManager obstacleManager)
        {
            _obstacleManager = obstacleManager;

            _obstacleManager.OnPlayerPassed += HandlePlayerPassed;

            _scoreText.text = "0";
        }

        private void HandlePlayerPassed(int score)
        {
            _currentScore += score;

            _scoreText.text = _currentScore.ToString();
        }

        public void HandleRetryBtnClicked()
        {
            TransitionManager.Instance.LoadNextScene(SceneName.InGame);
        }

        public void HandleTitleBtnClicked()
        {
            TransitionManager.Instance.LoadNextScene(SceneName.Title);
        }
    }
}

