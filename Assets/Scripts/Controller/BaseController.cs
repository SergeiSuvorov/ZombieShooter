using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;


namespace Controller
{
    public class BaseController : IDisposable
    {
        private List<BaseController> _baseControllers = new List<BaseController>();
        private List<GameObject> _gameObjects = new List<GameObject>();
        private bool _isDisposed;

        public void Dispose()
        {
            if (_isDisposed)
                return;

            _isDisposed = true;

            foreach (var baseController in _baseControllers)
                baseController?.Dispose();

            _baseControllers.Clear();

            foreach (var cachedGameObject in _gameObjects)
                Object.Destroy(cachedGameObject);

            _gameObjects.Clear();

            OnDispose();
        }

        protected void AddController(BaseController baseController)
        {
            _baseControllers.Add(baseController);
        }

        protected void RemoveController(BaseController baseController)
        {
            _baseControllers.Remove(baseController);
        }

        protected void AddGameObjects(GameObject gameObject)
        {
            _gameObjects.Add(gameObject);
        }

        protected void RemoveGameObjects(GameObject gameObject)
        {
            _gameObjects.Remove(gameObject);
        }

        protected virtual void OnDispose()
        {
        }
    }
}

