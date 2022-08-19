using Tools;

namespace Controller
{
    public class RemotePlayerCharacterController : BasePlayerCharacterController
    {
        private SubscriptionProperty<bool> _isFire = new SubscriptionProperty<bool>();
        public RemotePlayerCharacterController(CharacterView view)
        {
            InitView(view);

            _weaponController = new WeaponController(view.WeaponTransformRoot, _isFire);
            AddController(_weaponController);

            _photonCharacterController = new RemotePhotonCharacterSynchronizeController(view, _isFire);
            AddController(_photonCharacterController);
        }

        public override void UpdateExecute()
        {
            _weaponController.Timer();
            if (_isFire.Value)
                _weaponController.CheckReadyToAction();
        }

        public override void FixUpdateExecute()
        {
            _photonCharacterController.SynhronizeExecute();
        }
    }
}

