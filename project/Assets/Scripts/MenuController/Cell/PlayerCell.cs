using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class PlayerCell : MonoBehaviour
{
    [SerializeField] private Text playerName;

    public void setPlayerName(Player _player)
    {
        playerName.text = _player.NickName;
    }
}
