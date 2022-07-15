using UnityEngine;
using Photon.Pun;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float bulletDamage;
    [SerializeField] private float destroyTime;
    [SerializeField] private float moveSpeed;
    [SerializeField] private Vector2 bulletDir;
    private PhotonView view;
    private int ownerViewID;
    void Awake()
    {
        view = GetComponent<PhotonView>();
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (view.IsMine)
        {
            if (other.gameObject.tag == "Monster")
            {
                other.GetComponent<PhotonView>().RPC("takeDamage", RpcTarget.All, bulletDamage);
                if (other.GetComponent<MonsterModel>().Isdead())
                {
                    PlayerModel player = PhotonView.Find(ownerViewID).GetComponent<PlayerModel>();
                    GameObject.FindWithTag("CodePanel").GetComponent<CodePanelController>().pickItem(other.GetComponent<MonsterModel>().dropItem());
                    player.goSpawnPoint();
                }
            }
            else if (other.gameObject.tag == "Player")
            {
                other.GetComponent<PhotonView>().RPC("takeDamage", RpcTarget.All, bulletDamage);
            }
        }
        Destroy(gameObject);
    }
}