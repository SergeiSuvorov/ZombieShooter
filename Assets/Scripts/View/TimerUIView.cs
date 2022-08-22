using System;
using TMPro;
using UnityEngine;

public class TimerUIView : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _timeText;

    public Action<float> onTimeUpdate;
    public Action  onNeedTimeUpdate;

    public void ShowTime(string time)
    {
        if (_timeText == null)
            return;
        _timeText.text = time;
    }

}



