using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class CharacterCustomController : MonoBehaviour
{
    [Header("Scene")]
    [SerializeField] private string startMenu;

    [Header("character")]
    [SerializeField] private SpriteRenderer hair;
    [SerializeField] private SpriteRenderer head;
    [SerializeField] private SpriteRenderer body;
    [SerializeField] private SpriteRenderer weapon;
    [Header("UI")]
    [SerializeField] private GameObject[] panels;

    void Start()
    {
        Player player = PhotonNetwork.LocalPlayer;
        if (player.CustomProperties["SKIN"] != null)
        {
            setPlayerColor(ColorString.GetColorFromString((string)player.CustomProperties["SKIN_HAIR"]),
                            ColorString.GetColorFromString((string)player.CustomProperties["SKIN_HEAD"]),
                            ColorString.GetColorFromString((string)player.CustomProperties["SKIN_BODY"]),
                            ColorString.GetColorFromString((string)player.CustomProperties["SKIN_WEAPON"]));
        }
        else
        {
            setDefaultColor();
        }
    }
    public void setPlayerColor(Color _hair, Color _head, Color _body, Color _weapon)
    {
        hair.color = _hair;
        head.color = _head;
        body.color = _body;
        weapon.color = _weapon;
    }

    public void setDefaultColor()
    {
        Player player = PhotonNetwork.LocalPlayer;
        player.SetCustomProperties(MyCustomProperties.setColorProperties(Color.white, Color.white, Color.white, Color.white));
    }

    // ******** OnClick ********
    public void OnClickBack()
    {
        SceneManager.LoadScene(startMenu);
    }

    public void OnClickPart(GameObject _panel)
    {
        foreach (GameObject panel in panels)
        {
            if (panel.Equals(_panel))
            {
                panel.SetActive(true);
            }
            else
            {
                panel.SetActive(false);
            }
        }
    }
}
