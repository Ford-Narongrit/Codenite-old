using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemController : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Vector2 startLocalPosition;

    [Header("item info")]
    [SerializeField] public bool isUse = false;

    [Header("UI")]
    [SerializeField] private Text itemNameText;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }
    private void Start()
    {
        startLocalPosition = rectTransform.localPosition;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        // Debug.Log("OnPointerDown");
    }
    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta;
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 0.8f;
        canvasGroup.blocksRaycasts = false;
        setIsUse(false);
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
        if (!isUse)
        {
            resetPosition();
        }
    }

    public void resetPosition()
    {
        rectTransform.localPosition = startLocalPosition;
        setIsUse(false);
    }

    public void setIsUse(bool _isUse)
    {
        isUse = _isUse;
    }
    public void setItemName(string itemName)
    {
        itemNameText.text = itemName;
    }

    public string getitemName()
    {
        return itemNameText.text;
    }
}
