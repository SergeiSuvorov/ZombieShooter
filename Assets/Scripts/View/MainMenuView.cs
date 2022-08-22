using System;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuView : MonoBehaviour
{
    [SerializeField]
    private Button StartGameButton;

    [SerializeField]
    private FeedbackText _feedbackText;

    [SerializeField]
    private Text _xpCountText;

    [SerializeField]
    private Text _lastMatchXPCountText;

    public Action onStartButtonClick;

    private void Awake()
    {
        StartGameButton.onClick.AddListener(OnStartButtonClick);
    }

	private void OnStartButtonClick()
    {
        onStartButtonClick?.Invoke();
    }

    public void ShowXPInfo(int XP, int lastMatchXP)
    {
        _xpCountText.text = "XP:         " + XP.ToString();
        if (lastMatchXP > 0)
        {
            _lastMatchXPCountText.text = "LastMatchXP:" + lastMatchXP.ToString();
        }
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

