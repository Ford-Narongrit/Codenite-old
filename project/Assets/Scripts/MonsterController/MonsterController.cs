using UnityEngine;
using Photon.Pun;

public class MonsterController : MonoBehaviourPun, IPunObservable
{
    [Header("Monster Object")]
    [SerializeField] private MonsterModel monster;
    [SerializeField] private float respawnTime = 10f;
    private PhotonView view;
    private bool reset = false;
    private GameObject target;
    private void Start()
    {
        monster = GetComponent<MonsterModel>();
        view = GetComponent<PhotonView>();
    }
    private void Update()
    {
        if (!monster.Isdead())
        {
            GameObject target = monster.FindClosestTarget("Player");
            if (target && monster.isInMoveRange() && !reset)
            {
                monster.chasing(target.transform.position);
                if (monster.isInAttackRange(target.transform.position))
                {
                    monster.stop();
                    if(view.IsMine)
                        monster.attack();
                }
            }
            else
            {
                if (monster.isInSpawnPoint())
                {
                    // monster.patrol();
                    reset = false;
                }
                else
                {
                    monster.goToSpawnPoint();
                    reset = true;
                }
            }
        }
    }
    private void LateUpdate()
    {
        if (monster.Isdead())
        {
            gameObject.SetActive(false);
            Invoke("respawn", respawnTime);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(target);
        }
        else
        {
            target = (GameObject)stream.ReceiveNext();
        }
    }

    private void respawn()
    {
        monster.respawn();
        gameObject.SetActive(true);
    }

    // private void OnDrawGizmos()
    // {
    //     Gizmos.color = Color.white;
    //     UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.forward, monster.attackRadius);
    //     UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.forward, monster.chaseRadius);
    //     UnityEditor.Handles.DrawWireDisc(monster.spawnPosition, Vector3.forward, monster.moveRadius);
    // }
}
