using TMPro;
using UnityEngine;

public class ResultTableView : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _resultText;

    public void ShowResult(string Result)
    {
        _resultText.text = Result;
    }
}



