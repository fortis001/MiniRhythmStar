using System;
using System.Collections;
using System.Collections.Generic;
using MyGame.Common.DataFormat;
using MyGame.Common.Enums;
using MyGame.Core.Managers;
using MyGame.UI.Components;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ScrollController : MonoBehaviour, IBeginDragHandler, IEndDragHandler
{
    [SerializeField] private AnimationCurve _offsetCurve;
    [SerializeField] private float _maxHorizontalOffset = 200f;

    [Header("Snap Settings")]
    [SerializeField] private float _snapVelocityThreshold = 50f;
    [SerializeField] private float _snapSpeed = 10f;

    private ScrollRect _scrollRect;
    private RectTransform _viewport;
    private List<ScrollItem> _items = new List<ScrollItem>();

    private ScrollState _state;
    private ScrollItem _targetItem;

    private float _viewportHalfHeight = 0;
    private float _viewportPadding = 100f;

    public event Action<ScrollItem> OnItemCentered;
    public event Action<ScrollItem> OnItemConfirmed;

    private void Awake()
    {
        _scrollRect = GetComponent<ScrollRect>();
        _viewport = _scrollRect.viewport;

        _scrollRect.onValueChanged.AddListener(_ => UpdateItemPositions());
    }

    private void Start()
    {
        StartCoroutine(InitializeAfterLayout());
    }

    private IEnumerator InitializeAfterLayout()
    {
        yield return null; // 한 프레임 대기
        SetContentPadding();
        UpdateItemPositions();
        _targetItem = GetClosestItemToCenter();
        OnItemCentered.Invoke(_targetItem);
    }

    private void Update()
    {
        switch (_state)
        {
            case ScrollState.Idle:
                break;
            case ScrollState.Snapping:
                PerformSnap(); break;
            case ScrollState.Dragging:
                break;
            case ScrollState.Coasting:
                UpdateCoasting();
                break;
        }

    }

    public void RegisterItem(ScrollItem item)
    {
        if (!_items.Contains(item))

        {

            _items.Add(item);

            item.OnClicked += HandleItemClicked;

        }
    }

    private void HandleItemClicked(ScrollItem item)
    {
        if(item == _targetItem && _state == ScrollState.Idle)
        {
            ConfirmCurrentItem();
        }
        else
        {
            SoundManager.Instance.PlaySFX(SFXID.UI_Btn_Click);
            StartSnap(item);
        }
            
    }
    public void ConfirmCurrentItem()
    {
        if (_state != ScrollState.Idle) return;
        SoundManager.Instance.PlaySFX(SFXID.Game_Start);
        OnItemConfirmed?.Invoke(_targetItem);
    }

    private void UpdateCoasting()
    {
        float sqrVelocity = _scrollRect.velocity.sqrMagnitude;

        if (sqrVelocity <= 0.01f)
        {
            ChangeState(ScrollState.Idle);
            return;
        }

        if (sqrVelocity < _snapVelocityThreshold * _snapVelocityThreshold)
        {
            StartSnap();
        }
    }

    private void StartSnap(ScrollItem target = null)
    {
        _scrollRect.velocity = Vector2.zero;
        _targetItem = (target != null) ? target : GetClosestItemToCenter();
        ChangeState(ScrollState.Snapping);
    }

    private void PerformSnap()
    {
        if (_targetItem == null) return;

        Vector3 viewportCenter = _viewport.TransformPoint(_viewport.rect.center);

        Vector3 itemWorldPos = _targetItem.transform.position;
        Vector3 offset = viewportCenter - itemWorldPos;

        Vector2 targetContentPos = _scrollRect.content.anchoredPosition + new Vector2(0, offset.y);

        float distance = Vector2.Distance(_scrollRect.content.anchoredPosition, targetContentPos);

        if (distance < 0.1f)
        {
            _scrollRect.content.anchoredPosition = targetContentPos;
            _scrollRect.velocity = Vector2.zero;
            OnItemCentered?.Invoke(_targetItem);
            ChangeState(ScrollState.Idle);
            return;
        }

        float t = 1f - Mathf.Exp(-_snapSpeed * Time.deltaTime);
        _scrollRect.content.anchoredPosition = Vector2.Lerp(
            _scrollRect.content.anchoredPosition,
            targetContentPos,
            t
        );

    }

    private void UpdateItemPositions()
    {
        if (_viewportHalfHeight == 0)
        {
            _viewportHalfHeight = _viewport.rect.height * 0.5f;
        }

        float viewportHeightLimit = _viewportHalfHeight + _viewportPadding;


        foreach (ScrollItem item in _items)
        {
            Vector3 itemLocalPos = _viewport.InverseTransformPoint(item.transform.position);

            Vector3 viewportCenter = _viewport.rect.center;

            float localY = itemLocalPos.y - viewportCenter.y;
            float absY = Mathf.Abs(localY);

            Vector2 visualPos = item._visualRoot.anchoredPosition;


            if (absY < viewportHeightLimit)
            {
                float normalizedDist = Mathf.Clamp01(absY / _viewportHalfHeight);
                float intensity = _offsetCurve.Evaluate(normalizedDist);
                visualPos.x = (1f - intensity) * _maxHorizontalOffset;
            }
            else
            {
                visualPos.x = 0f;
            }

            item._visualRoot.anchoredPosition = visualPos;

        }
    }

    private ScrollItem GetClosestItemToCenter()
    {
        ScrollItem closest = null;
        float minDistance = float.MaxValue;

        Vector2 viewportCenter = Vector2.zero;

        foreach (var item in _items)
        {
            Vector2 itemLocalPos = _viewport.InverseTransformPoint(item.transform.position);
            float distance = Mathf.Abs(itemLocalPos.y - viewportCenter.y);

            if (distance < minDistance)
            {
                minDistance = distance;
                closest = item;
            }
        }
        return closest;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        ChangeState(ScrollState.Dragging);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        ChangeState(ScrollState.Coasting);
    }


    private void OnDestroy()
    {
        if (_scrollRect != null)
            _scrollRect.onValueChanged.RemoveAllListeners();

        foreach (var item in _items)
        {
            if (item != null) item.OnClicked -= HandleItemClicked;
        }
    }

    private void ChangeState(ScrollState state)
    {
        _state = state;
    }

    public void SetContentPadding()
    {
        VerticalLayoutGroup layoutGroup = _scrollRect.content.GetComponent<VerticalLayoutGroup>();
        if (layoutGroup != null)
        {
            float itemHeight = _items.Count > 0 ? _items[0].rectTransform.rect.height : 0;
            int paddingValue = Mathf.RoundToInt((_viewport.rect.height * 0.5f) - (itemHeight * 0.5f));

            layoutGroup.padding.top = paddingValue;
            layoutGroup.padding.bottom = paddingValue;

            LayoutRebuilder.ForceRebuildLayoutImmediate(_scrollRect.content);
        }
    }
    
}
