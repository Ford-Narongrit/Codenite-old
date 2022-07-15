using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class CodePanelController : MonoBehaviour
{
    [SerializeField] private GameUIController gameUIController;
    [Header("ItemList info")]
    [SerializeField] private int maxInventory = 5;
    [SerializeField] private int answerSlot = 3;
    [Header("Text UI")]
    [SerializeField] private Text problem;
    [SerializeField] private Text result;

    [Header("ItemList UI")]
    [SerializeField] private GameObject answerSlotPrefab;
    [SerializeField] private GameObject inventoryBorderPrefab;
    [SerializeField] private GameObject itemPrefab;
    [SerializeField] private Transform inventoryTransform;
    [SerializeField] private Transform answerSlotTransform;

    private List<GameObject> inventorySlotList = new List<GameObject>();
    private List<AnswerSlot> answerList = new List<AnswerSlot>();
    private List<ItemController> itemUI = new List<ItemController>();
    private List<string> itemList = new List<string>();
    private string[] correctAnswerArr;
    private void Start()
    {
        setProblemInfo();
        for (int i = 0; i < maxInventory; i++)
        {
            GameObject newSlot = Instantiate(inventoryBorderPrefab, inventoryTransform);
            inventorySlotList.Add(newSlot);
        }

        for (int i = 0; i < correctAnswerArr.Length; i++)
        {
            GameObject newSlot = Instantiate(answerSlotPrefab, answerSlotTransform);
            newSlot.GetComponentInChildren<AnswerSlot>().setIndexText(i + 1);
            answerList.Add(newSlot.GetComponentInChildren<AnswerSlot>());
        }
    }
    public void setProblemInfo()
    {
        problem.text = JsonReader.getMapInfo().problem;
        result.text = JsonReader.getMapInfo().result;
        correctAnswerArr = JsonReader.getMapInfo().correntAnsArr;
    }

    public void pickItem(string itemName)
    {
        if (itemList.Count < maxInventory)
        {
            if (!itemList.Contains(itemName))
            {
                GameObject newItemObject = Instantiate(itemPrefab, inventorySlotList[getEmpty()].transform);
                newItemObject.GetComponent<ItemController>().setItemName(itemName);

                itemList.Add(itemName);
                itemUI.Add(newItemObject.GetComponent<ItemController>());

                newItemObject.GetComponentInChildren<Button>().onClick.AddListener(delegate { OnClickDelete(newItemObject); });
            }
        }
    }
    private int getEmpty()
    {
        for (int i = 0; i < maxInventory; i++)
        {
            if (inventorySlotList[i].GetComponentInChildren<ItemController>() == null)
            {
                return i;
            }
        }
        return 0;
    }

    public void OnClickReset()
    {
        resetItemToSlot();
    }
    private void resetItemToSlot()
    {
        foreach (ItemController item in itemUI)
        {
            item.resetPosition();
        }
    }

    public void OnClickDelete(GameObject item)
    {
        ConfirmUIController.Instance.showQuestion("Do you want to delete this item ?" + item.GetComponent<ItemController>().getitemName(),
            () =>
            {
                Debug.Log(item.GetComponent<ItemController>().getitemName());
                itemList.Remove(item.GetComponent<ItemController>().getitemName());
                itemUI.Remove(item.GetComponent<ItemController>());
                Destroy(item);
            },
            () =>
            {
                // Do nothing
            }
        );
    }

    public void OnClickSubmit()
    {
        if (checkAnswer())
        {
            PhotonNetwork.LocalPlayer.SetCustomProperties(MyCustomProperties.setGameOver(false));
            gameUIController.setSpectator();
        }
        else
        {
            resetItemToSlot();
        }

    }

    private bool checkAnswer()
    {
        // for (int i = 0; i < correctAnswerArr.Length; i++)
        // {
        //     if (answerList[i].getItem() == null)
        //         return false;
        //     if (correctAnswerArr[i] != answerList[i].getItem().getitemName())
        //     {
        //         return false;
        //     }
        // }
        return true;
    }
}
