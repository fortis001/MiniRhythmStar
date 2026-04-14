using System.Collections.Generic;
using System.Linq;
using MyGame.Core.Managers;
using UnityEngine;

public class EditorNoteManager : MonoBehaviour
{
    [SerializeField] TimelineSetup _timelineSetup;
    [SerializeField] EditorInputManager _inputManager;
    [SerializeField] RectTransform _noteChart;
    [SerializeField] EditorNote _notePrefab;

    private float _laneWidth = 0;
    private EditorNote _selectedNote;

    private List<Timeline> _activeLanes = new List<Timeline>();
    private List<EditorNote> _notes = new List<EditorNote>();


    private void Awake()
    {
        _timelineSetup.OnCreateTimeline += HandleCreateTimeline;
        _inputManager.OnNoteKeyPressed += HandleNoteKeyPressed;
        _inputManager.OnDeleteKeyPressed += HandleDeletePressed;
    }

    private void HandleCreateTimeline(List<Timeline> activeLanes)
    {
        _activeLanes = activeLanes;
        _laneWidth = _activeLanes[0].MaxX - _activeLanes[0].MinX;

        _noteChart.SetAsLastSibling();

        ClearNote();
        // ++ 클리어노트 대신 index가 lanes보다 높은 얘들 옮겨주는 함수로 바꾸기
    }

    private void HandleNoteKeyPressed(int laneIndex)
    {
        float timeInSeconds = SoundManager.Instance.CurrentTime;
        float yPos = timeInSeconds * _timelineSetup.PixelsPerSecond;


        CreateNote(laneIndex, timeInSeconds, yPos);
    }

    public void HandleLaneClick(int laneIndex, float yPos)
    {
        float timeInSeconds = yPos / _timelineSetup.PixelsPerSecond;

        CreateNote(laneIndex, timeInSeconds, yPos);
    }

    private void CreateNote(int laneIndex, float timeInSeconds, float yPos)
    {
        float centerX = _activeLanes[laneIndex].CenterX;

        EditorNote note = Instantiate(_notePrefab, _noteChart);
        note.Initialize(laneIndex, timeInSeconds, new Vector2(centerX, yPos), _laneWidth);

        note.OnPositionChanged += HandleNoteUpdate;
        note.OnNoteClicked += HandleNoteClick;
        _notes.Add(note);
    }

    private void HandleNoteUpdate(EditorNote note, Vector2 droppedPos)
    {
        int newLaneIndex = GetLaneIndexFromX(droppedPos.x);
        float centerX = _activeLanes[newLaneIndex].CenterX;
        float newTime = droppedPos.y / _timelineSetup.PixelsPerSecond;


        note.Initialize(newLaneIndex, newTime, new Vector2(centerX, droppedPos.y), _laneWidth);

    }

    private int GetLaneIndexFromX(float xPos)
    {
        foreach (var lane in _activeLanes)
        {
            if (xPos >= lane.MinX && xPos < lane.MaxX)
                return lane.LaneIndex;
        }
        // 범위 밖이면 가장 가까운 레인
        return Mathf.Clamp((int)(xPos / (_activeLanes[0].MaxX - _activeLanes[0].MinX)),
                           0, _activeLanes.Count - 1);
    }

    private void HandleNoteClick(EditorNote note)
    {
        _selectedNote = note;
    }

    private void HandleDeletePressed()
    {
        if (_selectedNote == null) return;
        DeleteNote();
    }

    private void DeleteNote()
    {
        _notes.Remove(_selectedNote);
        DeleteNote(_selectedNote);
        _selectedNote = null;
    }

    private void DeleteNote(EditorNote note)
    {
        note.OnPositionChanged -= HandleNoteUpdate;
        note.OnNoteClicked -= HandleNoteClick;

        Destroy(note.gameObject);
    }

    private void ClearNote()
    {
        foreach(EditorNote note in _notes)
        {
            DeleteNote(note);
        }
        _notes.Clear();
    }

    public IReadOnlyList<EditorNote> GetSortedNotes()
    {
        return _notes.OrderBy(n => n.TimeInSeconds).ToList();
    }
}
