using TMPro;
using UnityEngine;

public class PlayerUIView : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _hpText;
    [SerializeField]
    private TMP_Text _currentAmmoText;

    public void ShowAmmoText(string currentAmmo)
    {
        _currentAmmoText.text = currentAmmo;
    }

    public void ShowHealthText(string currentHealth)
    {
        currentHealth = "HP: " + currentHealth;
        _hpText.text = currentHealth;
    }
}


