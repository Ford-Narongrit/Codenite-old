using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class QuickPlayController : MonoBehaviourPunCallbacks
{
    [Header("Scene")]
    [SerializeField] private string quickPlayWaitingRoom;
    [SerializeField] private string startMenu;

    [Header("Game Option")]
    [SerializeField] private int RoomSize;

    private TypedLobby quickPlay = new TypedLobby("QUICKPLAY", LobbyType.Default);

    private void Start()
    {
        PhotonNetwork.JoinLobby(quickPlay);
    }

    // ******** OnClick ********
    public void OnClickFindMatch()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    public void OnClickLeaveMatch()
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
        SceneManager.LoadScene(quickPlayWaitingRoom);
    }
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        CreateRoom();
    }
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("failed to crate room ... trying again");
        CreateRoom();
    }

    // ******** Private ********
    private void CreateRoom()
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable();
        roomOptions.CustomRoomProperties["MODE"] = "SOLO";
        roomOptions.CustomRoomProperties["PVP"] = false;
        roomOptions.CustomRoomProperties["MEMBER"] = 1;
        roomOptions.CustomRoomProperties["PROBLEMINDEX"] = 0;
        roomOptions.MaxPlayers = (byte)RoomSize;

        string randomRoomNumber = RandomRoomCode(5);
        PhotonNetwork.CreateRoom(randomRoomNumber, roomOptions);

        Debug.Log("room code is " + randomRoomNumber);
    }

    private string RandomRoomCode(int range)
    {
        string characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        string roomCode = "QUICKPLAY";

        for (int i = 0; i < range; i++)
        {
            roomCode += characters[Random.Range(0, characters.Length)];
        }
        return roomCode;
    }
}
