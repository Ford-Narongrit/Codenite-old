using UnityEngine;
using Photon.Pun;

public class Zone : MonoBehaviour
{
    public WarpGateController warp;
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("player out zone");
            warp.view.RPC("warpOut", RpcTarget.All);
        }
    }
}
