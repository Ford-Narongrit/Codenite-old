using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;

public class RoomCell : MonoBehaviour
{
    public Text roomName;
    public Text roomMode;
    public Text playerCount;
    private CustomController manager;
    
    private void Start() {
        manager = FindObjectOfType<CustomController>();
    }

    public void setRoomInfo(RoomInfo room)
    {
        roomName.text = room.Name;
        roomMode.text = (string)room.CustomProperties["MODE"];
        playerCount.text = room.PlayerCount + "/" + room.MaxPlayers;
    }

    public void OnClickCell()
    {
        manager.OnClickJoinWithRoomCell(roomName.text);
    }
}
