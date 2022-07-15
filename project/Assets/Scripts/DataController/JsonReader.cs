using UnityEngine;
using Photon.Pun;

public class JsonReader : MonoBehaviour
{
    public TextAsset textJson;
    public static Maps mapsJson;

    [System.Serializable]
    public class MapInfo
    {
        public string problem;
        public string result;
        public string[] itemArr;
        public string[] correntAnsArr;
    };

    [System.Serializable]
    public class Maps
    {
        public MapInfo[] maps;
    }


    private void Awake() {
        mapsJson = JsonUtility.FromJson<Maps>(textJson.text);
    }

    public static MapInfo getMapInfo()
    {
        int index = (int)PhotonNetwork.CurrentRoom.CustomProperties["PROBLEMINDEX"];
        return mapsJson.maps[index];
    }

}
