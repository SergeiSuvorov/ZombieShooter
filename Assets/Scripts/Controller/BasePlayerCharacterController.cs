using Tools;


namespace Controller
{
    public class BasePlayerCharacterController: BaseController, IUpdateable
    {
        protected CharacterView _view;
        protected IPhotonCharacterController _photonCharacterController;

        public bool IsActive { get; set; }

        protected virtual void InitView(CharacterView view)
        {
            _view = view;
            _view.Init();
            AddGameObjects(_view.gameObject);
        }
        public virtual void UpdateExecute()
        {
        }
    }
}

