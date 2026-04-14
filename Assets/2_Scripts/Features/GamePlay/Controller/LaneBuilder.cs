using System;
using System.Collections.Generic;
using UnityEngine;
using MyGame.Common.DataFormat;

namespace MyGame.Gameplay
{
    public class LaneBuilder : MonoBehaviour
    {
        [SerializeField] NotePoolManager _notePoolManager;
        [SerializeField] private NoteLane _noteLanePrefab;
        [SerializeField] private Transform _laneStartPoint;
        [SerializeField] private Transform _laneEndPoint;

        private float _totalLanesWidth;
        private float _laneHeight;
        private int _numOfLanes;

        private readonly List<NoteLane> _activeNoteLanes = new List<NoteLane>();

        public event Action<NoteLane[]> OnLaneSetupCompleted;
        public event Action<float> OnNoteWidthCalculated;

        public void Init(LevelData levelData)
        {
            SetParameters(levelData);

            ClearLanes();

            CreateLanes();

            OnLaneSetupCompleted?.Invoke(_activeNoteLanes.ToArray());
        }

        private void ClearLanes()
        {
            foreach (var lane in _activeNoteLanes)
            {
                if (lane != null)
                {
                    Destroy(lane.gameObject);
                }
            }
            _activeNoteLanes.Clear();
        }

        private void SetParameters(LevelData data)
        {
            _laneHeight = Camera.main.orthographicSize * 2f;

            _totalLanesWidth = _laneEndPoint.position.x - _laneStartPoint.position.x;

            _numOfLanes = data.numOfLanes;
        }

        private void CreateLanes()
        {
            float widthOfLane = _totalLanesWidth / _numOfLanes;
            OnNoteWidthCalculated?.Invoke(widthOfLane);
            float firstLaneX = _laneStartPoint.position.x + widthOfLane / 2;


            for (int i = 0; i < _numOfLanes; i++)
            {
                float xValue = firstLaneX + widthOfLane * i;

                Vector3 spawnPosition = new Vector3(xValue, 0f, 0f);

                NoteLane noteLaneInstance = Instantiate(_noteLanePrefab, spawnPosition, Quaternion.identity, transform);

                noteLaneInstance.Init(_notePoolManager, i, widthOfLane, _laneHeight);

                _activeNoteLanes.Add(noteLaneInstance);
            }
        }
    }
}

