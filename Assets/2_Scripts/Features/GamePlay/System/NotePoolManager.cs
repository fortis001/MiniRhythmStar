using System.Collections.Generic;
using MyGame.Core.Managers;
using UnityEngine;
using MyGame.Common.DataFormat;


namespace MyGame.Gameplay
{
    public class NotePoolManager : MonoBehaviour
    {
        [SerializeField] private Note _notePrefab;
        [SerializeField] private float _noteHeight = 0.55f;

        private float _defaultBPM;
        private float _noteTravelTime;

        private float _noteWidth;

        private readonly Queue<Note> notePool = new Queue<Note>();


        public void Init(LevelData data)
        {
            int poolSize = CalculatePoolSize(_defaultBPM, data.noteTravelTime);
            _noteTravelTime = data.noteTravelTime;

            for (int i = 0; i < poolSize; i++)
            {
                ReturnNote(CreateNote());
            }
        }



        public Note GetNote()
        {
            Note note = notePool.Count > 0 ? notePool.Dequeue() : CreateNote();
            note.gameObject.SetActive(true);
            return note;
        }

        public void ReturnNote(Note note)
        {
            note.gameObject.SetActive(false);
            notePool.Enqueue(note);
        }

        private Note CreateNote()
        {
            Note note = Instantiate(_notePrefab, transform);
            note.OnKilled += ReturnNote;
            note.Setup(_noteTravelTime, _noteWidth, _noteHeight);
            return note;
        }

        private int CalculatePoolSize(float bpm, float noteTravelTime)
        {
            float baseSize = bpm / 60f * noteTravelTime * 1.2f;
            return Mathf.CeilToInt(baseSize);
        }

        public void HandleNoteWidthCalculated(float noteWidth)
        {
            _noteWidth = noteWidth;
        }
    }
}

