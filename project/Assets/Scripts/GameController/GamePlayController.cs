using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class GamePlayController : MonoBehaviourPunCallbacks
{
    private PhotonView myPhotonView;
    [SerializeField] private Text timer;
    [Header("Scene")]
    [SerializeField] private string conclusionScene;
    [Header("Rule")]
    [SerializeField] private float gameTime;
    [SerializeField] private float quotaPercent;

    private int quota;
    private int qualifiedPlayer;
    private float currentTime;
    private bool gameOver = false;

    private void Start()
    {
        myPhotonView = GetComponent<PhotonView>();
        quota = (int)Mathf.Ceil(qualifiedPlayerInPreLv() * (quotaPercent / 100));
        currentTime = gameTime;
    }

    private void Update()
    {
        countDown();
        Debug.Log("qualifiedPlayer / quota: " + qualifiedPlayer + " / " + quota);
    }

    private void countDown()
    {
        currentTime -= Time.deltaTime;
        string tempTimer = string.Format("{0:00}", currentTime);
        timer.text = tempTimer;
        if (currentTime <= 0f)
        {
            if (gameOver)
                return;
            finishGame();
        }
    }

    private void finishGame()
    {
        gameOver = true;
        if (!PhotonNetwork.IsMasterClient)
            return;
        PhotonNetwork.LoadLevel(conclusionScene);
        Debug.Log("Game Over");
    }

    private int qualifiedPlayerInPreLv()
    {
        int _qualifiedPlayer = 0;
        foreach (KeyValuePair<int, Player> player in PhotonNetwork.CurrentRoom.Players)
        {
            if ((bool)player.Value.CustomProperties["QUALIFIED"])
            {
                _qualifiedPlayer++;
                Debug.Log("_qualifiedPlayer: "+ _qualifiedPlayer);
            }
        }
        return _qualifiedPlayer;
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if ((bool)targetPlayer.CustomProperties["QUALIFIED"])
        {
            Debug.Log("qualified");
            qualifiedPlayer++;
        }
        if (qualifiedPlayer == quota)
        {
            if (!gameOver)
                finishGame();
        }

    }
}

