using System;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuView : MonoBehaviour
{
    [SerializeField]
    private Button StartGameButton;

    [SerializeField]
    private FeedbackText _feedbackText;

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

    public void LogFeedback(string message)
    {
        _feedbackText.LogFeedback(message);
    }
}

