using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SkeletonController : MonoBehaviour
{
    //SkeletonComponent//
    public Animator animator;
    public int maxHealth;
    public Rigidbody2D rb;
    private int currentHeath;

    //Shoot//
    public float bulletForce = 1f;
    public GameObject primaryBullet;
    public GameObject secondaryBullet;
    private bool alreadyAttacked = true;
    private float distance;

    //Movement//
    public NavMeshAgent agent;
    public float stoppingDistance;
    public float sightDistance;

    //OtherComponent//
    public GameObject material_2;

    private GameObject player;
    private Transform target;
    private Vector2 targetDir;


    // Start is called before the first frame update
    void Start()
    {
        currentHeath = maxHealth;
        StartCoroutine(cooldownAttack());
        player = GameObject.FindGameObjectWithTag("Player");

        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
		agent.updateRotation = false;
		agent.updateUpAxis = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null) {
            target = player.GetComponent<Transform>();
            distance = Vector2.Distance(transform.position, target.position);
            targetDir = (target.position - transform.position).normalized * -5f;

            if (distance < sightDistance){
                if (distance < stoppingDistance) {
                    agent.destination = targetDir;
                }
            }
        }

    }

    void FixedUpdate() {
        if (!alreadyAttacked && player && (distance < sightDistance)) {
            alreadyAttacked = true;
            StartCoroutine(cooldownAttack());
            animator.SetTrigger("Attack");

            targetDir.Normalize();

            int tmp = Random.Range(0, 3);
            if (tmp == 0) {
                GameObject bullet = Instantiate(secondaryBullet, transform.position, Quaternion.identity);
            } else {
                GameObject bullet = Instantiate(primaryBullet, transform.position, Quaternion.identity);
                Bullet bulletScript = bullet.GetComponent<Bullet>();

                bulletScript.velocity = targetDir * bulletForce * -1f;
                bulletScript.player = gameObject;
                float angle = Mathf.Atan2(targetDir.y, targetDir.x) * Mathf.Rad2Deg - 90f;
                bullet.transform.Rotate(0, 0, angle);
            }
        }
    }

    public void TakeDamage(int damage) {
        currentHeath -= damage;
        
        animator.SetTrigger("Hurt");

        if (currentHeath <= 0) {
            StartCoroutine(EnemyDie());
        }
    }

    private IEnumerator EnemyDie() {
        animator.SetBool("IsDead", true);
        yield return new WaitForSeconds(0.75f);
        int tmp = Random.Range(1, 101);
        if (tmp > 30) {
            for (int i = 0; i < 1; i++) {
                Instantiate(material_2, transform.position, Quaternion.identity);
            }
        }
        Destroy(this.gameObject);
    }

    private IEnumerator cooldownAttack() {
        // yield return new WaitForSeconds(1.5f + Random.Range(-1f, 3f));
        yield return new WaitForSeconds(0.5f);
        alreadyAttacked = false;
    }
}
