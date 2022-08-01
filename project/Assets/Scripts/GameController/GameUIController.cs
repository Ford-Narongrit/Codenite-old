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
    [SerializeField] private GameObject howtoShoot;
    [SerializeField] private GameObject howtoSpectate;

    private GameObject player;

    private int playerSpectatateIndex = 0;
    private bool isSpectate = false;
    private void Update()
    {
        if (!isSpectate)
        {
            if (Input.GetKeyDown(KeyCode.M) || Input.GetKeyDown(KeyCode.Tab))
            {
                worldMap.SetActive(true);
            }
            if (Input.GetKeyUp(KeyCode.M) || Input.GetKeyUp(KeyCode.Tab))
            {
                worldMap.SetActive(false);
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                //TODO fix bug if close codePanel it get error because can't find gameobject
                // codePanel.SetActive(!codePanel.activeSelf);
            }
        }
        else
        {
            howtoSpectate.SetActive(true);
            howtoShoot.SetActive(false);
            if (Input.GetKeyDown(KeyCode.E))
            {
                cameraLookAt(getPlayerToSpectatate(1));
            }
            if (Input.GetKeyDown(KeyCode.Q))
            {
                cameraLookAt(getPlayerToSpectatate(-1));
            }
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            menu.SetActive(!menu.activeSelf);
        }
    }

    private void LateUpdate()
    {
        smileBar.SetValue(playerController.currentChargeTime);
    }

    public void setPlayer(GameObject newPlayer)
    {
        player = newPlayer;
        cameraLookAt(player.transform);

        playerController = newPlayer.GetComponent<PlayerController>();
        playerController.setCamera(playerCamera);

        smileBar.SetMaxValue(playerController.chargeToFireTime);
    }

    public void setSpectator()
    {
        player.GetPhotonView().RPC("hide", RpcTarget.All);
        isSpectate = true;
        worldMap.SetActive(false);
        codePanel.SetActive(false);

        cameraLookAt(getPlayerToSpectatate(0));
    }

    private Transform getPlayerToSpectatate(int _index)
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        if (players.Length <= 0)
        {
            return null;
        }

        playerSpectatateIndex = playerSpectatateIndex + _index;
        if (playerSpectatateIndex < 0)
            playerSpectatateIndex = players.Length - 1;
        if (playerSpectatateIndex >= players.Length)
            playerSpectatateIndex = 0;

        return players[playerSpectatateIndex].transform;
    }

    private void cameraLookAt(Transform _player)
    {
        if (_player == null)
        {
            return;
        }
        
        playerCamera.GetComponent<CameraFollow>().SetTarget(_player.transform);
        minimapCamera.GetComponent<CameraFollow>().SetTarget(_player.transform);
    }

    public void returnToStartMenu()
    {
        ConfirmUIController.Instance.showQuestion("Do you want to leave this game ?",
        () =>
        {
            PhotonNetwork.LeaveRoom();
        },
        () =>
        {
            // Do nothing
        }
    );

    }

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(menuScene);
    }
}
