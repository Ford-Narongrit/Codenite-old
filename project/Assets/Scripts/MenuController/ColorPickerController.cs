using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
public class ColorPickerController : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Transform colorPickerContrainer;
    [SerializeField] private GameObject colorPickerPrefab;

    [Header("info")]
    [SerializeField] private Color32[] colorList;
    [Header("character")]
    [SerializeField] private SpriteRenderer hair;
    [SerializeField] private SpriteRenderer head;
    [SerializeField] private SpriteRenderer body;
    [SerializeField] private SpriteRenderer weapon;
    private Hashtable myCustomProperties = new Hashtable();

    void Start()
    {
        foreach (var color in colorList)
        {
            GameObject colorPicker = GameObject.Instantiate(colorPickerPrefab, colorPickerContrainer);
            ColorCell colorCell = colorPicker.GetComponentInChildren<ColorCell>();
            colorCell.setCover(color);
            colorCell.setInfo(color, color, color, color);

            colorPicker.GetComponentInChildren<Button>().onClick.AddListener(delegate { OnClickColorCell(colorCell); });
        }
        Player player = PhotonNetwork.LocalPlayer;
        if (player.CustomProperties["SKIN_HAIR"] != null)
        {
            setPlayerColor(ColorString.GetColorFromString((string)player.CustomProperties["SKIN_HAIR"]),
                            ColorString.GetColorFromString((string)player.CustomProperties["SKIN_HEAD"]),
                            ColorString.GetColorFromString((string)player.CustomProperties["SKIN_BODY"]),
                            ColorString.GetColorFromString((string)player.CustomProperties["SKIN_WEAPON"]));
        }
    }

    public void OnClickColorCell(ColorCell _colorCell)
    {
        setPlayerColor(_colorCell.hair, _colorCell.head, _colorCell.body, _colorCell.weapon);
        setPhotonPlayerColor(_colorCell.hair,
                        _colorCell.head,
                        _colorCell.body,
                        _colorCell.weapon);
    }

    public void setPlayerColor(Color _hair, Color _head, Color _body, Color _weapon)
    {
        hair.color = _hair;
        head.color = _head;
        body.color = _body;
        weapon.color = _weapon;
    }

    public void setPhotonPlayerColor(Color _hair, Color _head, Color _body, Color _weapon)
    {
        Player player = PhotonNetwork.LocalPlayer;
        myCustomProperties["SKIN_HAIR"] = ColorString.GetStringFromColor(_hair);
        myCustomProperties["SKIN_HEAD"] = ColorString.GetStringFromColor(_head);
        myCustomProperties["SKIN_BODY"] = ColorString.GetStringFromColor(_body);
        myCustomProperties["SKIN_WEAPON"] = ColorString.GetStringFromColor(_weapon);
        player.SetCustomProperties(myCustomProperties);
    }
}
