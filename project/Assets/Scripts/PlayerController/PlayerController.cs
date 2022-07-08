using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class PlayerController : MonoBehaviourPun, IPunObservable
{
    [Header("Player Object")]
    [SerializeField] private PlayerModel player;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private Text playerName;
    [SerializeField] private GaugeBar healthBar;
    [SerializeField] private float respawnTime = 10f;

    [Header("Player Skin")]
    [SerializeField] private SpriteRenderer hair;
    [SerializeField] private SpriteRenderer head;
    [SerializeField] private SpriteRenderer body;
    [SerializeField] private SpriteRenderer weapon;

    private Vector2 movement = Vector2.zero;
    private Vector2 mousePos = Vector2.zero;
    private PhotonView view;

    private void Start()
    {
        player = GetComponent<PlayerModel>();
        view = GetComponent<PhotonView>();
        healthBar.SetMaxValue(player.maxHealth);
        playerName.text = view.Owner.NickName;
    }
    private void Update()
    {
        if (view.IsMine)
        {
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");
            mousePos = playerCamera.ScreenToWorldPoint(Input.mousePosition);
            if (Input.GetButtonDown("Fire1"))
            {
                player.attack();
            }
        }
    }

    private void FixedUpdate()
    {
        healthBar.SetValue(player.currentHealth);
        if (view.IsMine)
        {
            player.movement(movement);
            player.Rotation(mousePos);
        }
        else
        {
            player.Rotation(mousePos);
        }
    }
    private void LateUpdate()
    {
        if (player.isDead())
        {
            gameObject.SetActive(false);
            if (view.IsMine)
                Invoke("respawn", respawnTime);
        }
    }

    public void setCamera(Camera camera)
    {
        playerCamera = camera;
    }
    public void setPlayerColor(Color _hair, Color _head, Color _body, Color _weapon)
    {
        hair.color = _hair;
        head.color = _head;
        body.color = _body;
        weapon.color = _weapon;
    }

    private void respawn()
    {
        player.respawn();
        gameObject.SetActive(true);
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(mousePos);
        }
        else
        {
            mousePos = (Vector2)stream.ReceiveNext();
        }
    }
}
