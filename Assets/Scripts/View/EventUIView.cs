using TMPro;
using UnityEngine;

public class EventUIView: MonoBehaviour
{
    [SerializeField]
    private TMP_Text _eventText;

    public void ShowMessage(string message, Color color)
    {
        if (_eventText == null)
            return;

        _eventText.color = color;
        _eventText.text = message;
       
    }

    public void Clear()
    {
        if (_eventText == null)
            return;
        _eventText.text="";
    }
}


