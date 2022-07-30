using UnityEngine;
using UnityEngine.UI;

public class ColorCell : MonoBehaviour
{
    [SerializeField] public GameObject coverObject;
    [SerializeField] public Color32 cover;

    public void setCover(Color32 _cover)
    {
        cover = _cover;
        coverObject.GetComponent<Image>().color = cover;
    }
}
