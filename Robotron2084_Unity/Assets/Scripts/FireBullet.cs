using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBullet : MonoBehaviour
{
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] AudioClip BulletFireSound;
    [SerializeField] GameObject firingPoint;

    void Start()
    {
    }

    public void fireBullet(Vector3 forward, Vector3 ownerVelocity)
    {
        GameObject bullet = Instantiate(bulletPrefab, firingPoint.transform.position, firingPoint.transform.rotation);
       
        SFXHandler.Instance.PlaySFX(BulletFireSound, firingPoint.transform.position);
        
        bullet.transform.rotation.SetLookRotation(forward, Vector3.left);
        
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        bulletScript.owner = gameObject;
        
        BulletMovement bulletMovementScript = bullet.GetComponent<BulletMovement>();
        bulletMovementScript.setInstantaneousOwnerVelocity(ownerVelocity);
    }
}
