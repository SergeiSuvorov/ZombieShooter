using Tools;
using UnityEngine;

namespace Controller
{
    public class BasePlayerCharacterController: BaseController, IUpdateable, IFixUpdateable
    {
        protected CharacterView _view;
        protected BasePhotonPlayerCharacterController _photonCharacterController;
        protected WeaponController _weaponController;

        public bool IsActive { get; set; }

        protected virtual void InitView(CharacterView view)
        {
            _view = view;
            _view.Init();
            AddGameObjects(_view.gameObject);
        }
        protected virtual void ExecuteWeaponAction()
        {
        }
        public virtual void UpdateExecute()
        {
        }
        public virtual void FixUpdateExecute()
        {
        }
    }
}

