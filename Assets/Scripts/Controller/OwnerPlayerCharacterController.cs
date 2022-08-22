using ExitGames.Client.Photon;
using Model;
using Photon.Pun;
using Photon.Realtime;
using System;
using Tools;
using UnityEngine;


namespace Controller
{
    public class OwnerPlayerCharacterController : BasePlayerCharacterController
    {
        private SubscriptionProperty<Vector2> _moveDiff;
        private SubscriptionProperty<Vector2> _rotateDiff;
        private SubscriptionProperty<bool> _isFire;
        private SubscriptionProperty<int> _currentHealth=new SubscriptionProperty<int>();
        private Transform _placeForUi;
        private PlayerUIController _playerUIController;
        private PlayerModel _playerModel;
        public Action onPlayerDied;

        public OwnerPlayerCharacterController(SubscriptionProperty<Vector2> moveDiff, SubscriptionProperty<Vector2> rotateDiff, SubscriptionProperty<bool> isFire, CharacterView view, PlayerModel playerModel)
        {
            InitView(view);
            _view.onGetDamage += GetDamage;
            PlayerRegisterEvent(_view);

            _weaponController = new WeaponController(view.WeaponTransformRoot, isFire, view.photonView.Owner);
            AddController(_weaponController);

            _isFire = isFire;
            _playerModel = playerModel;
            _photonCharacterController = new OwnerPhotonCharacterSynchronizeController(view, isFire);
            AddController(_photonCharacterController);

            _moveDiff = moveDiff;
            _moveDiff.SubscribeOnChange(Move);
            _rotateDiff = rotateDiff;
            _rotateDiff.SubscribeOnChange(Rotate);

            _currentHealth.Value = _playerModel.PlayerHealth;
        }

        public  void CreatePlayerUI(Transform placeForUi)
        {
            var currentAmmo = _weaponController.CurrentAmmoInClip;
            _playerUIController = new PlayerUIController(placeForUi, _currentHealth, currentAmmo);
            AddController(_playerUIController);
        }
        private void GetDamage(int damage, Player player)
        {
            _currentHealth.Value -= damage;
            if (_currentHealth.Value <= 0)
            {
                if (player != null)
                {
                    PlayerKillPlayerEvent(player, _view.photonView.Owner);
                }
                else
                    MonsterKillPlayerEvent(_view.photonView.Owner);

                 //after UI add
                 //onPlayerDied?.Invoke();
            }
        }

        protected virtual void Rotate(Vector2 rotateDiff)
        {
            _view.SetRotate(rotateDiff);
        }

        protected virtual void Move(Vector2 moveDif)
        {
            _view.SetMove(moveDif);
        }

        public override void UpdateExecute()
        {
            _view.UpdateExecute();
            _weaponController.Aiming();
            _weaponController.Timer();

            if (_isFire.Value)
                _weaponController.CheckReadyToAction();
        }

        public override void FixUpdateExecute()
        {
            _view.FixUpdateExecute();

            if(_view.transform.position.y<-10)
                onPlayerDied?.Invoke();
        }

        protected override void OnDispose()
        {
            base.OnDispose();

            _moveDiff.UnSubscriptionOnChange(Move);
            _rotateDiff.UnSubscriptionOnChange(Rotate);
        }

        private void PlayerRegisterEvent(CharacterView view)
        {
            object[] content = new object[] { view.photonView.Owner };
            RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
            PhotonNetwork.RaiseEvent(PhotonEvenCodeList.PlayerRegisterCode, content, raiseEventOptions, SendOptions.SendReliable);
        }

        private void PlayerKillPlayerEvent(Player killer, Player killed)
        {
            object[] content = new object[] { killer, killed };
            RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
            PhotonNetwork.RaiseEvent(PhotonEvenCodeList.PlayerKillsPlayerCode, content, raiseEventOptions, SendOptions.SendReliable);
        }
        private void MonsterKillPlayerEvent(Player player)
        {
            object[] content = new object[] { player };
            RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
            PhotonNetwork.RaiseEvent(PhotonEvenCodeList.MonsterKillsPlayerCode, content, raiseEventOptions, SendOptions.SendReliable);
        }
    }
}

