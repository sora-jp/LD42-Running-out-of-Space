using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    public ParticleSystem explosion;
    public float speed;
    public float lifeTime;
    public int damage;

    public void Init()
    {
        damage = PersistentData.damage;
        Destroy(gameObject, lifeTime);
    }

	void Update ()
    {
        RaycastHit2D c = Physics2D.Raycast(transform.position, Vector2.up, speed * Time.deltaTime);
        if (c.collider != null) RayHit(c);
        transform.position += Vector3.up * speed;
	}

    private void RayHit(RaycastHit2D other)
    {
        if (other.transform.CompareTag("Enemy"))
        {
            // TODO: Better way to do this

            other.collider.GetComponent<FallDown>().Damage(damage);
            explosion.transform.parent = null;
            explosion.transform.position = other.point;
            explosion.Play();
            Destroy(gameObject);
            PersistentData.curStorage += damage;
        }
    }
}
