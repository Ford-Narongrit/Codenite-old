using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterCustomController : MonoBehaviour
{
    [Header("Scene")]
    [SerializeField] private string startMenu;

    
    // ******** OnClick ********
    public void OnClickBack()
    {
        SceneManager.LoadScene(startMenu);
    }
}
