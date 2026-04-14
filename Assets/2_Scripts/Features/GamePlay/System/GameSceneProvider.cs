using System.Collections.Generic;
using MyGame.Common.DataFormat;
using MyGame.Common.Enums;
using MyGame.Core.Managers;
using MyGame.Core.RuntimeData;
using UnityEngine;

public class GameSceneProvider : MonoBehaviour
{

    public Queue<RuntimeNote> ProcessRawChart(ChartData rawData, Level chartLevel)
    {

        bool isShared = chartLevel == Level.Shared;
        int lastLane = -1;
        Level userSelecetedLevel = GameManager.Instance.SelectedLevel;
        int laneCount = GetLaneCount(userSelecetedLevel);

        Queue<RuntimeNote> noteQueue = new();

        foreach (NoteData note in rawData.Notes)
        {
            float timeInSeconds = note.TimeMs / 1000f;

            int finalLane = isShared ?
                GetRandomLane(lastLane, laneCount) : note.Lane;

            lastLane = finalLane;


            noteQueue.Enqueue(new RuntimeNote(timeInSeconds, finalLane, note.Type));
        }



        return noteQueue;
    }

    private int GetLaneCount(Level userSelectedLevel) => userSelectedLevel switch
    {
        Level.Easy => 3,
        Level.Normal => 6,
        Level.Hard => 9,
        _ => -1
    };

    private int GetRandomLane(int exceptLane, int laneCount)
    {
        int random = Random.Range(0, laneCount - 1);

        if (random >= exceptLane)
            random++;

        return random;
    }

}
