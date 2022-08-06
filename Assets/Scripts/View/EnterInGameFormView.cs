using System;
using Tools;
using UnityEngine;
using UnityEngine.UI;

public class EnterInGameFormView : MonoBehaviour, ICanvasActivatable
{
	public Canvas FormCanvas;

	[SerializeField]
	private Button _signInButton;

	[SerializeField]
	private Button _createAccountButton;

	public Action onSignInButtonClick;
	public Action onCreateAccountButtonClick;

	private void Awake()
	{
		_signInButton.onClick.AddListener(OnSignInButtonClick);
		_createAccountButton.onClick.AddListener(OnCreateAccauntButtonClick);
	}

    private void OnCreateAccauntButtonClick()
    {
		onCreateAccountButtonClick?.Invoke();
	}

    private void OnSignInButtonClick()
    {
		onSignInButtonClick?.Invoke();
	}

	private void OnDestroy()
	{
		_signInButton.onClick.RemoveAllListeners();
		_createAccountButton.onClick.RemoveAllListeners();
	}

	public void SetActive(bool isActive)
	{
		FormCanvas.enabled = isActive;
	}
}
