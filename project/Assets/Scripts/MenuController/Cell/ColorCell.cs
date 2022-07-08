using UnityEngine;
using UnityEngine.UI;

public class ColorCell : MonoBehaviour
{
    [SerializeField] public GameObject coverObject;
    [SerializeField] public Color32 cover;
    [SerializeField] public Color32 head;
    [SerializeField] public Color32 hair;
    [SerializeField] public Color32 body;
    [SerializeField] public Color32 weapon;

    public void setCover(Color32 _cover)
    {
        cover = _cover;
        coverObject.GetComponent<Image>().color = cover;
    }

    public void setInfo(Color32 _head, Color32 _hair, Color32 _body, Color32 _weapon)
    {
        head = _head;
        hair = _hair;
        body = _body;
        weapon = _weapon;
    }
}
