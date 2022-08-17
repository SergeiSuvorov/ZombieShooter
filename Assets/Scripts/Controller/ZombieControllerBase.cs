using System;
using Tools;


namespace Controller
{
    public class ZombieControllerBase : BaseController, IFixUpdateable
    {
        protected ZombieView _view;
        public ZombieView View => _view;

        public bool IsActive => throw new NotImplementedException();

        public void RemoveGameObjectFromList()
        {
            RemoveGameObjects(_view.gameObject);
        }

        public virtual void FixUpdateExecute()
        {
        }
    }
}



