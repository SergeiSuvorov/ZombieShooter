using Tools;
using UnityEngine;

namespace Controller
{
    public class BasePlayerCharacterController: BaseController, IUpdateable, IFixUpdateable
    {
        protected CharacterView _view;
        protected IPhotonCharacterController _photonCharacterController;
        protected WeaponController _weaponController;

        protected readonly ResourcePath _viewPath = new ResourcePath { PathResource = ViewPathLists.WeaponView };
        public bool IsActive { get; set; }

        protected virtual void InitView(CharacterView view)
        {
            _view = view;
            _view.Init();
            AddGameObjects(_view.gameObject);
        }

        protected virtual WeaponView LoadWeaponView(Transform weaponRootTransform)
        {
            var objectView = Object.Instantiate(ResourceLoader.LoadPrefab(_viewPath), weaponRootTransform, false);
            AddGameObjects(objectView);
            return objectView.GetComponent<WeaponView>();
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

