using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class PlayerController : MonoBehaviourPun, IPunObservable
{
    [Header("Player Object")]
    [SerializeField] private PlayerModel player;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private Text playerName;
    [SerializeField] private Text timerText;
    [SerializeField] private GaugeBar healthBar;
    [SerializeField] private float respawnTime = 10f;
    [SerializeField] public float chargeToFireTime = 10f;

    [Header("Player Skin")]
    [SerializeField] private SpriteRenderer hair;
    [SerializeField] private SpriteRenderer head;
    [SerializeField] private SpriteRenderer body;
    [SerializeField] private SpriteRenderer weapon;

    private Vector2 movement = Vector2.zero;
    private Vector2 mousePos = Vector2.zero;
    private PhotonView view;
    private float currentRespawnTime = 0f;
    public float currentChargeTime = 0f;
    public bool canAttack;

    private void Start()
    {
        player = GetComponent<PlayerModel>();
        view = GetComponent<PhotonView>();
        healthBar.SetMaxValue(player.maxHealth);
        playerName.text = view.Owner.NickName;
    }
    private void Update()
    {
        if (view.IsMine && !player.isDead())
        {
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");
            mousePos = playerCamera.ScreenToWorldPoint(Input.mousePosition);
            if (Input.GetButton("Fire1"))
            {
                currentChargeTime += Time.deltaTime;
                if (currentChargeTime >= chargeToFireTime)
                {
                    player.attack();
                    currentChargeTime = 0f;
                }
            }
            if (Input.GetButtonUp("Fire1"))
            {
                currentChargeTime = 0f;
            }
        }
    }

    private void FixedUpdate()
    {
        healthBar.SetValue(player.currentHealth);
        if (view.IsMine && !player.isDead())
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
            timerText.gameObject.SetActive(true);
            player.setDead(true);

            currentRespawnTime += Time.deltaTime;
            timerText.text = currentRespawnTime.ToString();
            if (currentRespawnTime >= respawnTime)
            {
                if (view.IsMine)
                    view.RPC("respawn", RpcTarget.All);
                timerText.gameObject.SetActive(false);
                currentRespawnTime = 0f;
            }
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
