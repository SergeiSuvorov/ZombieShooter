﻿using Tools;
using UnityEngine;


namespace Controller
{
    public class InputController:BaseController
    {
        private InputView _view;
        private readonly ResourcePath _viewPath = new ResourcePath { PathResource = "Prefabs/GameInputView" };

        public InputController(SubscriptionProperty<Vector2> moveDiff, SubscriptionProperty<Vector2> rotateDiff, SubscriptionProperty<bool> isFire)
        {
            _view = LoadView();
            _view.Init(moveDiff, rotateDiff, isFire);
        }

        public InputView LoadView()
        {
            var objectView = Object.Instantiate(ResourceLoader.LoadPrefab(_viewPath));
            AddGameObjects(objectView);

            return objectView.GetComponent<InputView>();
        }
    }
}
