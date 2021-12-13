using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public float moveSpeed;
    private Rigidbody2D rb;
    private GameObject player;
    private Vector3 direction;
    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        player = GameObject.FindWithTag("Player");
    }

    private void FixedUpdate() {
        if (Vector3.Distance(player.transform.position, transform.position) < 5f) {
            direction = player.transform.position - transform.position;
            direction.Normalize();
            rb.MovePosition(transform.position + (direction * moveSpeed * Time.deltaTime));
        }
    }
}
