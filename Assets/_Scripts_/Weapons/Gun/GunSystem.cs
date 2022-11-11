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
    bool shootSeries;
    bool reloadNow;
    bool isLeftMouseHeld;
    public static bool weaponIsActive;

    // reference
    public Camera fpsCam;
    public Transform attackPoint;
    public RaycastHit rayHit;
    public LayerMask whatIsEnemy;

    // Graphics
    public GameObject muzzleFlash, bulletHole;
    public TextMeshProUGUI text;

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

        if (!isLeftMouseHeld && weaponIsActive)
        {
            if (startShooting)
            {
                Shoot();
            }
        }
        else if (weaponIsActive)
        {
            PewPew();
            isLeftMouseHeld = false;
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
            bulletsShot--;
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
        if (!reloading && bulletsLeft > 0 && readyToShoot)
        {
            isLeftMouseHeld = false;
            readyToShoot = false;
            //spread
            float x = Random.Range(-spread * (2), spread * (2));
            float y = Random.Range(-spread * (2), spread * (2));

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
            Destroy((Instantiate(bulletHole, rayHit.point + (rayHit.normal * 0.0005f), Quaternion.FromToRotation(Vector3.up, rayHit.normal))), 4);
            Instantiate(muzzleFlash, attackPoint.position, attackPoint.rotation);


            bulletsLeft--;
            bulletsShot--;
            Invoke("ResetShoot", timeBetweenShooting);
            if (bulletsShot > 0 && bulletsLeft > 0 && isLeftMouseHeld)
                Invoke("Shoot", timeBetweenShots);


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