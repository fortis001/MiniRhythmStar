using System.Collections;
using System.Collections.Generic;
using MyGame.Core.Managers;
using MyGame.Core.RuntimeData;
using UnityEngine;

namespace MyGame.Gameplay
{
    public class NoteScheduler : MonoBehaviour
    {
        [SerializeField] LaneBuilder _laneBuilder;

        private float _noteTravelTime;
        private NoteLane[] _activeNoteLanes;

        private Queue<RuntimeNote> _noteQueue = new();
        private RuntimeNote _nextNote;
        private float _chartOffset;
        private bool _hasNext;



        private void Awake()
        {
            _laneBuilder.OnLaneSetupCompleted += OnLanesReady;
        }

        public void Init(Queue<RuntimeNote> noteQueue, float chartOffset)
        {
            _noteQueue.Clear();
            _noteQueue = noteQueue;
            _chartOffset = chartOffset;

            FetchNext();
        }

        public void OnLanesReady(NoteLane[] activeNoteLanes)
        {
            if (GameManager.Instance != null)
            {
                _noteTravelTime = GameManager.Instance.NoteTravelTime;
            }
            _activeNoteLanes = activeNoteLanes;    
        }


        void Update()
        {
            if (_activeNoteLanes == null || !_hasNext)
                return;

            if (GameTimer.Instance.CurrentTime >= _nextNote.Time - _noteTravelTime)
            {
                Spawn(_nextNote);
                FetchNext();
            }

        }

        private void FetchNext()
        {
            _hasNext = _noteQueue.Count > 0;
            if (_hasNext)
                _nextNote = _noteQueue.Dequeue();
            else
                StartCoroutine(EndGameAfterDelay());
        }

        private IEnumerator EndGameAfterDelay()
        {
            yield return new WaitForSeconds(_noteTravelTime + 2f);
            GameManager.Instance.EndGamePlay();
        }

        private void Spawn(RuntimeNote info)
        {
            _activeNoteLanes[info.Lane].SpawnNote(info.Time);
        }

        private void OnDestroy()
        {
            _laneBuilder.OnLaneSetupCompleted -= OnLanesReady;
        }
    }
}


