using System;
using UnityEngine;
using UnityEngine.UI;

public class SignInWindowView : EnterInAccauntBaseView
{
	[SerializeField] private Button _signInButton;

	public Action onSignInButtonClick;
	protected override void SubscriptionsElementsUI()
	{
		base.SubscriptionsElementsUI();
		_signInButton.onClick.AddListener(SignIn);
	}

	private void SignIn()
	{
		onSignInButtonClick?.Invoke();
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
		_signInButton.onClick.RemoveAllListeners();
	}
}
