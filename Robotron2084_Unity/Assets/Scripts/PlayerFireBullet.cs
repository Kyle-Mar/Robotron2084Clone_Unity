using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFireBullet : MonoBehaviour
{
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] AudioClip BulletFireSound;
    GameObject firingPoint;

    void Start()
    {
        firingPoint = GameObject.Find("FiringPoint");
    }

    public void fireBullet(Vector3 forward, Vector3 playerVelocity)
    {
        GameObject bullet = Instantiate(bulletPrefab, firingPoint.transform.position, firingPoint.transform.rotation);
        SFXHandler.Instance.PlaySFX(BulletFireSound, firingPoint.transform.position);
        bullet.transform.rotation.SetLookRotation(forward, Vector3.left) ;
        BulletMovement bulletMovementScript = bullet.GetComponent<BulletMovement>();
        bulletMovementScript.setInstantaneousPlayerVelocity(playerVelocity);
    }
}
