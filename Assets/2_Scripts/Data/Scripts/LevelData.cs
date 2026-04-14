using MyGame.Common.Enums;
using UnityEngine;


namespace MyGame.Common.DataFormat
{
    [CreateAssetMenu]
    public class LevelData : ScriptableObject
    {
        [Header("Basic Information")]
        public Level level;

        [Space(10)] // 픽셀 단위 공백 추가
        [Header("Gameplay Settings")]
        [Tooltip("노트가 생성되어 판정선에 도달할 때까지의 시간(초)")]
        public float noteTravelTime;

        [Space(10)]
        [Header("Visual Layout")]
        [Range(1, 9)]
        public int numOfLanes;
    }
}
