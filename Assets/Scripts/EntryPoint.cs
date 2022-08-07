using Controller;
using Model;
using UnityEngine;

public class EntryPoint : MonoBehaviour
{
    [SerializeField]
    private Transform _placeForUi;
    [SerializeField]
    private UpdateManager _updateManager;

    private MainController _mainController;

    private void Awake()
    {
        var profilePlayer = new ProfilePlayer();
        profilePlayer.CurrentState.Value = GameState.Start;
        _mainController = new MainController(_placeForUi, profilePlayer, _updateManager);
    }

    protected void OnDestroy()
    {
        _mainController?.Dispose();
    }
}

