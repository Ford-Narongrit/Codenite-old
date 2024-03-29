using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class AnswerSlot : MonoBehaviour, IDropHandler
{
    [SerializeField] private Text answerIndexText;
    [SerializeField] private Image answerImage;
    private ItemController keepedItem;
    private int answerIndex;
    private void LateUpdate()
    {
        if (keepedItem != null)
        {
            if (!keepedItem.isUse)
            {
                keepedItem = null;
            }
        }
    }
    public void setIndexText(int index)
    {
        answerIndexText.text = index + " :";
        answerIndex = index;
    }
    public ItemController getItem()
    {
        return keepedItem;
    }
    public void showAnswer(string _ans)
    {
        answerIndexText.text = answerIndexText.text + " <color=#00000050>" + _ans + "</color>";
    }
    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            eventData.pointerDrag.GetComponent<ItemController>().setIsUse(true);
            eventData.pointerDrag.GetComponent<RectTransform>().position = GetComponent<RectTransform>().position;

            keepedItem = eventData.pointerDrag.GetComponent<ItemController>();
        }
    }
}
