using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System;
using Tools;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Controller
{
    public class GameTimerController : BaseController, IUpdateable, IOnEventCallback
    {

        private readonly ResourcePath _viewPath = new ResourcePath { PathResource = PathLists.TimerUIView};
        private TimerUIView _view;
        private UpdateManager _updateManager;
        private float _currentGameTime;
        private float _gameTime = 40;
        private float _endGameTime=5.6f;
        private bool _readyToEnd;

        public Action GameEndTimerFinish;
        public Action MatchEndTimerFinish;
        public bool IsActive => throw new System.NotImplementedException();
        public GameTimerController( UpdateManager updateManager, Transform placeForUi)
        {
           
            _updateManager = updateManager;
            _view = LoadView(placeForUi);
            if(_currentGameTime<1f)
                _currentGameTime = _gameTime;

            Subscribes();
        }

        private TimerUIView LoadView(Transform placeForUi)
        {
            var objectView = Object.Instantiate(ResourceLoader.LoadPrefab(_viewPath), placeForUi, false);
            AddGameObjects(objectView);

            return objectView.GetComponent<TimerUIView>();
        }

        private void Subscribes()
        {
            PhotonNetwork.AddCallbackTarget(this);
            _updateManager.UpdateList.Add(this);
        }

        private void UnSubscribes()
        {
            PhotonNetwork.RemoveCallbackTarget(this);
            _updateManager.UpdateList.Remove(this);
        }

        public void UpdateExecute()
        {
           if (_currentGameTime > 0)
                _currentGameTime-=Time.deltaTime;
            else if(!_readyToEnd)
            {
                Debug.Log("End math" + _currentGameTime);
                MatchEndTimerFinish?.Invoke();
            }
            else if (_readyToEnd)
            {
                Debug.Log("End Game " + _currentGameTime);
                GameEndTimerFinish?.Invoke();
            }

            var gameTime = ((int)_currentGameTime).ToString();
            _view.ShowTime(gameTime);
        }

        public void StartEndGameCountdown()
        {
            _currentGameTime = _endGameTime;
            _readyToEnd = true;
            Debug.Log("_currentGameTime " + _currentGameTime);
            Debug.Log(" _readyToEnd " + _readyToEnd );
        }

        protected override void OnDispose()
        {
            UnSubscribes();
            base.OnDispose();
        }

        private void TimeSynhronizeEvent(float time)
        {
            object[] content = new object[] { time};
            RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
            PhotonNetwork.RaiseEvent(PhotonEvenCodeList.TimeSynhronizeCode, content, raiseEventOptions, SendOptions.SendReliable);
        }

        public void OnEvent(EventData photonEvent)
        {
            byte eventCode = photonEvent.Code;
            if (eventCode == PhotonEvenCodeList.PlayerRegisterCode)
            {
                if (!PhotonNetwork.IsMasterClient)
                    return;
                TimeSynhronizeEvent(_currentGameTime);
            }
            if (eventCode == PhotonEvenCodeList.TimeSynhronizeCode)
            {
                object[] data = (object[])photonEvent.CustomData;
                float synhronizeTime = (float)data[0];
                _currentGameTime=synhronizeTime;
            }
        }
    }
}



