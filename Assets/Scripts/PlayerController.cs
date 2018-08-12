using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {

    public Bullet bullet;
    public ParticleSystem pSys;
    public float moveSpeed;
    public float maxX;
    public float fireRate;
    public float bulletSpeed;
    public float bulletLifetime;

    public bool useRapidFire;
    public bool useBoost;

    float t;
    float actualSpeed;

	void Start ()
    {
        PersistentData.Load();
        moveSpeed = actualSpeed = PersistentData.playerMoveSpeed;
        fireRate = PersistentData.playerFireRate;
        useRapidFire = PersistentData.playerUseRapidFire;
        useBoost = PersistentData.playerUseBoost;
	}

	void Update ()
    {
        var v = transform.position;
        v += Vector3.right * actualSpeed * Input.GetAxisRaw("Horizontal") * Time.deltaTime;
        v.x = Mathf.Clamp(v.x, -maxX, maxX);
        transform.position = v;

        t -= Time.deltaTime * fireRate;

        if (t <= 0 && ((Input.GetKey(KeyCode.Space) && useRapidFire) || (Input.GetKeyDown(KeyCode.Space) && !useRapidFire)))
        {
            Shoot();
            t = 1;
        }

        if (useBoost && Input.GetKeyDown(KeyCode.LeftShift))
        {
            actualSpeed = moveSpeed * 3;
            pSys.Play();
        }
        if (useBoost && Input.GetKeyUp(KeyCode.LeftShift))
        {
            actualSpeed = moveSpeed;
            pSys.Stop();
        }
    }

    void Shoot()
    {
        SoundManager.Instance.PlaySound(Sound.Shoot);
        var b = Instantiate(bullet);
        b.transform.position = transform.position;
        b.speed = bulletSpeed;
        b.lifeTime = bulletLifetime;
        b.Init();
    }
}
