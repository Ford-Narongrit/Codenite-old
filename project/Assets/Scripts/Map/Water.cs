using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
    [SerializeField] private float slow = 0.2f;
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<PlayerModel>().slow(slow);
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if(other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<PlayerModel>().resetSpeed();
        }
    }
}
