using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
public class ColorPickerController : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Transform colorPickerContrainer;
    [SerializeField] private GameObject colorPickerPrefab;

    [Header("info")]
    [SerializeField] private Color32[] colorList;

    [Header("character")]
    [SerializeField] private string ColorPropertiesKey;
    [SerializeField] private SpriteRenderer characterPart;


    void init()
    {
        foreach (var color in colorList)
        {
            GameObject colorPicker = GameObject.Instantiate(colorPickerPrefab, colorPickerContrainer);
            ColorCell colorCell = colorPicker.GetComponentInChildren<ColorCell>();
            colorCell.setCover(color);

            colorPicker.GetComponentInChildren<Button>().onClick.AddListener(delegate { OnClickColorCell(colorCell); });
        }
    }

    void Start()
    {
        init();
    }

    public void OnClickColorCell(ColorCell _colorCell)
    {
        setPlayerColor(_colorCell.cover);
        setPhotonPlayerColor(_colorCell.cover);
    }

    public void setPlayerColor(Color _color)
    {
        characterPart.color = _color;
    }

    public void setPhotonPlayerColor(Color _color)
    {
        Player player = PhotonNetwork.LocalPlayer;
        player.SetCustomProperties(MyCustomProperties.setColor(ColorPropertiesKey, _color));
    }
}
