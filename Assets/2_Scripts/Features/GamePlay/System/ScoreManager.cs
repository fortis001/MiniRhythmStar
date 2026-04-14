using System;
using System.Collections.Generic;
using System.Linq;
using MyGame.Common.Enums;
using MyGame.Core.RuntimeData;
using UnityEngine;

namespace MyGame.Gameplay
{
    public class ScoreManager : MonoBehaviour
    {
        private const int PerfectScore = 300;
        private const int GreatScore = 200;
        private const int GoodScore = 100;
        private const int BadScore = 50;

        private readonly Dictionary<JudgeType, (int BaseScore, bool ComboIncrement, bool ComboBreak)> _judgeTable =
            new Dictionary<JudgeType, (int BaseScore, bool ComboIncrement, bool ComboBreak)>
        {
        //              (기본 점수, 콤보 증가 여부, 콤보 초기화 여부)
        { JudgeType.Perfect, (PerfectScore, true,  false) },
        { JudgeType.Great,   (GreatScore,   true,  false) },
        { JudgeType.Good,    (GoodScore,    true,  false) },
        { JudgeType.Bad,     (BadScore,     false, true)  },
        { JudgeType.Miss,    (0,            false, true)  }
        };

        private Dictionary<JudgeType, int> _judgeCount = new Dictionary<JudgeType, int>
        {
        { JudgeType.Perfect, 0 },
        { JudgeType.Great,   0 },
        { JudgeType.Good,    0 },
        { JudgeType.Bad,     0 },
        { JudgeType.Miss,    0 }
        };


        private int _score = 0;
        private int _combo = 0;
        private int _maxCombo = 0;
        public int CurrentScore => _score;
        public int CurrentCombo => _combo;
        public int MaxCombo => _maxCombo;

        public Dictionary<JudgeType, int> JudgeCount => _judgeCount;
        public Rank Rank => GetRank();

        public event Action<int> OnScoreChanged;
        public event Action<ComboUpdateData> OnComboIncrease;
        public event Action OnComboBreak;

        public static ScoreManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }

        public void HitNote(JudgeType judge)
        {
            _judgeCount[judge]++;

            if (!_judgeTable.TryGetValue(judge, out var judgeData))
            {
                return;
            }

            if (judgeData.ComboBreak)
            {
                UpdateMaxCombo();
                _combo = 0;
                OnComboBreak?.Invoke();
            }
            else if (judgeData.ComboIncrement)
            {
                _combo++;

                ComboUpdateData data = new ComboUpdateData
                {
                    ComboCount = _combo,
                    Judge = judge
                };

                OnComboIncrease?.Invoke(data);
            }

            int baseScore = judgeData.BaseScore;

            if (baseScore > 0)
            {
                int addScore = Mathf.FloorToInt(baseScore * GetComboBonus(_combo));
                _score += addScore;
                OnScoreChanged?.Invoke(_score);
            }
        }

        private float GetComboBonus(int combo)
        {
            float comboBonus = 1;
            int bonusStep = combo / 50;

            comboBonus += (float)bonusStep / 10.0f;

            return comboBonus;
        }

        private Rank GetRank()
        {
            int totalCount = _judgeCount.Values.Sum();
            float perfectRatio = (float)_judgeCount[JudgeType.Perfect] / totalCount;
            float missRatio = (float)_judgeCount[JudgeType.Miss] / totalCount;

            if (missRatio == 0 && perfectRatio >= 0.95f) return Rank.S;
            if (missRatio <= 0.05f) return Rank.A;
            if (missRatio <= 0.1f) return Rank.B;
            if (missRatio <= 0.2f) return Rank.C;
            return Rank.D;
        }

        public void UpdateMaxCombo()
        {
            _maxCombo = Mathf.Max(_maxCombo, _combo);
        }
    }
}


