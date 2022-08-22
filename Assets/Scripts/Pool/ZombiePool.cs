
using Controller;
using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;

namespace Pools
{
    public class ZombiePool
    {

        private readonly List<ZombieControllerBase> _objectList = new List<ZombieControllerBase>();

        private int _createObjectIndex;
        public int CreateObjectIndex { get { return _createObjectIndex; } }
        public ZombiePool()
        {

        }

        public void ReturnToPool(ZombieControllerBase controller)
        {
            AddToPoolList(controller);
        }

        public ZombieControllerBase GetFromPool()
        {
            ZombieControllerBase controller = RemoveFromPoolList();
            return controller;
        }

        private void AddToPoolList(ZombieControllerBase controller)
        {
            if (!_objectList.Contains(controller))
                _objectList.Add(controller);
            Debug.Log($"Add to pull {_objectList.Count}");
        }

        private ZombieControllerBase RemoveFromPoolList()
        {
            ZombieControllerBase controller = null;
            if (_objectList.Count == 0)
            {
                if (PhotonNetwork.IsMasterClient)
                {
                    controller = new MasterClientZombiController();
                }
            }
            else
            {
                Debug.Log($"Get From pull {_objectList.Count}");
                controller = _objectList[0];
                _objectList.Remove(controller);
            }

            return controller;
        }
    }
}
