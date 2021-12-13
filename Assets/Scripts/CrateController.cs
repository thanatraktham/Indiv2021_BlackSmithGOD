using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrateController : MonoBehaviour
{
    public GameObject material_1;
    public GameObject material_2;
    public GameObject material_3;


    public void dropMaterial() {
        int tmp = Random.Range(1, 101);
        if (tmp > 85) {
            Instantiate(material_3, transform.position, Quaternion.identity);
        } else if (tmp > 55) {
            Instantiate(material_2, transform.position, Quaternion.identity);
        } else if (tmp > 5) {
            Instantiate(material_1, transform.position, Quaternion.identity);
        } else {
            Debug.Log("Drop nothing");
        }
    }
}
