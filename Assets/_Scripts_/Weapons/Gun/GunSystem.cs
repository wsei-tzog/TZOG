using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
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
    bool startShooting;
    bool reloadNow;

    // reference
    public Camera fpsCam;
    public Transform attackPoint;
    public RaycastHit rayHit;
    public LayerMask whatIsEnemy;

    // Graphics
    public GameObject muzzleFlash, bulletHole;
    public TextMeshProUGUI text;
    private void Start()
    {
        bulletsLeft = magazineSize;
        readyToShoot = true;
    }

    private void Update()
    {
        text.SetText(bulletsLeft + " / " + magazineSize);

        if (startShooting)
        {
            Shoot();
        }

        if (reloadNow)
        {
            Reload();
        }

    }




    public void OnShootPressed()
    {
        startShooting = true;
        readyToShoot = true;
    }

    private void Shoot()
    {
        if (readyToShoot && !reloading && bulletsLeft > 0)
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

            //Graphics
            Destroy((Instantiate(bulletHole, rayHit.point + (rayHit.normal * 0.05f), Quaternion.FromToRotation(Vector3.up, rayHit.normal))), 4);
            Destroy((Instantiate(muzzleFlash, attackPoint.position, attackPoint.rotation)), 0.1f);

            bulletsLeft--;
            Invoke("ResetShoot", timeBetweenShooting);
        }
        else
        {
            Debug.Log("No more bullets");
        }
        startShooting = false;
    }

    public void ResetShoot()
    {
        readyToShoot = true;
    }


    public void OnReloadPressed()
    {
        reloadNow = true;
    }
    private void Reload()
    {
        if (bulletsLeft < magazineSize && !reloading)
        {
            reloading = true;
            Invoke("ReloadingFinished", reloadTime);
        }

        reloadNow = false;
    }

    private void ReloadingFinished()
    {
        bulletsLeft = magazineSize;
        reloading = false;
    }
}
