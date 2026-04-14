using UnityEngine;
using System.Linq;

namespace MyGame.Gameplay
{
    public class LaneInputHandler : MonoBehaviour
    {
        [SerializeField] LaneBuilder _laneBuilder;


        private readonly KeyCode[] _allNumpadKeys = new KeyCode[]
        {
        KeyCode.Keypad1, KeyCode.Keypad2, KeyCode.Keypad3,
        KeyCode.Keypad4, KeyCode.Keypad5, KeyCode.Keypad6,
        KeyCode.Keypad7, KeyCode.Keypad8, KeyCode.Keypad9
        };

        private NoteLane[] _activeNoteLanes;
        private KeyCode[] _activeKeyCodes;



        private void Awake()
        {
            _laneBuilder.OnLaneSetupCompleted += Init;
        }



        private void Init(NoteLane[] activeNoteLanes)
        {
            _activeNoteLanes = activeNoteLanes;
            _activeKeyCodes = _allNumpadKeys.Take(activeNoteLanes.Length).ToArray();
        }

        private void Update()
        {
            for (int laneIndex = 0; laneIndex < _activeKeyCodes.Length; laneIndex++)
            {
                if (Input.GetKeyDown(_activeKeyCodes[laneIndex]))
                {
                    NoteLane targetLane = _activeNoteLanes[laneIndex];

                    if (targetLane != null)
                    {
                        targetLane.OnInputFunc();
                    }
                }
            }

        }

        void OnDestroy()
        {
            _laneBuilder.OnLaneSetupCompleted -= Init;
        }
    }

}
