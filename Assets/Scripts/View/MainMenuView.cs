using System;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuView : MonoBehaviour
{
	public Button StartGameButton;

	public Action onStartButtonClick;

    private void Awake()
    {
        StartGameButton.onClick.AddListener(OnStartButtonClick);
    }

	private void OnStartButtonClick()
    {
        onStartButtonClick?.Invoke();
    }

    private void OnDestroy()
    {
        StartGameButton.onClick.RemoveAllListeners();
    }

}

