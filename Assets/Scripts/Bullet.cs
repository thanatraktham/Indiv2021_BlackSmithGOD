using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int baseDamage;
    public GameObject hitEffect;
    public Vector2 velocity = new Vector2(0f, 0f);
    public GameObject player;
    public Vector2 offset = new Vector2(0f, 0f);
    private Vector2 bulletOffset = new Vector2(0f, 0.8f);
    private int bulletDirection = 1;
    private Vector2 currentPosition;

    private void Update() {
        currentPosition = new Vector2(transform.position.x, transform.position.y);
        Vector2 newPosition = currentPosition + bulletDirection * velocity * Time.deltaTime;

        RaycastHit2D[] hits = Physics2D.LinecastAll(currentPosition + offset, newPosition + offset);

        foreach (RaycastHit2D hit in hits) {
            GameObject other = hit.collider.gameObject;
            if (other != player) {
                Find_AOE_Script();
                if (other.CompareTag("Player")) {
                    other.GetComponent<playerMovement>().TakeDamage(baseDamage / 2);
                    Destroy(gameObject);
                    break;
                } else if (other.CompareTag("Wall")) {
                    createHitEffect();
                    break;
                } else if (other.CompareTag("Slime")) {
                    SlimeController slimeController = other.GetComponent<SlimeController>();
                    slimeController.TakeDamage(baseDamage);
                    createHitEffect();
                    break;
                } else if (other.CompareTag("Skeleton")) {
                    // SkeletonController skeletonController = other.GetComponent<SkeletonController>();
                    other.GetComponent<SkeletonController>().TakeDamage(baseDamage * 3);
                    createHitEffect();
                    break;
                } else if (other.CompareTag("Treant")) {
                    // TreantController treantController = other.GetComponent<TreantController>();
                    other.GetComponent<TreantController>().TakeDamage(baseDamage * 3);
                    createHitEffect();
                    break;
                } else if (other.CompareTag("Mole")) {
                    Debug.Log("Bullet hit mole");
                    MoleController moleController = other.GetComponent<MoleController>();
                    if (moleController && !moleController.getIsInvincible()) {
                    // if (moleController) {
                        Debug.Log("mole hits");
                        other.GetComponent<MoleController>().TakeDamage(baseDamage * 3);
                        createHitEffect();
                    } else {
                        Debug.Log("Cannot Find Mole Script");
                    }
                    break;
                } else if (other.CompareTag("HomingBullet")) {
                    createHitEffect();
                    Destroy(other.gameObject);
                    break;
                } else if (other.CompareTag("Crate")) {
                    other.GetComponent<CrateController>().dropMaterial();
                    Debug.Log("Hit crate");
                    createHitEffect();
                    Destroy(other.gameObject);
                    break;
                } else {

                    break;
                }

                
            }
        }

        transform.position = newPosition;
    }

    public void ReflectBullet() {
        bulletDirection = -1;
    }

    private bool Find_AOE_Script() {
        Bullet_AOE bullet_aoe = GetComponent<Bullet_AOE>();
        Enemy_Bullet_AOE enemy_bullet_aoe = GetComponent<Enemy_Bullet_AOE>();

        if (bullet_aoe != null) {
            bullet_aoe.StartAOE();
        } else if (enemy_bullet_aoe != null) {
            enemy_bullet_aoe.StartAOE();
        }

        return (bullet_aoe != null);
    }

    private void createHitEffect() {
        GameObject effect = Instantiate(hitEffect, currentPosition + bulletOffset, Quaternion.identity);
        Destroy(effect, 0.5f);
        Destroy(gameObject);
    }

    //Getter/Setter
    public int getBaseDamage() {
        return baseDamage;
    }

    public void setBaseDamage(int newBaseDamage) {
        baseDamage = newBaseDamage;
    }

}
