using FlappyStar.Core;
using LSH.Core;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class TitleSceneManager : MonoBehaviour
{

    private InputAction _startAction;
    private InputAction _exitAction;

    private void Start()
    {
        InputManager.Instance.SetActionMap(InputMapName.Title);
        _startAction = InputManager.Instance.GetAction(InputActionName.Start);
        _exitAction = InputManager.Instance.GetAction(InputActionName.Exit);

        SoundManager.Instance.PlayBGM(BGMID.Title);

        _startAction.performed += HandleStart;
        _exitAction.performed += HandleExit;
    }

    private void HandleStart(InputAction.CallbackContext context)
    {
        TransitionManager.Instance.LoadNextScene(SceneName.InGame);
    }

    private void HandleExit(InputAction.CallbackContext context)
    {
        Application.Quit();
    }

    private void OnDestroy()
    {
        if (InputManager.Instance == null) return;

        _startAction.performed -= HandleStart;
        _exitAction.performed -= HandleExit;
    }
}
