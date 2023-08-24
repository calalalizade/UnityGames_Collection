using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootController : MonoBehaviour
{
    [Header("Main References")]
    [SerializeField] Bullet bulletPrefab;
    [SerializeField] Transform shootOrigin;

    [Header("Bullet Properties")]
    [SerializeField] float bulletSpeed = 5.0f;
    [SerializeField] float bulletLifetime = 1.5f;
    [SerializeField] float fireRate = 1.0f;

    private float nextFire = 0.0f;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space) && Time.time > nextFire)
        {
            ShootBullet();
            nextFire = Time.time + fireRate;
        }
    }

    private void ShootBullet()
    {
        Bullet bullet = Instantiate(bulletPrefab, shootOrigin.position, shootOrigin.rotation);
        bullet.Launch(transform.up, bulletSpeed, bulletLifetime);
    }
}
