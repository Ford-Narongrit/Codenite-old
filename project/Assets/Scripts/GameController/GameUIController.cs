using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class GameUIController : MonoBehaviourPunCallbacks
{
    [Header("Scence")]
    [SerializeField] private string menuScene;

    [Header("Player")]
    [SerializeField] private PlayerController playerController;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private Camera minimapCamera;

    [Header("UI")]
    [SerializeField] private GaugeBar smileBar;
    [SerializeField] private GameObject worldMap;
    [SerializeField] private GameObject menu;
    [SerializeField] private GameObject codePanel;

    private Transform player;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M) || Input.GetKeyDown(KeyCode.Tab))
        {
            worldMap.SetActive(true);
        }
        if (Input.GetKeyUp(KeyCode.M) || Input.GetKeyUp(KeyCode.Tab))
        {
            worldMap.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            menu.SetActive(!menu.activeSelf);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            codePanel.SetActive(!codePanel.activeSelf);
        }
    }

    public void setPlayer(GameObject newPlayer)
    {
        player = newPlayer.transform;
        playerCamera.GetComponent<CameraFollow>().SetTarget(player);
        minimapCamera.GetComponent<CameraFollow>().SetTarget(player);

        playerController = newPlayer.GetComponent<PlayerController>();
        playerController.setCamera(playerCamera);
    }
    public void returnToStartMenu()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(menuScene);
    }
}
