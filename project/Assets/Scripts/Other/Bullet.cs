using UnityEngine;
using Photon.Pun;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float bulletDamage;
    [SerializeField] private float destroyTime;
    [SerializeField] private float moveSpeed;
    [SerializeField] private Vector2 bulletDir;
    [SerializeField] private string ally;
    private PhotonView view;
    private int ownerViewID;
    private bool isPvpOn = false;
    void Awake()
    {
        view = GetComponent<PhotonView>();
        isPvpOn = (bool)PhotonNetwork.CurrentRoom.CustomProperties["PVP"];
        Destroy(gameObject, destroyTime);
    }

    void Update()
    {
        transform.Translate(bulletDir * moveSpeed * Time.deltaTime);
    }

    public void setOwnerViewID(int viewID)
    {
        ownerViewID = viewID;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (view.IsMine)
        {
            if(ally == other.gameObject.tag && !isPvpOn)
            {
                Destroy(gameObject);
                return;
            }
            if (other.gameObject.tag == "Monster")
            {
                other.gameObject.GetComponent<PhotonView>().RPC("takeDamage", RpcTarget.All, bulletDamage);
                if (other.gameObject.GetComponent<MonsterModel>().Isdead())
                {
                    PlayerModel player = PhotonView.Find(ownerViewID).GetComponent<PlayerModel>();
                    GameObject.FindWithTag("CodePanel").GetComponent<CodePanelController>().pickItem(other.gameObject.GetComponent<MonsterModel>().dropItem());
                    player.goSpawnPoint();
                }
                Destroy(gameObject);
            }
            else if (other.gameObject.tag == "Player")
            {
                other.gameObject.GetComponent<PhotonView>().RPC("takeDamage", RpcTarget.All, bulletDamage);
                Destroy(gameObject);
            }

            else if (other.gameObject.tag == "Wall")
            {
                Destroy(gameObject);
            }
        }
    }
}