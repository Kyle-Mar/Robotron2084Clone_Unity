using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFireBullet : MonoBehaviour
{
    [SerializeField] GameObject bulletPrefab;
    GameObject firingPoint;

    void Start()
    {
        firingPoint = GameObject.Find("FiringPoint");
    }

    public void fireBullet(Quaternion rot, Vector3 playerVelocity)
    {
        GameObject bullet = Instantiate(bulletPrefab, firingPoint.transform.position, firingPoint.transform.rotation);
        bullet.transform.rotation = rot;
        BulletMovement bulletMovementScript = bullet.GetComponent<BulletMovement>();
        bulletMovementScript.setInstantaneousPlayerVelocity(playerVelocity);
    }
}
