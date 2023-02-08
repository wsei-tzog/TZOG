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

    [Header("Gun stats")]
    public int damage, magazineSize, bulletsPerTap;
    public float timeBetweenShooting, spread, aimSpread, spreadHolder, range, reloadTime, timeBetweenShots, aimAnimationSpeed, pushForce;
    public bool allowButtonHold, turnOffCanvas;
    int bulletsLeft, bulletsShot;
    bool shooting, readyToShoot, reloading, startShooting, reloadNow, isLeftMouseHeld;
    public bool isAiming, wasAiming;
    public static bool weaponIsActive;

    [Header("Reference")]

    public Camera fpsCam;
    public Transform attackPoint;
    public RaycastHit rayHit;
    public LayerMask hittable;


    [Header("Polish")]

    public GameObject muzzleFlash, bulletHole, enemyHole, defaultPosition, aimPosition;
    public AudioSource audioSource;
    public AudioClip clip;
    public TextMeshProUGUI text;
    public Transform targetTransform;



    [Header("Recoil")]

    // public float recoilAmount = 25f;
    // public float aimRecoilAmount = 15f;
    public float recoilAmout;
    public float aimRecoilAmout;
    public float counter;
    public float returnSpeed;

    // Transform positionAfterRecoil;
    // Quaternion rotationAfterRecoil;

    // public float positionReturnSpeed = 10f;
    // public float rotationReturnSpeed = 15f;

    // // public Vector3 RecoilKickRotation = new Vector3();
    // public Vector3 RecoilKickBackAim = new Vector3();
    // public Vector3 RecoilKickBack = new Vector3();

    // // Vector3 rotationRecoil;
    // Vector3 positionalRecoil;

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
            if (startShooting)
            {
                Shoot();
            }
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
        else if (!isAiming)
        {
            OutAim();
        }


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


    }

    private void Awake()
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

        // float recoil = Mathf.Sin(counter) * aimRecoilAmount;

        // transform.position = Vector3.Lerp(transform.position, positionAfterRecoil.transform.position, positionReturnSpeed * Time.deltaTime);
    }


    private void OutAim()
    {
        spread = spreadHolder;
        GameObject self = gameObject;

        Vector3 weaponPosition = self.transform.position;
        Vector3 scale = self.transform.localScale;
        self.transform.SetParent(defaultPosition.transform, false);
        self.transform.localScale = scale;

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
            AlarmEnemies();
            if (isAiming)
            {
                transform.rotation = Quaternion.Euler(aimRecoilAmout, 0, 0);
            }
            else
            {
                transform.rotation = Quaternion.Euler(recoilAmout, 0, 0);
            }

            StartCoroutine(Return());

            // shoot
            for (int i = 0; i < bulletsPerTap; i++)
            {
                //spread
                float x = Random.Range(-spread, spread);
                float y = Random.Range(-spread, spread);

                Vector3 direction = fpsCam.transform.forward + new Vector3(x, y, 0);

                //Raycast
                if (Physics.Raycast(fpsCam.transform.position, direction, out rayHit, range, hittable))
                {
                    if (rayHit.collider.CompareTag("Enemy"))
                    {
                        rayHit.collider.GetComponent<NewEnemyAI>().TakeDamage(damage);
                        targetTransform = rayHit.collider.GetComponent<Transform>();

                        Vector3 forceDirection = targetTransform.position - transform.position;
                        targetTransform.position += direction.normalized * pushForce * Time.deltaTime;

                        //Graphics
                        Destroy((Instantiate(enemyHole, rayHit.point + (rayHit.normal * 0.0005f), Quaternion.FromToRotation(Vector3.up, rayHit.normal), rayHit.transform)), 4);

                    }
                    else if (rayHit.collider.CompareTag("destructibleEnv"))
                    {
                        rayHit.collider.GetComponent<destroyEnv>().destroyObject(damage);

                        targetTransform = rayHit.collider.GetComponent<Transform>();
                        Vector3 forceDirection = targetTransform.position - transform.position;
                        targetTransform.position += direction.normalized * pushForce * Time.deltaTime;
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


    private IEnumerator Return()
    {
        Quaternion presentRotation = transform.rotation;

        var timePassed = 0f;
        while (timePassed < returnSpeed)
        {
            var factor = timePassed / returnSpeed;
            // optional add ease-in and -out
            // factor = Mathf.SmoothStep(0, 1, factor);
            factor = 1f - Mathf.Cos(factor * Mathf.PI * 0.7f);

            transform.localRotation = Quaternion.Lerp(presentRotation, Quaternion.identity, factor);
            // or
            //transformToRotate.rotation = Quaternion.Slerp(startRotation, targetRotation, factor);

            // increae by the time passed since last frame
            timePassed += Time.deltaTime;

            // important! This tells Unity to interrupt here, render this frame
            // and continue from here in the next frame
            yield return null;
        }


        // to be sure to end with exact values set the target rotation fix when done
        transform.localRotation = Quaternion.identity;

    }

    public void AlarmEnemies()
    {


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

}