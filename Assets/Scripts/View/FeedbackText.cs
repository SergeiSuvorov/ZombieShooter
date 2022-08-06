using UnityEngine;
using UnityEngine.UI;

public class FeedbackText: MonoBehaviour
{
	[Tooltip("The Ui Text to inform the user about the connection progress")]
	[SerializeField]
	private Text _feedbackText;

    public void LogFeedback(string message)
	{
		if (_feedbackText == null)
		{
			return;
		}
		_feedbackText.text = message;
	}

	public void Clear()
	{
		if (_feedbackText == null)
		{
			return;
		}
		_feedbackText.text = string.Empty;
	}

}
