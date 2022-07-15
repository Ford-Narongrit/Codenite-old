using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;

public class LeaderCell : MonoBehaviour
{
    [SerializeField] private Text playerName;
    [SerializeField] private Text status;

    public void setPlayerinfo(string name, bool isGameOver)
    {
        playerName.text = name;
        status.text = isGameOver + "";
    }
}
