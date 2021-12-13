using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour {

    private GameObject target;

    public Vector3 offset = new Vector3(0,0,-1);

    void Awake() {
        DontDestroyOnLoad(this.gameObject);
    }

    void Start() {
        target = GameObject.FindGameObjectWithTag("Player");
    }

    void FixedUpdate()
    {
        if(target)
        {
            transform.position = new Vector3
            (
                target.transform.position.x + offset.x,
                target.transform.position.y + offset.y,
                target.transform.position.z + offset.z
            );
        }
    }
}