using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SlimeController : MonoBehaviour
{
    //SlimeComponent//
    public Animator animator;
    public int maxHealth;
    public Rigidbody2D rb;
    private int currentHeath;

    //Dash//
    public float dashSpeed;
    private bool isDashing;
    private bool alreadyAttacked;
    private float currentDashTimer;
    private float distance;
    private float enemyKnockbackForce;
    
    //Movement//
    public float stoppingDistance;
    public NavMeshAgent agent;
    public float sightDistance;

    //OtherComponent//
    public GameObject material_1;
    public GameObject material_3;
    private GameObject player;
    private Transform target;
    private Vector3 lastTargetPos;

    // Start is called before the first frame update
    void Start()
    {
        currentHeath = maxHealth;
        enemyKnockbackForce = 1000f;
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
        
            if (!alreadyAttacked && distance < sightDistance) {
                if (distance > stoppingDistance) {
                    agent.SetDestination(target.position);
                }

                if (!isDashing) {
                    if (distance <= stoppingDistance) {
                        lastTargetPos = target.position;
                        isDashing = true;
                        currentDashTimer = 0.5f;
                    }
                }
            }
        }

    }

    void FixedUpdate() {
        if (isDashing && player) {

            animator.SetTrigger("Attack");

            transform.position = Vector2.MoveTowards(transform.position, lastTargetPos, dashSpeed * Time.deltaTime);
            currentDashTimer -= Time.deltaTime;

            if (currentDashTimer <= 0) {
                isDashing = false;
                alreadyAttacked = true;
                StartCoroutine(cooldownAttack());
            }
        }
    }

    public void TakeDamage(int damage) {
        currentHeath -= damage;
        
        animator.SetTrigger("Hurt");

        if (currentHeath <= 0) {
            StartCoroutine(EnemyDie());
            // EnemyDie();
        }
    }

    private IEnumerator EnemyDie() {
        animator.SetBool("IsDead", true);
        yield return new WaitForSeconds(0.75f);
        int tmp = Random.Range(1, 101);
        if (tmp < 10) {
            for (int i = 0; i < 1; i++) {
                Instantiate(material_3, transform.position, Quaternion.identity);
            }
        }else if (tmp > 30) {
            for (int i = 0; i < 1; i++) {
                Instantiate(material_1, transform.position, Quaternion.identity);
            }
        }
        Destroy(this.gameObject);
    }

    private IEnumerator cooldownAttack() {
        if (rb != null) {
            Vector2 distance = transform.position - target.transform.position;
            distance = distance.normalized * enemyKnockbackForce * 2;
            rb.AddForce(distance, ForceMode2D.Impulse);
            StartCoroutine(EnemyKnockback());
        }
        yield return new WaitForSeconds(1.5f);
        alreadyAttacked = false;
    }

    private IEnumerator EnemyKnockback() {
        yield return new WaitForSeconds(0.25f);
        rb.velocity = Vector2.zero;
    }
}
