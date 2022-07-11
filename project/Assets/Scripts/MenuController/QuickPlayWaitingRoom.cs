using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class QuickPlayWaitingRoom : MonoBehaviourPunCallbacks
{
    private PhotonView myPhotonView;

    [Header("Scene")]
    [SerializeField] private string gameScene;
    [SerializeField] private string menuScene;

    [Header("UI")]
    [SerializeField] private Text playerCountText;
    [SerializeField] private Text timerToStartText;

    [Header("Timer")]
    [SerializeField] private int minPlayerToStart;
    [SerializeField] private float maxWaitTime;
    [SerializeField] private float maxFullGameTime;

    private float timerToStartGame;
    private float notFullGameTimer;
    private float fullGameTimer;
    private int playerCount;
    private int roomSize;
    private bool readyToCountdown;
    private bool readyToStart;
    private bool startingGame;
    private Hashtable myCustomProperties = new Hashtable();



    private void Start()
    {
        myPhotonView = GetComponent<PhotonView>();
        fullGameTimer = maxFullGameTime;
        notFullGameTimer = maxWaitTime;
        timerToStartGame = maxWaitTime;

        PlayerCountUpdate();
        setPhotonProperties();
    }

    private void setPhotonProperties()
    {
        myCustomProperties["ISPLAY"] = true;
        myCustomProperties["QUALIFIED"] = true;
        myCustomProperties["TEAM"] = null;
        PhotonNetwork.LocalPlayer.SetCustomProperties(myCustomProperties);
    }
    private void Update()
    {
        WaitingForMorePlayer();
    }

    // ******** Private ********
    private void PlayerCountUpdate()
    {
        playerCount = PhotonNetwork.PlayerList.Length;
        roomSize = PhotonNetwork.CurrentRoom.MaxPlayers;
        playerCountText.text = playerCount + " / " + roomSize;
        if (playerCount == roomSize)
        {
            readyToStart = true;
        }
        else if (playerCount >= minPlayerToStart)
        {
            readyToCountdown = true;
        }
        else
        {
            readyToCountdown = false;
            readyToStart = false;
        }
    }

    private void WaitingForMorePlayer()
    {
        if (playerCount <= 1)
        {
            ResetTimer();
        }
        else if (readyToStart)
        {
            fullGameTimer -= Time.deltaTime;
            timerToStartGame = fullGameTimer;
        }
        else if (readyToCountdown)
        {
            notFullGameTimer -= Time.deltaTime;
            timerToStartGame = notFullGameTimer;
        }

        string tempTimer = string.Format("{0:00}", timerToStartGame);
        timerToStartText.text = tempTimer;

        if (timerToStartGame <= 0f)
        {
            if (startingGame)
                return;
            StartGame();
        }
    }

    private void ResetTimer()
    {
        timerToStartGame = maxWaitTime;
        notFullGameTimer = maxWaitTime;
        fullGameTimer = maxFullGameTime;
    }

    private void StartGame()
    {
        startingGame = true;
        if (!PhotonNetwork.IsMasterClient)
            return;
        Debug.Log("Starting Game . . .");
        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.LoadLevel(gameScene);
    }

    // ******** Callsback ********
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        PlayerCountUpdate();

        if (PhotonNetwork.IsMasterClient)
            myPhotonView.RPC("RPC_SendTimer", RpcTarget.Others, timerToStartGame);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        PlayerCountUpdate();
    }

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(menuScene);
    }

    [PunRPC]
    private void RPC_SendTimer(float timeIn)
    {
        timerToStartGame = timeIn;
        notFullGameTimer = timeIn;
        if (timeIn < fullGameTimer)
        {
            fullGameTimer = timeIn;
        }
    }

    // ******** OnClick ********
    public void OnClickCancel()
    {
        PhotonNetwork.LeaveRoom();
    }

    public void OnCilckStart()
    {
        StartGame();
    }
}