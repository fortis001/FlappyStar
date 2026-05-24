using System;
using FlappyStar.Core;
using LSH.Core;
using UnityEngine;

namespace FlappyStar.Gameplay.InGame
{
    public class InGameTimeManager : MonoBehaviour
    {
        public bool IsGamePaused => TimeManager.Instance.IsPaused(TimeChannelName.Game);
        public float GameDeltaTime => TimeManager.Instance.GetDeltaTime(TimeChannelName.Game);
        public float GameTime => TimeManager.Instance.GetTime(TimeChannelName.Game);

        public void PauseGame() => TimeManager.Instance.Pause(TimeChannelName.Game);
        public void ResumeGame() => TimeManager.Instance.Resume(TimeChannelName.Game);


        public event Action OnGamePaused;
        public event Action OnGameResume;

        public void Init()
        {
            TimeManager.Instance.OnPaused += HandlePaused;
            TimeManager.Instance.OnResumed += HandleResume;


            TimeManager.Instance.ResetAndResume(TimeChannelName.Game);
        }

        private void HandlePaused(TimeChannelReference channel)
        {
            if (channel == TimeChannelName.Game) OnGamePaused?.Invoke();
        }

        private void HandleResume(TimeChannelReference channel)
        {
            if (channel == TimeChannelName.Game) OnGameResume?.Invoke();
        }

        private void OnDestroy()
        {
            if(TimeManager.Instance == null) return;

            TimeManager.Instance.OnPaused -= HandlePaused;
            TimeManager.Instance.OnResumed -= HandleResume;
        }
    }
}

