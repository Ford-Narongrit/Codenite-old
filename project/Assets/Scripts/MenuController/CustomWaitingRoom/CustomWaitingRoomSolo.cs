using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class CustomWaitingRoomSolo : MonoBehaviourPunCallbacks
{
    [Header("Scene")]
    [SerializeField] private string startMenu;

    [Header("UI")]
    [SerializeField] private Text roomCode;
    [SerializeField] private PlayerCell playerCellPrefab;
    [SerializeField] private Transform spectatorContent;
    [SerializeField] private Transform playerContent;
    private List<PlayerCell> playerList = new List<PlayerCell>();
    private Hashtable myCustomProperties = new Hashtable();
    private string mode;
    private int member;

    void Start()
    {
        mode = (string)PhotonNetwork.CurrentRoom.CustomProperties["MODE"];
        member = (int)PhotonNetwork.CurrentRoom.CustomProperties["MEMBER"];

        myCustomProperties["ISPLAY"] = false;
        PhotonNetwork.LocalPlayer.SetCustomProperties(myCustomProperties);

        roomCode.text = PhotonNetwork.CurrentRoom.Name;
    }

    // ******** OnClick ********
    public void OnClickLeave()
    {
        PhotonNetwork.LeaveRoom();
    }
    public void OnClickJoin()
    {
        myCustomProperties["ISPLAY"] = true;
        PhotonNetwork.LocalPlayer.SetCustomProperties(myCustomProperties);
    }
    public void OnClickCancel()
    {
        myCustomProperties["ISPLAY"] = false;
        PhotonNetwork.LocalPlayer.SetCustomProperties(myCustomProperties);
    }
    public void OnClickStart()
    {
        startGame();
    }

    // ******** Callsback ********
    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(startMenu);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        updateAllPlayerList();
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        updateAllPlayerList();
    }
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        base.OnPlayerPropertiesUpdate(targetPlayer, changedProps);
        if (targetPlayer != null)
        {
            updateAllPlayerList();
        }
    }

    // ******** private ********
    private void updateAllPlayerList()
    {
        updateList(spectatorContent, playerContent);
    }
    private void updateList(Transform spectatorTarget, Transform playerTarget)
    {
        foreach (PlayerCell player in playerList)
        {
            Destroy(player.gameObject);
        }
        playerList.Clear();

        if (PhotonNetwork.CurrentRoom == null)
        {
            return;
        }

        foreach (KeyValuePair<int, Player> player in PhotonNetwork.CurrentRoom.Players)
        {
            if (player.Value.CustomProperties.ContainsKey("ISPLAY"))
            {
                if ((bool)player.Value.CustomProperties["ISPLAY"])
                {
                    PlayerCell newPlayer = Instantiate(playerCellPrefab, playerTarget);
                    newPlayer.setPlayerName(player.Value);
                    playerList.Add(newPlayer);
                }
                if (!(bool)player.Value.CustomProperties["ISPLAY"])
                {
                    PlayerCell newPlayer = Instantiate(playerCellPrefab, spectatorTarget);
                    newPlayer.setPlayerName(player.Value);
                    playerList.Add(newPlayer);
                }
            }
        }
    }

    private void startGame()
    {
        Debug.Log("startGame custom");
    }
}
