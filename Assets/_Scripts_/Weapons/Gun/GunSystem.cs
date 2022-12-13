using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class GunSystem : MonoBehaviour
{
    // TODO 
    // korutyny w recoil i aim
    // poprawić automatyczne strzelanie
    // weapon swing & bob

    #region  Gun stats
    public int damage;
    public float timeBetweenShooting, spread, aimSpread, spreadHolder, range, reloadTime, timeBetweenShots;
    public int magazineSize, bulletsPerTap;
    public bool allowButtonHold;
    int bulletsLeft, bulletsShot;
    #endregion

    #region  bools
    bool shooting, readyToShoot, reloading, startShooting, reloadNow, isLeftMouseHeld;
    public bool allowPewPew, isAiming, wasAiming;
    public static bool weaponIsActive;
    public bool turnOffCanvas;
    public float aimAnimationSpeed;
    #endregion

    #region reference
    public Camera fpsCam;
    public Transform attackPoint;
    public RaycastHit rayHit;
    public LayerMask hittable;

    #endregion

    #region polish
    public GameObject muzzleFlash, bulletHole, enemyHole, defaultPosition, aimPosition;
    public AudioSource audioSource;
    public AudioClip clip;
    public TextMeshProUGUI text;
    #endregion

    #region recoil

    [Header("Recoil")]
    Transform positionAfterRecoil;
    Quaternion rotationAfterRecoil;

    public float positionReturnSpeed = 10f;
    public float rotationReturnSpeed = 15f;

    // public Vector3 RecoilKickRotation = new Vector3();
    public Vector3 RecoilKickBackAim = new Vector3();
    public Vector3 RecoilKickBack = new Vector3();

    // Vector3 rotationRecoil;
    Vector3 positionalRecoil;


    #endregion
    public void UIBullets()
    {
        if (turnOffCanvas)
        {
            text.gameObject.SetActive(false);

        }
        else if (!turnOffCanvas)
        {
            text.gameObject.SetActive(true);
        }
    }

    public void ReceiveInput(bool _isLeftMouseHeld)
    {
        isLeftMouseHeld = _isLeftMouseHeld;

        if (weaponIsActive && isLeftMouseHeld)
        {
            // if (!allowPewPew)
            // {
            if (startShooting)
            {
                Shoot();
            }
            // }
            // else
            // {
            //     PewPew();
            // }
        }
    }

    public void ReceiveAimInput(bool _isAiming)
    {
        isAiming = _isAiming;

        if (isAiming)
        {
            wasAiming = true;
            Aim();
        }
        // else if (wasAiming && !isAiming)
        else if (!isAiming)
        {
            OutAim();
            // wasAiming = false;
        }


        if (isAiming)
        {
            positionAfterRecoil = aimPosition.transform;
            rotationAfterRecoil = aimPosition.transform.rotation;
        }
        else if (!isAiming)
        {
            positionAfterRecoil = defaultPosition.transform;
            rotationAfterRecoil = defaultPosition.transform.rotation;
        }


    }

    private void Start()
    {
        bulletsLeft = magazineSize;
        readyToShoot = true;
        spreadHolder = spread;
        if (this.gameObject.TryGetComponent<Renderer>(out Renderer renderer))
            renderer.material.SetFloat("_startClue", 1f);
    }

    private void Update()
    {
        text.SetText(bulletsLeft + " / " + magazineSize);
        if (weaponIsActive)
            MouseLook.slotFull = true;
        // if (weaponIsActive && isLeftMouseHeld)
        // {
        //     if (!allowPewPew)
        //     {

        //         if (startShooting)
        //         {
        //             Shoot();
        //         }
        //     }
        //     else
        //     {
        //         PewPew();
        //     }
        // }

        // if (reloadNow)
        // {
        //   Reload();
        // }

        // if (isAiming)
        // {
        //     wasAiming = true;
        //     Aim();
        // }
        // // else if (wasAiming && !isAiming)
        // else if (!isAiming)
        // {
        //     OutAim();
        //     // wasAiming = false;
        // }

        //recoil
        // if (isAiming)
        // {
        //     positionAfterRecoil = aimPosition.transform;
        //     rotationAfterRecoil = aimPosition.transform.rotation;
        // }
        // else if (!isAiming)
        // {
        //     positionAfterRecoil = defaultPosition.transform;
        //     rotationAfterRecoil = defaultPosition.transform.rotation;
        // }
        transform.position = Vector3.Lerp(transform.position, positionAfterRecoil.transform.position, positionReturnSpeed * Time.deltaTime);
    }

    #region shooting

    private void OutAim()
    {
        spread = spreadHolder;
        GameObject self = gameObject;
        Vector3 weaponPosition = self.transform.position;
        Vector3 scale = self.transform.localScale;
        self.transform.SetParent(defaultPosition.transform, false);
        self.transform.localScale = scale;

        // self.transform.position = Vector3.Lerp(weaponPosition, defaultPosition.transform.position, aimAnimationSpeed * Time.deltaTime);
        Vector3.Lerp(weaponPosition, defaultPosition.transform.position, aimAnimationSpeed * Time.deltaTime);
    }
    private void Aim()
    {
        spread = aimSpread;
        GameObject self = gameObject;
        Vector3 weaponPosition = self.transform.position;
        Vector3 scale = self.transform.localScale;
        self.transform.SetParent(aimPosition.transform, false);
        self.transform.localScale = scale;

        Vector3.Lerp(weaponPosition, aimPosition.transform.position, aimAnimationSpeed * Time.deltaTime);
    }
    private void Shoot()
    {
        bulletsShot = bulletsPerTap;

        if (!reloading && readyToShoot && bulletsLeft > 0)
        {
            readyToShoot = false;
            Instantiate(muzzleFlash, attackPoint.position, attackPoint.rotation);
            // recoil2
            // transform.position = Vector3.Lerp(transform.position, positionAfterRecoil.transform.position, positionReturnSpeed * Time.deltaTime);

            //recoil
            if (isAiming)
            {
                positionalRecoil = new Vector3(RecoilKickBackAim.x, RecoilKickBackAim.y, RecoilKickBackAim.z);

                transform.position += positionalRecoil;
            }
            else
            {
                positionalRecoil = new Vector3(RecoilKickBack.x, RecoilKickBack.y, RecoilKickBack.z);

                transform.position += positionalRecoil;
            }

            // shoot
            for (int i = 0; i < bulletsPerTap; i++)
            {
                //direction with spread
                //spread
                float x = Random.Range(-spread, spread);
                float y = Random.Range(-spread, spread);

                Vector3 direction = fpsCam.transform.forward + new Vector3(x, y, 0);

                //Raycast
                if (Physics.Raycast(fpsCam.transform.position, direction, out rayHit, range, hittable))
                {
                    if (rayHit.collider.CompareTag("Enemy"))
                    {
                        rayHit.collider.GetComponent<EnemyLocomotion>().TakeDamage(damage);
                        //Graphics
                        Destroy((Instantiate(enemyHole, rayHit.point + (rayHit.normal * 0.0005f), Quaternion.FromToRotation(Vector3.up, rayHit.normal), rayHit.transform)), 4);

                    }
                    else if (rayHit.collider.CompareTag("destructibleEnv"))
                    {
                        Debug.Log("destrucible");
                        rayHit.collider.GetComponent<destroyEnv>().destroyObject(damage);
                        //Graphics
                        Destroy((Instantiate(bulletHole, rayHit.point + (rayHit.normal * 0.0005f), Quaternion.FromToRotation(Vector3.up, rayHit.normal), rayHit.transform)), 4);
                    }
                    else
                    {
                        //Graphics
                        Destroy((Instantiate(bulletHole, rayHit.point + (rayHit.normal * 0.0005f), Quaternion.FromToRotation(Vector3.up, rayHit.normal))), 4);
                    }
                }

                bulletsLeft--;
                bulletsShot--;
            }

            Invoke("ResetShoot", timeBetweenShooting);
            // if (bulletsShot > 0 && bulletsLeft > 0)
            //     Invoke("Shoot", timeBetweenShots);

        }
        startShooting = false;
    }
    // public void PewPew()
    // {
    //     // isLeftMouseHeld = false;
    //     bulletsShot = bulletsPerTap;

    //     if (!reloading && readyToShoot && bulletsLeft > 0)
    //     {
    //         readyToShoot = false;
    //         Instantiate(muzzleFlash, attackPoint.position, attackPoint.rotation);

    //         if (isAiming)
    //         {
    //             positionalRecoil = new Vector3(RecoilKickBackAim.x, RecoilKickBackAim.y, RecoilKickBackAim.z);
    //             // rotationRecoil = new Vector3((transform.rotation.x + RecoilKickRotation.x), (transform.rotation.y + RecoilKickRotation.y), (transform.rotation.z + RecoilKickRotation.z));
    //             transform.position += positionalRecoil;
    //             // transform.rotation = Quaternion.Euler(rotationRecoil);
    //         }
    //         else
    //         {
    //             positionalRecoil = new Vector3(RecoilKickBack.x, RecoilKickBack.y, RecoilKickBack.z);
    //             // rotationRecoil = new Vector3((transform.rotation.x + RecoilKickRotation.x), (transform.rotation.y + RecoilKickRotation.y), (transform.rotation.z + RecoilKickRotation.z));
    //             transform.position += positionalRecoil;
    //             // transform.rotation = Quaternion.Euler(rotationRecoil);
    //         }


    //         for (int i = 0; i < bulletsPerTap; i++)
    //         {
    //             //direction with spread
    //             //spread
    //             float x = Random.Range(-spread * 1.25f, spread * 1.25f);
    //             float y = Random.Range(-spread * 1.25f, spread * 1.25f);

    //             Vector3 direction = fpsCam.transform.forward + new Vector3(x, y, 0);

    //             //Raycast
    //             if (Physics.Raycast(fpsCam.transform.position, direction, out rayHit, range, hittable))
    //             {
    //                 if (rayHit.collider.CompareTag("Enemy"))
    //                 {
    //                     rayHit.collider.GetComponent<Enemy>().TakeDamage(damage);
    //                     //Graphics
    //                     Destroy((Instantiate(enemyHole, rayHit.point + (rayHit.normal * 0.0005f), Quaternion.FromToRotation(Vector3.up, rayHit.normal), rayHit.transform)), 4);

    //                 }
    //                 else if (rayHit.collider.CompareTag("destructibleEnv"))
    //                 {
    //                     Debug.Log("destrucible");
    //                     rayHit.collider.GetComponent<destroyEnv>().destroyObject(damage);
    //                     //Graphics
    //                     Destroy((Instantiate(bulletHole, rayHit.point + (rayHit.normal * 0.0005f), Quaternion.FromToRotation(Vector3.up, rayHit.normal), rayHit.transform)), 4);
    //                 }
    //                 else
    //                 {
    //                     //Graphics
    //                     Destroy((Instantiate(bulletHole, rayHit.point + (rayHit.normal * 0.0005f), Quaternion.FromToRotation(Vector3.up, rayHit.normal))), 4);
    //                 }
    //             }




    //             bulletsLeft--;
    //             bulletsShot--;
    //         }

    //         Invoke("ResetShoot", timeBetweenShooting);
    //         // if (bulletsShot > 0 && bulletsLeft > 0 && isLeftMouseHeld)
    //         //     Invoke("PewPew", timeBetweenShots);


    //     }
    //     // startShooting = false;
    // }


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
        Reload();
    }


    private void Reload()
    {
        if (bulletsLeft < magazineSize && !reloading)
        {
            audioSource.Play();
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

    #region recoil
    #endregion

}