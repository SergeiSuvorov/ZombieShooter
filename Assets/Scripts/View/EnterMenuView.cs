using UnityEngine;


public class EnterMenuView : MonoBehaviour
{
	[SerializeField]
	private CreateAccountWindowView _createAccountWindow;

	[SerializeField]
	private EnterInGameFormView _enterInGameView;

	[SerializeField]
	private SignInWindowView _signInWindow;

	[SerializeField]
	private FeedbackText _feedbackLog;

	public CreateAccountWindowView CreateAccountWindow => _createAccountWindow;

	public EnterInGameFormView EnterInGameView => _enterInGameView;

	public SignInWindowView SignInWindow=> _signInWindow;

	public FeedbackText FeedbackLog=> _feedbackLog;
}

