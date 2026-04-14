using System.Collections.Generic;
using UnityEngine;
using MyGame.Common.Enums;

namespace MyGame.Gameplay
{
    public class NoteLane : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _sr;
        [SerializeField] private ParticleSystem _hitEffect;
        [SerializeField] private float _spawnPointOffset = 0.5f;

        private NotePoolManager _notePoolManager;

        private const float MAX_HIT_ERROR = 0.2f;
        private const float MISS_JUDGE_BUFFER = 0.05f;

        private int _laneNumber;
        private Vector3 _spawnPosition;
        private Vector3 _hitPosition;

        private readonly (JudgeType Type, float Range)[] judgeRanges = new[]
        {
        (JudgeType.Perfect, 0.05f),
        (JudgeType.Great,   0.10f),
        (JudgeType.Good,    0.15f),
        (JudgeType.Bad,     0.20f),
    };

        private List<LaneEntry> entryList = new List<LaneEntry>();

        public struct LaneEntry
        {
            public Note Note;
            public float HitTime;
            public float MissTimeThreshold;
        }


        public void Init(NotePoolManager notePoolManager, int laneNumber, float laneWidth, float laneHeight)
        {
            _notePoolManager = notePoolManager;
            _laneNumber = laneNumber;

            _sr.drawMode = SpriteDrawMode.Sliced;
            _sr.size = new Vector2(laneWidth, laneHeight);

            float topY = laneHeight / 2f;
            _spawnPosition = new Vector3(transform.position.x, topY + _spawnPointOffset, 0f);
            _hitPosition = new Vector3(transform.position.x, -topY, 0f);
            _hitEffect.transform.position = _hitPosition;
        }

        private void Update()
        {
            if (entryList.Count <= 0) return;
            float currentTime = GameTimer.Instance.CurrentTime;
            
            if (currentTime >= this.entryList[0].MissTimeThreshold)
            {
                ScoreManager.Instance.HitNote(JudgeType.Miss);
                this.entryList[0].Note.Kill();
                this.entryList.RemoveAt(0);
            }
        }


        public void SpawnNote(float hitTime)
        {
            Note note = _notePoolManager.GetNote();
            note.Init(_spawnPosition, _hitPosition, hitTime, _laneNumber);

            LaneEntry entry = new LaneEntry();
            entry.Note = note;
            entry.HitTime = hitTime;
            entry.MissTimeThreshold = hitTime + MAX_HIT_ERROR + MISS_JUDGE_BUFFER;

            entryList.Add(entry);

        }

        public void OnInputFunc()
        {
            _hitEffect.Play();

            if (entryList.Count <= 0) return;

            if (TryJudgeNote(0)) return;

            bool isCurrentNoteTooLate = entryList.Count >= 2 &&
            GameTimer.Instance.CurrentTime > this.entryList[0].HitTime + MAX_HIT_ERROR;

            if (isCurrentNoteTooLate)
            {
                TryJudgeNote(1);
            }

        }

        public bool TryJudgeNote(int index)
        {
            if (index >= entryList.Count) return false;

            float currentTime = GameTimer.Instance.CurrentTime;
            float targetHitTime = this.entryList[index].HitTime;

            if (currentTime < targetHitTime - MAX_HIT_ERROR) return false;

            float hitError = Mathf.Abs(currentTime - targetHitTime);
            bool judged = false;

            foreach (var judge in judgeRanges)
            {
                if (hitError < judge.Range)
                {
                    ScoreManager.Instance.HitNote(judge.Type);
                    judged = true;
                    break;
                }
            }
            if (judged)
            {
                this.entryList[index].Note.Kill();
                this.entryList.RemoveAt(index);
                return true;
            }
            else
            {
                return false;
            }
        }



    }
}

