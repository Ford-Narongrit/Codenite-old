using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class WarpGateController : MonoBehaviourPunCallbacks
{
    [SerializeField] private int limitPlayerInZone = 1;
    [SerializeField] private Transform warpTo;
    [SerializeField] public PhotonView view;

    [Header("Player UI")]
    [SerializeField] private GameObject waitingPanel;
    [SerializeField] private Text waitingQueue;

    private int currentPlayerInZone = 0;
    private List<int> playerQueue = new List<int>();
    private void Update()
    {
        if (currentPlayerInZone < limitPlayerInZone && playerQueue.Count > 0)
        {
            view.RPC("warp", RpcTarget.All, playerQueue[0]);
        }
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            view.RPC("addToQueue", RpcTarget.All, other.GetComponent<PhotonView>().ViewID);
            if (other.GetComponent<PhotonView>().IsMine)
            {
                waitingPanel.SetActive(true);
                updateWaitingPanel(other.GetComponent<PhotonView>().ViewID);
            }
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        view.RPC("removeToQueue", RpcTarget.All, other.GetComponent<PhotonView>().ViewID);
        if (other.GetComponent<PhotonView>().IsMine)
        {
            waitingPanel.SetActive(false);
        }
    }

    private void updateWaitingPanel(int playerViewID)
    {
        int queue = playerQueue.IndexOf(playerViewID) + 1;
        waitingQueue.text = queue + ".";
    }

    [PunRPC]
    public void addToQueue(int playerViewID)
    {
        if (!playerQueue.Contains(playerViewID))
        {
            playerQueue.Add(playerViewID);
        }
    }

    [PunRPC]
    public void removeToQueue(int playerViewID)
    {
        playerQueue.Remove(playerViewID);
    }

    [PunRPC]
    public void warp(int playerViewID)
    {
        GameObject player = PhotonView.Find(playerViewID).gameObject;
        player.transform.position = warpTo.transform.position;
        currentPlayerInZone++;
    }

    [PunRPC]
    public void warpOut()
    {
        Debug.Log("warpOut");
        currentPlayerInZone--;
        if(currentPlayerInZone < 0)
        {
            currentPlayerInZone = 0;
        }
    }
}
