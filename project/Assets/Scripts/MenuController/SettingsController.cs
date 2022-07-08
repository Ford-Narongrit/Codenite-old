using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingsController : MonoBehaviour
{
    [Header("Scene")]
    [SerializeField] private string startMenu;

    
    // ******** OnClick ********
    public void OnClickBack()
    {
        SceneManager.LoadScene(startMenu);
    }
}
