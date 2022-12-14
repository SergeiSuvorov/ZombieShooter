using Model;
using Photon.Realtime;
using Tools;
using UnityEngine;

namespace Controller
{
    public class WeaponController: BaseController
    {
        private WeaponView _view;
        private WeaponModel _model;
        private SubscriptionProperty<bool> _isFire = new SubscriptionProperty<bool>();
        private float _currentTime;
        private SubscriptionProperty<int> _currentAmmoInClip = new SubscriptionProperty<int>();

        private readonly ResourcePath _viewPath = new ResourcePath { PathResource = PathLists.WeaponView };
        private readonly ResourcePath _modelPath = new ResourcePath { PathResource = PathLists.WeaponModel };

        private bool _isReloading;
        public SubscriptionProperty<int> CurrentAmmoInClip => _currentAmmoInClip;

        public WeaponController(Transform weaponRootTransform,SubscriptionProperty<bool> isFire, Player ownerPlayer)
        {
            _model = LoadWeaponModel();
            _view = LoadWeaponView(weaponRootTransform);
            _view.SetOwner(ownerPlayer);
            _currentAmmoInClip.Value= _model.AmmoClipSize;
            _isFire = isFire;
        }

        private WeaponView LoadWeaponView(Transform weaponRootTransform)
        {
            var objectView = Object.Instantiate(ResourceLoader.LoadPrefab(_viewPath), weaponRootTransform, false);
            AddGameObjects(objectView);
            return objectView.GetComponent<WeaponView>();
        }

        private WeaponModel LoadWeaponModel()
        {
            var objectModel = Object.Instantiate(ResourceLoader.LoadScriptable(_modelPath));

            return (objectModel as WeaponModel);
        }

        private void WeaponAction()
        {
            _currentAmmoInClip.Value--;
            _view.Action(_model.Damage);
            _view.SetActiveVisualEffect(_isFire.Value);
            _currentTime = _model.ShootDelayTime;

            if (_currentAmmoInClip.Value <= 0)
                BeginReload();
        }

        public void BeginReload()
        {
            _isReloading = true;
            _currentTime = _model.ReloadTime;
            Debug.Log("BeginReload()");
        }

        private void EndReload()
        {
            _isReloading = false;
            _currentAmmoInClip.Value = _model.AmmoClipSize;
            Debug.Log("EndReload()");
        }

        public void CheckReadyToAction()
        {
            if (_currentTime > 0 || _isReloading)
                return;
            WeaponAction();
        }

        public void Timer()
        {
            if (_currentTime > 0)
            {
                _view.SetActiveVisualEffect(false);
                _currentTime -= Time.deltaTime;
            }
            else if (_isReloading)
                EndReload();
        }

        internal void Aiming()
        {
            _view.RedDotAiming();
        }
    }
}