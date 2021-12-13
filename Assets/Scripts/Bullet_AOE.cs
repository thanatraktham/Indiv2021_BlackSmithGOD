using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_AOE : MonoBehaviour
{

    private float enemyKnockbackForce = 1000f;
    public float balstRange;
    public float balstDamage;
    public LayerMask enemyLayers;
    public LayerMask crateLayers;

    private GameObject player;

    public void StartAOE() {
        // Debug.Log("AOE Start");
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, balstRange, enemyLayers);

        foreach(Collider2D obj in hitEnemies) {
            // Debug.Log("hit : " + obj.gameObject.name);
            if (obj.gameObject.CompareTag("Slime")) {
                obj.GetComponent<SlimeController>().TakeDamage(100);
            } else if (obj.gameObject.CompareTag("Skeleton")) {
                obj.GetComponent<SkeletonController>().TakeDamage(100);
            } else if (obj.gameObject.CompareTag("Treant")) {
                // Debug.Log("Hit Treant");
                obj.GetComponent<TreantController>().TakeDamage(100);
            } else if (obj.gameObject.CompareTag("Mole")) {
                MoleController moleController = obj.GetComponent<MoleController>();
                    if (moleController && !moleController.getIsInvincible()) {
                        obj.GetComponent<MoleController>().TakeDamage(100);
                    }
            }
            Rigidbody2D enemy_rb = obj.GetComponent<Rigidbody2D>();
            if (enemy_rb != null) {
                enemy_rb.isKinematic = false;
                Vector2 distance = obj.transform.position - transform.position;
                distance = distance.normalized * enemyKnockbackForce * 2;
                enemy_rb.AddForce(distance, ForceMode2D.Impulse);
                StartCoroutine(EnemyKnockback(enemy_rb));
            }
        }

        Collider2D[] hitCrate = Physics2D.OverlapCircleAll(transform.position, balstRange, crateLayers);

        foreach(Collider2D crate in hitCrate) {
            if (crate) {
                crate.GetComponent<CrateController>().dropMaterial();
                Destroy(crate.gameObject);
            }
        }
    }

    private IEnumerator EnemyKnockback(Rigidbody2D enemy_rb) {
        yield return new WaitForSeconds(0.25f);
        if (enemy_rb != null) {
            enemy_rb.velocity = Vector2.zero;
            enemy_rb.isKinematic = true;
        }
    }

    private void OnDrawGizmosSelected() {
        if (transform == null) return;
        Gizmos.DrawWireSphere(transform.position, balstRange);
    }
}
