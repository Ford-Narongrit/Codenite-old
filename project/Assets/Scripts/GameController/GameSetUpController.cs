using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using Photon.Pun;
using Photon.Realtime;
public class GameSetUpController : MonoBehaviour
{
    [Header("Player spawner")]
    [SerializeField] private GameObject playerPrefabs;
    [SerializeField] private Tilemap playerSpawnpoint;
    [SerializeField] private GameUIController gameUI;

    [Header("Monster spawner")]
    [SerializeField] private Tilemap[] monsterSpawnList;
    [SerializeField] private GameObject MonsterPrefab;
    [SerializeField] private int monsterInEachRoom = 6;

    [Header("Map setup")]
    [SerializeField] private Text[] itemNameText;
    [SerializeField] private string[] itemNameValue;
    void Start()
    {
        setCodeFragmentName();
        intiPlayer();
        if (PhotonNetwork.IsMasterClient)
        {
            SpawnMonster();
        }
    }
    private void intiPlayer()
    {
        Player playerPhoton = PhotonNetwork.LocalPlayer;
        int playerID = playerPhoton.ActorNumber;
        List<Vector3> availablePlaces = findLocationsOfTiles(playerSpawnpoint);
        Vector2 position = new Vector2(availablePlaces[playerID - 1].x + 0.5f, availablePlaces[playerID - 1].y + 0.5f);
        GameObject player = PhotonNetwork.Instantiate(playerPrefabs.name, position, Quaternion.identity);
        gameUI.setPlayer(player);

        if ((bool)playerPhoton.CustomProperties["ISSPECTATE"])
        {
            gameUI.setSpectator();
        }
        else
        {
            if ((bool)playerPhoton.CustomProperties["QUALIFIED"])
            {
                if (playerPhoton.CustomProperties["SKIN"] != null)
                {
                    player.GetComponent<PlayerController>().setPlayerColor(ColorString.GetColorFromString((string)playerPhoton.CustomProperties["SKIN_HAIR"]),
                                    ColorString.GetColorFromString((string)playerPhoton.CustomProperties["SKIN_HEAD"]),
                                    ColorString.GetColorFromString((string)playerPhoton.CustomProperties["SKIN_BODY"]),
                                    ColorString.GetColorFromString((string)playerPhoton.CustomProperties["SKIN_WEAPON"]));
                }
                PhotonNetwork.LocalPlayer.SetCustomProperties(MyCustomProperties.setGameOver(true));
            }
        }
    }

    private void SpawnMonster()
    {
        List<Vector3> availablePlaces;
        for (int n = 0; n < monsterSpawnList.Length; n++)
        {
            List<Vector3> Placed = new List<Vector3>();
            availablePlaces = findLocationsOfTiles(monsterSpawnList[n]);

            for (int i = 1; i <= monsterInEachRoom;)
            {
                int position = Random.Range(0, availablePlaces.Count);
                if (Placed.Contains(availablePlaces[position]) == false)
                {
                    Vector2 randomMonsterPosition = new Vector2(availablePlaces[position].x + 0.5f, availablePlaces[position].y + 0.5f);
                    GameObject monster = PhotonNetwork.InstantiateRoomObject(MonsterPrefab.name, randomMonsterPosition, Quaternion.identity);
                    monster.GetComponent<PhotonView>().RPC("setItem", RpcTarget.All, itemNameText[n].text);
                    Placed.Add(availablePlaces[position]);
                    i++;
                }
                else
                    continue;

                if (availablePlaces.Count < monsterInEachRoom)
                {
                    break;
                }
            }
        }
    }

    private void setCodeFragmentName()
    {
        itemNameValue = JsonReader.getMapInfo().itemArr;
        for (int i = 0; i < itemNameText.Length; i++)
        {
            itemNameText[i].text = itemNameValue[i];
        }
    }
    private List<Vector3> findLocationsOfTiles(Tilemap tileMap)
    {
        List<Vector3> availablePlaces = new List<Vector3>(); // create a new list of vectors by doing...
        for (int n = tileMap.cellBounds.xMin; n < tileMap.cellBounds.xMax; n++) // scan from left to right for tiles
        {
            for (int p = tileMap.cellBounds.yMin; p < tileMap.cellBounds.yMax; p++) // scan from bottom to top for tiles
            {
                Vector3Int localPlace = new Vector3Int(n, p, (int)tileMap.transform.position.y); // if you find a tile, record its position on the tile map grid
                Vector3 place = tileMap.CellToWorld(localPlace); // convert this tile map grid coords to local space coords
                if (tileMap.HasTile(localPlace))
                {
                    //Tile at "place"
                    availablePlaces.Add(place);
                }
            }
        }
        return availablePlaces;
    }
}
