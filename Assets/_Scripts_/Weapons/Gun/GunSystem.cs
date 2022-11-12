using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class GunSystem : MonoBehaviour
{
    #region  Gun stats
    public int damage;
    public float timeBetweenShooting, spread, range, reloadTime, timeBetweenShots;
    public int magazineSize, bulletsPerTap;
    public bool allowButtonHold;
    int bulletsLeft, bulletsShot;
    #endregion

    #region  bools
    bool shooting, readyToShoot, reloading, startShooting, reloadNow, isLeftMouseHeld;
    public bool allowPewPew;
    public static bool weaponIsActive, turnOffCanvas;
    #endregion

    #region reference
    public Camera fpsCam;
    public Transform attackPoint;
    public RaycastHit rayHit;
    public LayerMask whatIsEnemy;

    #endregion

    #region  Graphics
    public GameObject muzzleFlash, bulletHole;
    public TextMeshProUGUI text;
    #endregion

    public void ReceiveInput(bool _isLeftMouseHeld)
    {
        isLeftMouseHeld = _isLeftMouseHeld;
    }

    private void Start()
    {
        bulletsLeft = magazineSize;
        readyToShoot = true;
    }

    private void Update()
    {
        text.SetText(bulletsLeft + " / " + magazineSize);
        if (turnOffCanvas)
        {
            text.enabled = false;
        }

        if (weaponIsActive && isLeftMouseHeld)
        {
            if (!allowPewPew)
            {
                if (startShooting)
                {
                    Shoot();
                }
            }
            else
            {
                PewPew();
            }
        }

        if (reloadNow)
        {
            Reload();
        }

    }

    #region shooting
    private void Shoot()
    {
        bulletsShot = bulletsPerTap;

        if (!reloading && readyToShoot && bulletsLeft > 0)
        {
            readyToShoot = false;
            Instantiate(muzzleFlash, attackPoint.position, attackPoint.rotation);
            for (int i = 0; i < bulletsPerTap; i++)
            {
                //direction with spread
                //spread
                float x = Random.Range(-spread, spread);
                float y = Random.Range(-spread, spread);

                Vector3 direction = fpsCam.transform.forward + new Vector3(x, y, 0);

                //Raycast
                if (Physics.Raycast(fpsCam.transform.position, direction, out rayHit, range, whatIsEnemy))
                {
                    if (rayHit.collider.CompareTag("Enemy"))
                    {
                        rayHit.collider.GetComponent<Enemy>().TakeDamage(damage * bulletsPerTap);
                    }
                }

                //Graphics
                Destroy((Instantiate(bulletHole, rayHit.point + (rayHit.normal * 0.0005f), Quaternion.FromToRotation(Vector3.up, rayHit.normal))), 4);

                bulletsLeft--;
                bulletsShot--;
            }

            Invoke("ResetShoot", timeBetweenShooting);
            if (bulletsShot > 0 && bulletsLeft > 0)
                Invoke("Shoot", timeBetweenShots);

        }
        startShooting = false;
    }
    public void PewPew()
    {
        isLeftMouseHeld = false;
        bulletsShot = bulletsPerTap;

        if (!reloading && readyToShoot && bulletsLeft > 0)
        {
            readyToShoot = false;
            Instantiate(muzzleFlash, attackPoint.position, attackPoint.rotation);
            for (int i = 0; i < bulletsPerTap; i++)
            {
                //direction with spread
                //spread
                float x = Random.Range(-spread - 0.25f, spread + 0.25f);
                float y = Random.Range(-spread - 0.25f, spread + 0.25f);

                Vector3 direction = fpsCam.transform.forward + new Vector3(x, y, 0);

                //Raycast
                if (Physics.Raycast(fpsCam.transform.position, direction, out rayHit, range, whatIsEnemy))
                {
                    if (rayHit.collider.CompareTag("Enemy"))
                    {
                        rayHit.collider.GetComponent<Enemy>().TakeDamage(damage);
                    }
                }

                //Graphics
                Destroy((Instantiate(bulletHole, rayHit.point + (rayHit.normal * 0.0005f), Quaternion.FromToRotation(Vector3.up, rayHit.normal))), 4);


                bulletsLeft--;
                bulletsShot--;
            }

            Invoke("ResetShoot", timeBetweenShooting);
            // if (bulletsShot > 0 && bulletsLeft > 0 && isLeftMouseHeld)
            //     Invoke("PewPew", timeBetweenShots);


        }
        startShooting = false;
    }
    public void OnShootPressed()
    {
        startShooting = true;
        readyToShoot = true;
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
    #endregion

}