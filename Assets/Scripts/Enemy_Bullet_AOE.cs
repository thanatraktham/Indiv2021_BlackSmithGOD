using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Bullet_AOE : MonoBehaviour
{
    public GameObject hitEffect;
    public GameObject treantPoison;

    public void StartAOE() {
        GameObject poison = Instantiate(treantPoison, transform.position, Quaternion.identity);
        Destroy(poison, 3f);
        Destroy(gameObject);
    }
}
