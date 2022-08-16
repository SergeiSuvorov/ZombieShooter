using Tools;

namespace Controller
{
    public class WeaponController: BaseController
    {
        private WeaponView _weaponView;
        private SubscriptionProperty<bool> _isFire = new SubscriptionProperty<bool>();

        public WeaponController(WeaponView weaponView, SubscriptionProperty<bool> isFire)
        {
            _weaponView = weaponView;
            _isFire = isFire;
            _isFire.SubscribeOnChange(_weaponView.SetActiveVisualEffect);
        }
        private void WeaponAction()
        {
            _weaponView.SetActiveVisualEffect(_isFire.Value);
            _weaponView.Action();
        }

        public void CheckReadyToAction()
        {
            WeaponAction();
        }

        internal void Aiming()
        {
            _weaponView.RedDotAiming();
        }
    }
}