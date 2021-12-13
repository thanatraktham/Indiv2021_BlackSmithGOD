using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoleController : MonoBehaviour
{
    //MoleComponent//
    public Animator animator;
    public int maxHealth;
    public Rigidbody2D rb;
    public LayerMask playerLayers;
    private int currentHeath;
    private bool invincible;

    //Dash//
    public float dashSpeed;
    private bool isPreAttack;
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
    public GameObject moleAOE;
    private GameObject player;
    private Transform target;
    private Vector3 lastTargetPos;
    private CircleCollider2D cc2d;

    // Start is called before the first frame update
    void Start()
    {
        currentHeath = maxHealth;
        enemyKnockbackForce = 1000f;
        player = GameObject.FindGameObjectWithTag("Player");

        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
		agent.updateRotation = false;
		agent.updateUpAxis = false;

        cc2d = GetComponent<CircleCollider2D>();
        cc2d.enabled = false;
        invincible = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Debug.Log("Mole is invincible : " + invincible);

        if (player != null) {
            target = player.GetComponent<Transform>();
            distance = Vector2.Distance(transform.position, target.position);
        
            if (!alreadyAttacked && distance < sightDistance) {
                if (distance > stoppingDistance) {
                    agent.SetDestination(target.position);
                }

                if (distance <= stoppingDistance) {
                    alreadyAttacked = true;
                    StartCoroutine(cooldownAttack());
                }
            }
        }

    }

    // void FixedUpdate() {

    //     if (isPreAttack && player) {

    //         // animator.SetTrigger("Attack");

    //         transform.position = Vector2.MoveTowards(transform.position, lastTargetPos, dashSpeed * Time.deltaTime);
    //         currentDashTimer -= Time.deltaTime;

    //         if (currentDashTimer <= 0) {
    //             isPreAttack = false;
    //             alreadyAttacked = true;
    //             StartCoroutine(cooldownAttack());
    //         }
    //     }
    // }

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
            Instantiate(material_3, transform.position, Quaternion.identity);
        }else if (tmp > 30) {
            Instantiate(material_1, transform.position, Quaternion.identity);
        }
        Destroy(this.gameObject);
    }

    private IEnumerator cooldownAttack() {
        // if (rb != null) {
        //     Vector2 distance = transform.position - target.transform.position;
        //     distance = distance.normalized * enemyKnockbackForce * 2;
        //     rb.AddForce(distance, ForceMode2D.Impulse);
        //     StartCoroutine(EnemyKnockback());
        // }
        animator.SetTrigger("PreAttack");
        yield return new WaitForSeconds(1f);
        animator.SetTrigger("Attack");
        invincible = false;
        cc2d.enabled = true;
        Collider2D[] hitPlayers = Physics2D.OverlapCircleAll(transform.position, 10f, playerLayers);
        foreach(Collider2D obj in hitPlayers) {
            obj.GetComponent<playerMovement>().TakeDamage(50);
        }
        GameObject moleAOE_tmp = Instantiate(moleAOE, transform.position, Quaternion.identity);
        Destroy(moleAOE_tmp, .5f);

        yield return new WaitForSeconds(.5f);
        alreadyAttacked = false;
        invincible = true;
        cc2d.enabled = false;
    }

    private IEnumerator EnemyKnockback() {
        yield return new WaitForSeconds(0.25f);
        rb.velocity = Vector2.zero;
    }

    public bool getIsInvincible() {
        return invincible;
    }

    public void setIsInvincible(bool tmp) {
        invincible = tmp;
    }
}
