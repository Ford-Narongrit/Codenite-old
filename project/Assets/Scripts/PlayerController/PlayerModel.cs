using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerModel : MonoBehaviour
{
    [SerializeField] private PhotonView view;
    [Header("Object Info")]
    [SerializeField] private Transform body;
    [SerializeField] private Transform aim;
    [SerializeField] private GameObject bulletPrefab;

    [Header("Character Info")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] public float maxHealth = 100f;
    public float currentHealth;
    public float currentSpeed;
    public Vector2 spawnPoint;

    private void Start()
    {
        currentHealth = maxHealth;
        currentSpeed = moveSpeed;

        view = GetComponent<PhotonView>();
    }

    public void movement(Vector2 movement)
    {
        Vector3 newMovement = Time.deltaTime * currentSpeed * movement.normalized;
        transform.position += newMovement;
    }
    public void Rotation(Vector2 mousePos)
    {
        Vector2 lookDir = mousePos - (Vector2)body.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg + 90f;
        body.eulerAngles = Vector3.forward * angle;
    }

    [PunRPC]
    public void takeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth < 0)
        {
            currentHealth = 0;
        }
    }
    public void attack()
    {
        GameObject bullet = PhotonNetwork.Instantiate(bulletPrefab.name, aim.position, aim.rotation);
        bullet.GetComponent<Bullet>().setOwnerViewID(view.ViewID);
    }
    public void respawn()
    {
        goSpawnPoint();
        currentHealth = maxHealth;
    }

    public void goSpawnPoint()
    {
        transform.position = spawnPoint;
    }

    public bool isDead()
    {
        return currentHealth <= 0;
    }

    public void slow(float slowPercent)
    {
        currentSpeed = currentSpeed - (currentSpeed * slowPercent);
    }

    public void resetSpeed(){
        currentSpeed = moveSpeed;
    }
}
