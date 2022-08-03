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
    [SerializeField] private string gameScene;

    [Header("UI")]
    [SerializeField] private Text roomCode;
    [SerializeField] private Toggle togglePVP;
    [SerializeField] private GameObject startBtn;
    [SerializeField] private PlayerCell playerCellPrefab;
    [SerializeField] private Transform spectatorContent;
    [SerializeField] private Transform playerContent;
    private List<PlayerCell> playerList = new List<PlayerCell>();
    private List<Player> readyPlayerList = new List<Player>();
    private string mode;
    private int member;
    private bool pvp;

    void Start()
    {
        mode = (string)PhotonNetwork.CurrentRoom.CustomProperties["MODE"];
        member = (int)PhotonNetwork.CurrentRoom.CustomProperties["MEMBER"];
        pvp = (bool)PhotonNetwork.CurrentRoom.CustomProperties["PVP"];
        togglePVP.isOn = pvp; 

        PhotonNetwork.LocalPlayer.SetCustomProperties(MyCustomProperties.setInfoProperties(true, null));
        roomCode.text = PhotonNetwork.CurrentRoom.Name;

        if(!PhotonNetwork.IsMasterClient)
        {
            togglePVP.gameObject.SetActive(false);
            startBtn.SetActive(false);
        }
    }
    // ******** Ontoggle ********
    public void OnTogglePVP(bool newValue)
    {
        Hashtable properties = new Hashtable();
        properties["PVP"] = newValue;
        PhotonNetwork.CurrentRoom.SetCustomProperties(properties);
    }

    // ******** OnClick ********
    public void OnClickLeave()
    {
        PhotonNetwork.LeaveRoom();
    }
    public void OnClickJoin()
    {
        PhotonNetwork.LocalPlayer.SetCustomProperties(MyCustomProperties.setInfoProperties(false, null));
    }
    public void OnClickCancel()
    {
        PhotonNetwork.LocalPlayer.SetCustomProperties(MyCustomProperties.setInfoProperties(true, null));
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

        if(PhotonNetwork.IsMasterClient)
            startBtn.GetComponent<Button>().interactable = readyPlayerList.Count > 0;
    }
    private void updateList(Transform spectatorTarget, Transform playerTarget)
    {
        foreach (PlayerCell player in playerList)
        {
            Destroy(player.gameObject);
        }
        playerList.Clear();
        readyPlayerList.Clear();

        if (PhotonNetwork.CurrentRoom == null)
        {
            return;
        }

        foreach (KeyValuePair<int, Player> player in PhotonNetwork.CurrentRoom.Players)
        {
            if (player.Value.CustomProperties.ContainsKey("ISSPECTATE"))
            {
                if (!(bool)player.Value.CustomProperties["ISSPECTATE"])
                {
                    PlayerCell newPlayer = Instantiate(playerCellPrefab, playerTarget);
                    newPlayer.setPlayerName(player.Value);
                    playerList.Add(newPlayer);
                    readyPlayerList.Add(player.Value);
                }
                if ((bool)player.Value.CustomProperties["ISSPECTATE"])
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
        if (!PhotonNetwork.IsMasterClient)
            return;
        Debug.Log("Starting Game . . .");
        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.LoadLevel(gameScene);
    }
}
