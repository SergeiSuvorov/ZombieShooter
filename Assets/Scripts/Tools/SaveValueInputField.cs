using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(InputField))]
public class SaveValueInputField : MonoBehaviour
{
    [SerializeField]
	private string _savePrefKey = "default" ;

    [SerializeField]
    private InputField _inputField;



    void Start()
	{

		string defaultName = string.Empty;

		if (_inputField != null)
		{
			if (PlayerPrefs.HasKey(_savePrefKey))
			{
				defaultName = PlayerPrefs.GetString(_savePrefKey);
				_inputField.text = defaultName;
			}
            _inputField.onValueChanged.AddListener(SetNewValue);
		}
	}

	private void SetNewValue(string value)
	{

		if (string.IsNullOrEmpty(value))
		{
			Debug.LogError("Player Name is null or empty");
			return;
		}

		PlayerPrefs.SetString(_savePrefKey, value);
	}

    private void OnDestroy()
    {
        _inputField.onValueChanged.RemoveAllListeners();
    }
}

