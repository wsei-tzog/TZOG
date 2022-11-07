using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunSystem : MonoBehaviour
{
    // Gun stats
    public int damage;
    public float timeBetweenShooting, spread, range, reloadTime, timeBetweenShots;
    public int magazineSize, bulletsPerTap;
    public bool allowButtonHold;
    int bulletsLeft, bulletsShot;

    // bools
    bool shooting, readyToShoot, reloading;

    // reference
    public Camera fpsCam;
    public Transform attackPoint;
    public RaycastHit rayHit;
    public LayerMask whatIsEnemy;



    private void Start()
    {
        readyToShoot = true;
    }

    public void Shoot()
    {
        if (readyToShoot && shooting && !reloading && bulletsLeft > 0)
        {
            readyToShoot = false;
            //spread
            float x = Random.Range(-spread, spread);
            float y = Random.Range(-spread, spread);

            //direction with spread
            Vector3 direction = fpsCam.transform.forward + new Vector3(x, y, 0);

            //Raycast
            if (Physics.Raycast(fpsCam.transform.position, direction, out rayHit, range, whatIsEnemy))
            {
                Debug.Log(rayHit.collider.name);
                if (rayHit.collider.CompareTag("Enemy"))
                {
                    rayHit.collider.GetComponent<Enemy>().TakeDamage(damage);
                }
            }

            bulletsLeft--;
            Invoke("ResetShoot", timeBetweenShooting);

        }
    }

    public void ResetShoot()
    {
        readyToShoot = true;
    }


    public void Reload()
    {
        if (bulletsLeft < magazineSize && !reloading) ;
        {
            reloading = true;
            Invoke("ReloadingFinished", reloadTime);
        }

    }

    private void ReloadingFinished()
    {
        bulletsLeft = magazineSize;
        reloading = false;
    }
}
