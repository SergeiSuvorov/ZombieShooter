using System;
using UnityEngine;
using UnityEngine.UI;

public class CreateAccountWindowView : EnterInAccauntBaseView
{
	[SerializeField] private InputField _emailField;
	[SerializeField] private Button _createAccountButton;

	public Action onCreateAccountButtonClick;
	public Action<string> onEmailInputFieldUpdate;

	protected override void SubscriptionsElementsUI()
	{
		base.SubscriptionsElementsUI();
		_emailField.onValueChanged.AddListener(UpdateEmail);
		_createAccountButton.onClick.AddListener(CreateAccount);
	}

	private void CreateAccount()
	{
		onCreateAccountButtonClick?.Invoke();
	}

	private void UpdateEmail(string email)
	{
		onEmailInputFieldUpdate?.Invoke( email);
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
		_emailField.onValueChanged.RemoveAllListeners();
		_createAccountButton.onClick.RemoveAllListeners();
	}
}
