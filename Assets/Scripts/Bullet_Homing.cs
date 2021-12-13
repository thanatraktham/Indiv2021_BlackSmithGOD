using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Homing : MonoBehaviour
{
    public float speed;
    public float stoppingDistance;
    public GameObject hitEffect;

    private Transform target;
    private float distance;
    private GameObject player;
    private Vector2 targetDir;

    void Start(){
      player = GameObject.FindGameObjectWithTag("Player");
      Destroy(this.gameObject, 5f);
    }

    void Update(){
        if (player != null) {
            target = player.GetComponent<Transform>();
            distance = Vector2.Distance(transform.position, target.position);
            targetDir = (target.position - transform.position).normalized;

            if (distance > stoppingDistance) {
                transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
            }
        }

    }

    void FixedUpdate() {
        if (player != null) {
            transform.position = Vector2.MoveTowards(transform.position, target.position, 5 * speed * Time.deltaTime);
        }

        targetDir.Normalize();
        float angle = Mathf.Atan2(targetDir.y, targetDir.x) * Mathf.Rad2Deg - 90f;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }
    
    private void OnCollisionEnter2D(Collision2D other) {
        if (player) {
            if (!other.gameObject.CompareTag("Skeleton")) {
                DestroyHomingBullet();
            }
        }
    }

    public void DestroyHomingBullet() {
        GameObject effect = Instantiate(hitEffect, transform.position, Quaternion.identity);
        Destroy(effect, 0.5f);
        Destroy(gameObject);
    }
}
