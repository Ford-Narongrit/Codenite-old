using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class CustomWaitingRoomTeam : MonoBehaviourPunCallbacks
{
    [Header("Scene")]
    [SerializeField] private string startMenu;
    [SerializeField] private string gameScene;

    [Header("UI")]
    [SerializeField] private Text roomCode;
    [SerializeField] private GameObject startBtn;
    [SerializeField] private PlayerCell playerCellPrefab;
    [SerializeField] private TeamCell teamCellPrefab;
    [SerializeField] private Transform spectatorContent;
    [SerializeField] private Transform teamsContent;

    private List<PlayerCell> playerList = new List<PlayerCell>();
    private string mode;
    private int member;
    void Start()
    {
        mode = (string)PhotonNetwork.CurrentRoom.CustomProperties["MODE"];
        member = (int)PhotonNetwork.CurrentRoom.CustomProperties["MEMBER"];
        int maxPlayer = PhotonNetwork.CurrentRoom.MaxPlayers;

        PhotonNetwork.LocalPlayer.SetCustomProperties(MyCustomProperties.setInfoProperties(true, null));

        roomCode.text = PhotonNetwork.CurrentRoom.Name;
        if (!PhotonNetwork.IsMasterClient)
        {
            startBtn.SetActive(false);
        }

        for (int i = 1; i <= maxPlayer / member; i++)
        {
            TeamCell newTeam = Instantiate(teamCellPrefab, teamsContent);
            newTeam.GetComponentInChildren<Button>().onClick.AddListener(delegate { OnClickJoin(newTeam.gameObject); });
            newTeam.setTeamName("TEAM" + i);
        }
    }

    // ******** OnClick ********
    public void OnClickLeave()
    {
        PhotonNetwork.LeaveRoom();
    }
    public void OnClickJoin(GameObject btn)
    {
        PhotonNetwork.LocalPlayer.SetCustomProperties(MyCustomProperties.setInfoProperties(false, btn.GetComponent<TeamCell>().getTeamName()));
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
        updatePlayerList();
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        updatePlayerList();
    }
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        base.OnPlayerPropertiesUpdate(targetPlayer, changedProps);
        if (targetPlayer != null)
        {
            updatePlayerList();
        }
    }

    // ******** private ********
    private void updatePlayerList()
    {
        foreach (PlayerCell player in playerList)
        {
            Destroy(player.gameObject);
        }
        playerList.Clear();
        foreach (KeyValuePair<int, Player> player in PhotonNetwork.CurrentRoom.Players)
        {
            if (player.Value.CustomProperties.ContainsKey("ISSPECTATE"))
            {
                if ((bool)player.Value.CustomProperties["ISSPECTATE"])
                {
                    PlayerCell newPlayer = Instantiate(playerCellPrefab, spectatorContent);
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
