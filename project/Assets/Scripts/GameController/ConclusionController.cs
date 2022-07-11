using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class ConclusionController : MonoBehaviour
{
    [Header("Scene")]
    [SerializeField] private string gameScene;
    [SerializeField] private string waittingScene;
    [Header("info")]
    [SerializeField] private float nextSceneTime;
    [SerializeField] private int maxWinner;
    [Header("Object")]
    [SerializeField] private Text timerText;
    [SerializeField] private GameObject contentview;
    [SerializeField] private GameObject leaderCell;
    private float currentTime;
    private int currentplayers;
    private bool startingGame;
    private void Start()
    {
        currentTime = nextSceneTime;
        foreach (KeyValuePair<int, Player> player in PhotonNetwork.CurrentRoom.Players)
        {
            if ((bool)player.Value.CustomProperties["ISPLAY"])
            {
                GameObject leader = GameObject.Instantiate(leaderCell, contentview.transform);
                LeaderCell cell = leader.GetComponent<LeaderCell>();
                cell.setPlayerinfo(player.Value);

                if ((bool)player.Value.CustomProperties["QUALIFIED"])
                {
                    currentplayers++;
                }
            }
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
            if (!startingGame)
            {
                if (currentplayers > maxWinner)
                    startGame();
                else
                    backtoWaitRoom();

            }
        }
    }

    private void startGame()
    {
        startingGame = true;
        if (!PhotonNetwork.IsMasterClient)
            return;
        Debug.Log("Starting Game . . .");
        PhotonNetwork.LoadLevel(gameScene);
    }

    private void backtoWaitRoom()
    {
        startingGame = true;
        if (!PhotonNetwork.IsMasterClient)
            return;
        Debug.Log("back to waitting room . . .");
        PhotonNetwork.LoadLevel(gameScene);
    }
}
