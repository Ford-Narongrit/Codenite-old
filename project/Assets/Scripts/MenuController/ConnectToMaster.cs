using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class ConnectToMaster : MonoBehaviourPunCallbacks
{
    [Header("Menu")]
    [SerializeField] private InputField usernameInput;
    [SerializeField] private InputField passwordInput;
    [SerializeField] private Button buttonBtn;
    [SerializeField] private GameObject loading;

    [Header("Scene")]
    [SerializeField] private string nextSceneName;
    
    private void Update() {
        if(usernameInput.text.Length > 0 && passwordInput.text.Length > 0)
        {
            buttonBtn.interactable = true;
        }
        else
        {
            buttonBtn.interactable = false;
        }
    }

    private void login()
    {
        Debug.Log("login success");
    }

    public void OnClickConnect(){

        login();

        PhotonNetwork.NickName = usernameInput.text;
        loading.SetActive(true);
        PhotonNetwork.ConnectUsingSettings();
    }

    public void OnCilckRegister()
    {
        Debug.Log("go register");
    }

    public override void OnConnectedToMaster()
    {
        SceneManager.LoadScene(nextSceneName);
    }
}
