using System;
using UnityEngine;
using UnityEngine.UI;

public class AlertController : MonoBehaviour
{
    public static AlertController Instance { get; private set; }
    [SerializeField] private Text title;
    [SerializeField] private Text content;
    [SerializeField] private Button btn;
    private void Awake()
    {
        Instance = this;
        Hide();
    }

    public void showAlert(string _title, string _content, string _btnText, Action onClick)
    {
        gameObject.SetActive(true);
        btn.onClick.RemoveAllListeners();

        title.text = _title;
        content.text = _content;
        btn.GetComponentInChildren<Text>().text = _btnText;
        btn.onClick.AddListener(() =>
        {
            Hide();
            onClick();
        });
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
