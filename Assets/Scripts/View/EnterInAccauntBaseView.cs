using System;
using Tools;
using UnityEngine;
using UnityEngine.UI;

public class EnterInAccauntBaseView : MonoBehaviour, ICanvasActivatable
{
	[SerializeField]
	private InputField _usernameField;

	[SerializeField]
	private InputField _passwordField;

	[SerializeField]
	private Button _backButton;

	[SerializeField]
	private Canvas _formCanvas;

	public Action onBackButtonClick;
	public Action<string> onUserNameInputFieldUpdate;
	public Action<string> onPasswordInputFieldUpdate;



	private void Awake()
	{
		SubscriptionsElementsUI();
	}

	protected virtual void SubscriptionsElementsUI()
	{
		_usernameField.onValueChanged.AddListener(UpdateUsername);
		_passwordField.onValueChanged.AddListener(UpdatePassword);
		_backButton.onClick.AddListener(OnBackButtonClick);
	}

	private void OnBackButtonClick()
	{
		onBackButtonClick?.Invoke();
	}

	private void UpdatePassword(string password)
	{
		onPasswordInputFieldUpdate?.Invoke(password);
	}

	private void UpdateUsername(string username)
	{
		onUserNameInputFieldUpdate?.Invoke(username);
	}
	protected virtual void OnDestroy()
	{
		_usernameField.onValueChanged.RemoveAllListeners();
		_passwordField.onValueChanged.RemoveAllListeners();
		_backButton.onClick.RemoveAllListeners();
	}

	public void SetActive(bool isActive)
	{
		_formCanvas.enabled = isActive;
	}
}

