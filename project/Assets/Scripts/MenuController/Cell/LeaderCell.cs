using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;

public class LeaderCell : MonoBehaviour
{
    [SerializeField] private Text playerName;
    [SerializeField] private Text status;

    public void setPlayerinfo(Player _player)
    {
        playerName.text = _player.NickName;
        status.text = (bool)_player.CustomProperties["QUALIFIED"] + "";
    }
}
