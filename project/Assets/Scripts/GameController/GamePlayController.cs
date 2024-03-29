using System;
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
    [SerializeField] private Text quotaText;
    [Header("Scene")]
    [SerializeField] private string conclusionScene;
    [Header("Rule")]
    [SerializeField] private float gameTime = 300;
    [SerializeField] private float hintTime = 200;
    [SerializeField] private float quotaPercent;
    [SerializeField] private int minPlayerToFindWinner;

    [SerializeField] private CodePanelController codePanelController;

    private int quota = 1;
    private int qualifiedPlayer;
    private float currentTime;
    private bool gameOver = false;
    private bool isShowHint = false;

    private void Start()
    {
        myPhotonView = GetComponent<PhotonView>();
        int _qualifiedPlayer = qualifiedPlayerInPreLv();
        if (_qualifiedPlayer > minPlayerToFindWinner)
        {
            quota = (int)Mathf.Ceil(_qualifiedPlayer * (quotaPercent / 100));
        }
        else
        {
            quota = minPlayerToFindWinner;
        }
        currentTime = gameTime;
    }

    private void Update()
    {
        countDown();
    }

    private void LateUpdate()
    {
        updateQuotaText();
    }
    private void updateQuotaText()
    {
        quotaText.text = qualifiedPlayer + " / " + quota;
    }
    private void countDown()
    {
        currentTime -= Time.deltaTime;
        TimeSpan timeSpan = TimeSpan.FromSeconds(currentTime);
        string tempTimer = string.Format("{0:D2}:{1:D2}", timeSpan.Minutes, timeSpan.Seconds);
        timer.text = tempTimer;
        if (!isShowHint && currentTime <= gameTime - hintTime)
        {
            showHint();
        }
        if (currentTime <= 0f)
        {
            if (gameOver)
                return;
            finishGame();
        }
    }
    private void showHint()
    {
        codePanelController.showHint();
        isShowHint = true;
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
            }
        }
        return _qualifiedPlayer;
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if (!(bool)targetPlayer.CustomProperties["GAMEOVER"])
        {
            qualifiedPlayer++;
        }
        if (qualifiedPlayer == quota)
        {
            if (!gameOver)
                finishGame();
        }

    }
}

