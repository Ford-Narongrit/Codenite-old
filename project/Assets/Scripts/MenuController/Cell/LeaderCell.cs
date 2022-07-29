using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;

public class LeaderCell : MonoBehaviour
{
    [SerializeField] private Text playerName;
    [SerializeField] private Image image;
    [SerializeField] private Color passColor;
    [SerializeField] private Color failColor;
    public void setPlayerinfo(string name, bool isGameOver)
    {
        playerName.text = name;
        
        if(isGameOver)
        {
            image.color = failColor;
        }
        else
        {
            image.color = passColor;
        }
    }
}
