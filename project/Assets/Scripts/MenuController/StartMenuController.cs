using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class StartMenuController : MonoBehaviourPunCallbacks
{
    [Header("Scene")]
    [SerializeField] private string quickPlayScene;
    [SerializeField] private string customScene;
    [SerializeField] private string settingScene;
    [SerializeField] private string connectToServerScene;

    [Header("UI")]
    [SerializeField] private Text playerName;

    void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        playerName.text = PhotonNetwork.NickName;
    }

    // ******** OnClick ********
    public void OnClickQuickPlay()
    {
        SceneManager.LoadScene(quickPlayScene);
    }
    public void OnClickCustom()
    {
        SceneManager.LoadScene(customScene);
    }

    public void OnCilckSetting()
    {
        SceneManager.LoadScene(settingScene);
    }
    public void OnclickExit()
    {
        PhotonNetwork.Disconnect();
    }

    // ******** Callsback ********
    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log(PhotonNetwork.NickName + " has disconnected ");
        SceneManager.LoadScene(connectToServerScene);
    }
}
