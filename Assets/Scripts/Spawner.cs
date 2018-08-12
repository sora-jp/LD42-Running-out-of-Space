using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

    public GameObject[] spawnables;
    public float xRange;
    public float spawnY;
    public float spawnsPS;
    float size;

    float t;

    private void Start()
    {
        spawnsPS = PersistentData.spawnsPerSec;
        size = PersistentData.size;
    }

    void Update ()
    {
        t -= Time.deltaTime * spawnsPS;
        if (t <= 0)
        {
            t = 1;
            Spawn();
        }
	}

    void Spawn()
    {
        GameObject go = Instantiate(spawnables[Random.Range(0, spawnables.Length)]);
        go.transform.position = new Vector2(Random.Range(-xRange, xRange), spawnY);
        go.transform.localScale = Vector3.one * size;
    }
}