using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;


public class TeamCell : MonoBehaviourPunCallbacks
{
    [SerializeField] private PlayerCell playerCellPrefab;
    [SerializeField] private Text teamName;
    [SerializeField] private Transform content;
    [SerializeField] private Button joinBtn;
    private List<PlayerCell> playerList = new List<PlayerCell>();
    private int maxMember = 4;
    private void Start()
    {
        maxMember = (int)PhotonNetwork.CurrentRoom.CustomProperties["MEMBER"];
    }
    private void FixedUpdate()
    {
        if (playerList.Count >= maxMember)
        {
            joinBtn.interactable = false;
        }
        else
        {
            joinBtn.interactable = true;
        }
    }
    public void setTeamName(string _teamName)
    {
        teamName.text = _teamName;
    }
    public string getTeamName()
    {
        return teamName.text;
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        base.OnPlayerPropertiesUpdate(targetPlayer, changedProps);
        updateTeam();
    }

    private void updateTeam()
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
            if (player.Value.CustomProperties.ContainsKey("TEAM"))
            {
                if ((string)player.Value.CustomProperties["TEAM"] == teamName.text)
                {
                    PlayerCell newPlayer = Instantiate(playerCellPrefab, content);
                    newPlayer.setPlayerName(player.Value);
                    playerList.Add(newPlayer);
                }
            }
        }
    }
}
