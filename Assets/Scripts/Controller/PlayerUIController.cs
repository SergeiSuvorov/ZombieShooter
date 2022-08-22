using Tools;
using UnityEngine;


namespace Controller
{
    public class PlayerUIController: BaseController
    {
        private readonly ResourcePath _viewPath = new ResourcePath { PathResource = ViewPathLists.PlayerUIView };
        private Transform _placeForUi;
        private SubscriptionProperty<int> _currentHealth;
        private SubscriptionProperty<int> _currentClipAmmo;
        private PlayerUIView _view;

        public PlayerUIController(Transform placeForUi, SubscriptionProperty<int> currentHealth, SubscriptionProperty<int> currentClipAmmo)
        {
            _placeForUi = placeForUi;
            _view = LoadView(_placeForUi);

            _currentHealth = currentHealth;
            _currentClipAmmo = currentClipAmmo;

            ShowHealthChange(_currentHealth.Value);
            ShowAmmoChange(_currentClipAmmo.Value);

            Subscribe();
        }

        private void Subscribe()
        {
            _currentHealth.SubscribeOnChange(ShowHealthChange);
            _currentClipAmmo.SubscribeOnChange(ShowAmmoChange);
        }

        private void UnSubscribe()
        {
            _currentHealth.UnSubscriptionOnChange(ShowHealthChange);
            _currentClipAmmo.UnSubscriptionOnChange(ShowAmmoChange);
        }
        private PlayerUIView LoadView(Transform placeForUi)
        {
            var objectView = Object.Instantiate(ResourceLoader.LoadPrefab(_viewPath), placeForUi, false);
            AddGameObjects(objectView);

            return objectView.GetComponent<PlayerUIView>();
        }

        private void ShowHealthChange(int currentHealth)
        {
            _view.ShowHealthText(currentHealth.ToString());
        }

        private void ShowAmmoChange(int currentAmmo)
        {
            var ammoText = currentAmmo + "/--";
            _view.ShowAmmoText(ammoText);
        }

        protected override void OnDispose()
        {
            UnSubscribe();
            base.OnDispose();
        }
    }

}

