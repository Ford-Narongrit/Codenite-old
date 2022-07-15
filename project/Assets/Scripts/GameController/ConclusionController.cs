using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class ConclusionController : MonoBehaviourPunCallbacks
{
    [Header("Scene")]
    [SerializeField] private string gameScene;
    [SerializeField] private string startMenu;
    [Header("info")]
    [SerializeField] private float nextSceneTime;
    [SerializeField] private int maxWinner;
    [Header("Object")]
    [SerializeField] private Text timerText;
    [SerializeField] private GameObject contentview;
    [SerializeField] private GameObject leaderCell;
    private float currentTime;
    private int currentplayers;
    private bool isLoading = false;
    private bool isHaveWinner = false;
    private void Start()
    {
        currentTime = nextSceneTime;
        loadNextLevel();
        foreach (KeyValuePair<int, Player> player in PhotonNetwork.CurrentRoom.Players)
        {
            if (!(bool)player.Value.CustomProperties["ISSPECTATE"])
            {
                GameObject leader = GameObject.Instantiate(leaderCell, contentview.transform);
                LeaderCell cell = leader.GetComponent<LeaderCell>();
                cell.setPlayerinfo(player.Value);

                if (!(bool)player.Value.CustomProperties["GAMEOVER"])
                {
                    currentplayers++;
                }
                if ((bool)player.Value.CustomProperties["GAMEOVER"])
                {
                    PhotonNetwork.LocalPlayer.SetCustomProperties(MyCustomProperties.setQualified(false));
                }
            }
        }

        if (currentplayers <= maxWinner)
        {
            isHaveWinner = true;
        }
    }

    private void Update()
    {
        countDown();
    }

    private void countDown()
    {
        currentTime -= Time.deltaTime;
        string tempTimer = string.Format("{0:00}", currentTime);
        timerText.text = tempTimer;

        if (currentTime <= 0)
        {
            if (isHaveWinner)
            {
                if (!isLoading)
                {
                    backtoWaitRoom();
                }
            }
            else
            {
                if (!isLoading)
                {
                    startGame();
                }
            }
        }
    }

    private void loadNextLevel()
    {
        Hashtable customRoomProperties = new ExitGames.Client.Photon.Hashtable();
        customRoomProperties["PROBLEMINDEX"] = (int)PhotonNetwork.CurrentRoom.CustomProperties["PROBLEMINDEX"] + 1;
        PhotonNetwork.CurrentRoom.SetCustomProperties(customRoomProperties);
    }

    private void startGame()
    {
        isLoading = true;
        if (!PhotonNetwork.IsMasterClient)
            return;
        Debug.Log("Starting Game . . . : " + (int)PhotonNetwork.CurrentRoom.CustomProperties["PROBLEMINDEX"]);
        PhotonNetwork.LoadLevel(gameScene);
    }

    private void backtoWaitRoom()
    {
        isLoading = true;
        Debug.Log("back to waitting room . . .");
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(startMenu);
    }
}
