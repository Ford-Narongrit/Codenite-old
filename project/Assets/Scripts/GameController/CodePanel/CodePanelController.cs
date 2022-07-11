using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using ExitGames.Client.Photon;

public class CodePanelController : MonoBehaviour
{
    [SerializeField] private GameUIController gameUIController;
    [Header("ItemList info")]
    [SerializeField] private int maxInventory = 5;
    [SerializeField] private int answerSlot = 3;

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
    private Hashtable myCustomProperties = new Hashtable();
    private void Start()
    {
        for (int i = 0; i < maxInventory; i++)
        {
            GameObject newSlot = Instantiate(inventoryBorderPrefab, inventoryTransform);
            inventorySlotList.Add(newSlot);
        }

        for (int i = 0; i < answerSlot; i++)
        {
            GameObject newSlot = Instantiate(answerSlotPrefab, answerSlotTransform);
            newSlot.GetComponentInChildren<AnswerSlot>().setIndexText(i + 1);
            answerList.Add(newSlot.GetComponentInChildren<AnswerSlot>());
        }
    }

    public void pickItem(string itemName)
    {
        if (itemList.Count < maxInventory)
        {
            // ItemModel newItem = new ItemModel(itemName);
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
                Debug.Log("index: " + i + " empty.");
                return i;
            }
        }
        return 0;
    }

    public void OnClickReset()
    {
        Debug.Log("OnclickReset");
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
        Debug.Log("Submit");
        if (checkAnswer())
        {
            myCustomProperties["QUALIFIED"] = true;
            PhotonNetwork.LocalPlayer.SetCustomProperties(myCustomProperties);
            gameUIController.setSpectator();
        }
        else
        {
            // error alert
        }

    }

    private bool checkAnswer()
    {
        //TODO add condition to submit
        foreach (AnswerSlot slot in answerList)
        {
            if (slot.getItem() != null)
                Debug.Log(slot.getItem().getitemName());
        }
        return true;
    }
}
