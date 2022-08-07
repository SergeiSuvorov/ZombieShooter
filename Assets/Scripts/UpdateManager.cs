using System.Collections.Generic;
using Tools;
using UnityEngine;

public class UpdateManager : MonoBehaviour
{
    private List<IUpdateable> _updateList = new List<IUpdateable>();
    private List<ILateUpdateable> _lateUpdateList = new List<ILateUpdateable>();
    private List<IFixUpdateable> _fixUpdateList = new List<IFixUpdateable>();

    public List<IUpdateable> UpdateList => _updateList;
    public List<ILateUpdateable> LateUpdateList => _lateUpdateList;
    public List<IFixUpdateable> FixUpdateList => _fixUpdateList;

    private void Update()
    {
        for(int i=0; i<_updateList.Count; i++)
        {
            _updateList[i].UpdateExecute();
        }
    }

    private void LateUpdate()
    {
        for (int i = 0; i < _lateUpdateList.Count; i++)
        {
            _lateUpdateList[i].LateUpdateExecute();
        }
    }

    private void FixedUpdate()
    {
        for (int i = 0; i < _fixUpdateList.Count; i++)
        {
            _fixUpdateList[i].FixUpdateExecute();
        }
    }
}

