using Tools;

namespace Controller
{
    public class RemotePlayerCharacterController : BasePlayerCharacterController
    {
        private SubscriptionProperty<bool> _isFire = new SubscriptionProperty<bool>();
        public RemotePlayerCharacterController(CharacterView view)
        {
            InitView(view);

            var weaponView = LoadWeaponView(view.WeaponTransformRoot);
            _weaponController = new WeaponController(weaponView, _isFire);
            AddController(_weaponController);

            _photonCharacterController = new RemotePhotonCharacterSynchronizeController(view, _isFire);
        }

        public override void UpdateExecute()
        {
            _photonCharacterController.SynhronizeExecute();

            if (_isFire.Value)
                _weaponController.CheckReadyToAction();
        }
    }
}

