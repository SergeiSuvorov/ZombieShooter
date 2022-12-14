using Model;
using Pathfinding;
using System;
using Tools;
using UnityEngine;


namespace Controller
{
    public class StalkerAIController:BaseController
    {
        private readonly ResourcePath _modelPath = new ResourcePath { PathResource = PathLists.AIStalkerZombieModel };

        private readonly ZombieView _view;
        private readonly Seeker _seeker;
        private readonly AIZombieModelConfig _model;

        private Transform _target;
        private Path _path;
        private int _currentPointIndex;
        private float _timeTillNextRecalculate = 0;
        private const float _delay = 1;

        public bool IsActive;
        public Action needNewTarget; 

        public StalkerAIController(ZombieView view, Transform target)
        {
            _view = view != null ? view : throw new ArgumentNullException(nameof(view));
            _seeker = _view.Seeker;
            _view.Init();
            _target = target; /*!= null ? target : throw new ArgumentNullException(nameof(target));*/
            _model = LoadAIZombieModelConfig();
            IsActive = true;
        }

        private AIZombieModelConfig LoadAIZombieModelConfig()
        {
            var objectModel = UnityEngine.Object.Instantiate(ResourceLoader.LoadScriptable(_modelPath));

            return (objectModel as AIZombieModelConfig);
        }

        public void FixUpdateExecute()
        {
            if (!IsActive)
                return;

            Timer();
            var newVelocity = CalculateVelocity(_view.Transform.position);
           
            newVelocity *= Time.fixedDeltaTime;
            _view.Rigidbody.velocity = newVelocity;
        }

        public void ChangeTarget( Transform target)
        {
            _target = target;
            if (_target != null) 
            RecalculatePath();
        }
        private void Timer()
        {

            if (_timeTillNextRecalculate > 0)
            {
                _timeTillNextRecalculate -= Time.fixedDeltaTime;
            }
            else RecalculatePath();
        }

        public void RecalculatePath()
        {
            if (_seeker.IsDone())
            {
                if (_target != null)
                {
                    _seeker.StartPath(_view.Rigidbody.position, _target.position, OnPathComplete);
                    _timeTillNextRecalculate = _delay;
                }
                else
                {
                    needNewTarget?.Invoke();
                }
            }
        }

        private void OnPathComplete(Path p)
        {
            if (p.error) return;
            UpdatePath(p);
        }

        private void UpdatePath(Path p)
        {
            _path = p;
            _currentPointIndex = 0;
        }

        private Vector3 CalculateVelocity(Vector3 fromPosition)
        {
            if (_path == null|| _path.vectorPath == null) 
            {
                return Vector3.zero;
            }

            if (_currentPointIndex >= _path.vectorPath.Count) return Vector3.zero;

            var direction = ((Vector3)_path.vectorPath[_currentPointIndex] - fromPosition).normalized;
            var result = _model.Speed * direction;
            var sqrDistance = Vector2.SqrMagnitude((Vector3)_path.vectorPath[_currentPointIndex] - fromPosition);
            if (sqrDistance <= _model.MinSqrDistanceToTarget)
            {
                _currentPointIndex++;
            }
            return result;
        }

    }
}



