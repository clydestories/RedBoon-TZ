using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform), typeof(CanvasGroup))]
public class Cell : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [SerializeField] private Image _visual;

    private ItemSO _currentItem;
    private Canvas _canvas;
    private RectTransform _rectTransform;
    private CanvasGroup _canvasGroup;
    private Vector2 _initialPosition;
    private Transform _initialParent;

    public ItemSO CurrentItem => _currentItem;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    public void Construct(ItemSO item, Canvas canvas)
    {
        _currentItem = item;
        _visual.sprite = _currentItem.Sprite;
        _canvas = canvas;
    }

    public void OnDrag(PointerEventData eventData)
    {
        _rectTransform.anchoredPosition += eventData.delta / _canvas.scaleFactor;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _initialPosition = _rectTransform.anchoredPosition;
        _initialParent = _rectTransform.parent;
        _rectTransform.SetParent(_canvas.transform);
        _canvasGroup.blocksRaycasts = false;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _rectTransform.anchoredPosition = _initialPosition;
        _rectTransform.SetParent(_initialParent, false);
        _canvasGroup.blocksRaycasts = true;
    }
}
