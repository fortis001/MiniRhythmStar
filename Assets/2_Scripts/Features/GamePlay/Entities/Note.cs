using System;
using MyGame.Core.Managers;
using TMPro;
using UnityEngine;


namespace MyGame.Gameplay
{
    public class Note : MonoBehaviour
    {
        [SerializeField] SpriteRenderer _sr;
        [SerializeField] TextMeshPro _laneText;

        private Vector3 _spawnPoint;
        private Vector3 _hitPoint;
        private float _hitTime;
        private float _noteTravelTime;

        public event Action<Note> OnKilled;

        public void Setup(float noteTravelTime, float noteWidth, float noteHeight)
        {
            _noteTravelTime = noteTravelTime;
            _sr.size = new Vector2(noteWidth, noteHeight);
        }

        public void Init(Vector3 spawnPoint, Vector3 hitPoint, float hitTime, int laneNumber)
        {
            _spawnPoint = spawnPoint;
            _hitPoint = hitPoint;
            _hitTime = hitTime;
            _laneText.text = (laneNumber + 1).ToString();

            transform.position = spawnPoint;
            gameObject.SetActive(true);
        }

        void Update()
        {
            UpdatePosition();

        }

        private void UpdatePosition()
        {
            float currentTime = GameTimer.Instance.CurrentTime;

            float startTime = _hitTime - _noteTravelTime;
            float progress = (currentTime - startTime) / _noteTravelTime;



            transform.position = Vector3.LerpUnclamped(_spawnPoint, _hitPoint, progress);
        }

        public void Kill()
        {
            OnKilled?.Invoke(this);
        }


    }
}

