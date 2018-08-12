using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallDown : MonoBehaviour {

    public float fallSpeed;
    int health;

    private void Start()
    {
        health = PersistentData.health;
        fallSpeed = PersistentData.fallSpeed;
    }

    void Update ()
    {
        transform.position += Vector3.down * fallSpeed * Time.deltaTime;
        if (transform.position.y < -5.5f)
        {
            Destroy(gameObject);
            // TODO: Make penalty modifiable through variable
            PersistentData.curStorage -= health;
            if (PersistentData.curStorage < 0) PersistentData.curStorage = 0;
        }
	}

    public bool Damage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            SoundManager.Instance.PlaySound(Sound.Die);
            Destroy(gameObject);
        }
        return health <= 0;
    }
}
