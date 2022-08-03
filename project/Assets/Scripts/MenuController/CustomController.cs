using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class CustomController : MonoBehaviourPunCallbacks
{
    [Header("Scene")]
    [SerializeField] private string customWaitingRoomSolo;
    [SerializeField] private string customWaitingRoomTeam;
    [SerializeField] private string startMenu;

    [Header("UI")]
    [SerializeField] private GameObject[] modeBtn;
    [SerializeField] private Button joinBtn;
    [SerializeField] private InputField joinInputField;
    [SerializeField] private RoomCell roomCellPrefab;
    [SerializeField] private Transform contentObject;

    [Header("Room Option")]
    [SerializeField] private int RoomCodeLength = 6;
    [SerializeField] private int RoomSize = 20;
    private RoomOptions roomOptions = new RoomOptions();

    private TypedLobby custom = new TypedLobby("custom", LobbyType.Default);
    private List<RoomCell> roomCellList = new List<RoomCell>();
    private float timeBetweenUpdates = 1.5f;
    private float nextUpdateTime = 0f;
    private string mode = "SOLO";
    private void Start()
    {
        //roomOption setup
        roomOptions.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable();
        roomOptions.CustomRoomProperties["MODE"] = "SOLO";
        roomOptions.CustomRoomProperties["PVP"] = false;
        roomOptions.CustomRoomProperties["MEMBER"] = 1;
        roomOptions.CustomRoomProperties["PROBLEMINDEX"] = 0;

        PhotonNetwork.JoinLobby(custom);
        joinInputField.characterLimit = RoomCodeLength;

        for (int i = 0; i < modeBtn.Length; i++)
        {
            modeBtn[i].GetComponent<Image>().color = Color.gray;
        }
        modeBtn[0].GetComponent<Image>().color = Color.green;
    }
    private void Update()
    {
        if (joinInputField.text.Length == RoomCodeLength)
        {
            joinBtn.interactable = true;
        }
    }

    // ******** OnChange *******
    public void OnChangeInputJoin()
    {
        joinInputField.text = joinInputField.text.ToUpper();
    }

    // ******** OnClick ********
    public void OnClickMode(GameObject btn)
    {
        for (int i = 0; i < modeBtn.Length; i++)
        {
            if (btn.Equals(modeBtn[i]))
            {
                modeBtn[i].GetComponent<Image>().color = Color.green;
            }
            else
            {
                modeBtn[i].GetComponent<Image>().color = Color.gray;
            }
        }
    }
    public void OnClickMode(string _mode)
    {
        roomOptions.CustomRoomProperties["MODE"] = _mode;
        mode = _mode;
    }
    public void OnClickMode(int playerInTeam)
    {
        roomOptions.CustomRoomProperties["MEMBER"] = playerInTeam;
    }
    public void OnClickCreate()
    {
        CreateRoom();
    }
    public void OnClickJoinWithRoomCell(string roomCode)
    {
        PhotonNetwork.JoinRoom(roomCode);
    }
    public void OnClickJoin()
    {
        PhotonNetwork.JoinRoom(joinInputField.text);
    }
    public void OnClickLeave()
    {
        PhotonNetwork.LeaveLobby();
    }

    // ******** Callsback ********
    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        Debug.Log("connect to " + PhotonNetwork.CurrentLobby.Name + "Lobby.");
    }
    public override void OnLeftLobby()
    {
        base.OnLeftLobby();
        SceneManager.LoadScene(startMenu);
    }
    public override void OnJoinedRoom()
    {
        if (mode == "SOLO")
        {
            SceneManager.LoadScene(customWaitingRoomSolo);
        }
        else
        {
            SceneManager.LoadScene(customWaitingRoomTeam);
        }
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);
        AlertController.Instance.showAlert("", "Could not find the game you're looking for.", "close", () =>
                {
                    joinInputField.text = "";
                });
        PhotonNetwork.JoinLobby(custom);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        if (Time.time >= nextUpdateTime)
        {
            UpdateRoomList(roomList);
            nextUpdateTime = Time.time + timeBetweenUpdates;
        }
    }

    // ******** Private ********
    private void CreateRoom()
    {
        roomOptions.MaxPlayers = (byte)RoomSize;

        string randomRoomNumber = RandomRoomCode(RoomCodeLength);
        PhotonNetwork.CreateRoom(randomRoomNumber, roomOptions);
        Debug.Log("room code is " + randomRoomNumber);
    }

    private string RandomRoomCode(int range)
    {
        string characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        string roomCode = "";

        for (int i = 0; i < range; i++)
        {
            roomCode += characters[Random.Range(0, characters.Length)];
        }
        return roomCode;
    }

    private void UpdateRoomList(List<RoomInfo> list)
    {
        foreach (RoomCell item in roomCellList)
        {
            Destroy(item.gameObject);
        }
        roomCellList.Clear();

        foreach (RoomInfo room in list)
        {
            RoomCell newRoom = Instantiate(roomCellPrefab, contentObject);
            newRoom.setRoomInfo(room);
            roomCellList.Add(newRoom);
        }
    }
}
