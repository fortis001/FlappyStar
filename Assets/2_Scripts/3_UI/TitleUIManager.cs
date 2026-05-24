using FlappyStar.Core;
using LSH.Core;
using UnityEngine;

public class TitleUIManager : MonoBehaviour
{
    public void HandleStartBtnClicked()
    {
        TransitionManager.Instance.LoadNextScene(SceneName.InGame);
    }
}
