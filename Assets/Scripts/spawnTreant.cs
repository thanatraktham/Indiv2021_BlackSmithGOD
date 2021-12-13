using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnTreant : MonoBehaviour
{
    public GameObject treant1;
    public GameObject treant2;
    public GameObject treant3;
    public GameObject treant4;
    float randX1;
    float randX2;
    float randX3;
    float randX4;
    float randY1;
    float randY2;
    float randY3;
    float randY4;
    Vector2 whereToSpawn1;
    Vector2 whereToSpawn2;
    Vector2 whereToSpawn3;
    Vector2 whereToSpawn4;
    public float spawnRate = 5f;
    float nextSpawn = 3.0f;
    public float wave = 4;
    float waveCount = 0;
    public float start = 1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time > nextSpawn && waveCount < wave && start == 1 ){
            waveCount += 1;
            nextSpawn = Time.time + spawnRate;
            randX1 = Random.Range(-4f, 4f);
            randX2 = Random.Range(-4f, 4f);
            randX3 = Random.Range(-4f, 4f);
            randX4 = Random.Range(-4f, 4f);
            randY1 = Random.Range(-4f, 4f);
            randY2 = Random.Range(-4f, 4f);
            randY3 = Random.Range(-4f, 4f);
            randY4 = Random.Range(-4f, 4f);
            whereToSpawn1 = new Vector2(randX1+transform.position.x+5, randY1+transform.position.y+5);
            whereToSpawn2 = new Vector2(randX2+transform.position.x+5, randY2+transform.position.y-5);
            whereToSpawn3 = new Vector2(randX3+transform.position.x-5, randY3+transform.position.y+5);
            whereToSpawn4 = new Vector2(randX4+transform.position.x-5, randY4+transform.position.y-5);
            Instantiate(treant1, whereToSpawn1, Quaternion.identity);
            Instantiate(treant2, whereToSpawn2, Quaternion.identity);
            Instantiate(treant3, whereToSpawn3, Quaternion.identity);
            Instantiate(treant4, whereToSpawn4, Quaternion.identity);
        }    
    }
    public void startSpawn(){
        start = 1;
    }
}
