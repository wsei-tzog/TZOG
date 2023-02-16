using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class GunSystem : MonoBehaviour
{


    [Header("Ammo stats")]
    public AmmoType type;
    public int ammoUsage;
    public int bulletsActuallyFired;
    public AmmoManager ammoManager;
    public int amountOfAmmoType;
    int ammoLeft;


    [Header("Gun stats")]

    public int damage, magazineSize, bulletsPerTap;
    // , recoilForce;
    public float fireRate, spread, aimSpread, spreadHolder, range, reloadTime, aimAnimationSpeed, pushForce;
    // , shootWaveRange;
    int bulletsLeftInMagazine, bulletsShot;
    bool reloading, isLeftMouseHeld, isAiming;
    // , wasAiming;
    public bool turnOffCanvas, readyToShoot;
    public static bool weaponIsActive;
    private float nextFireTime;

    [Header("Reference")]

    public Camera fpsCam;
    public Transform attackPoint;
    public RaycastHit rayHit;
    public LayerMask hittable;


    [Header("Polish")]

    public GameObject muzzleFlash, bulletHole, enemyHole, defaultPosition, aimPosition;
    public AudioSource audioSource;
    public TextMeshProUGUI text;



    [Header("Recoil")]

    public bool resettingWeapon;

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
    public void ReceiveAimInput(bool _isAiming)
    {
        isAiming = _isAiming;

        if (isAiming)
        {
            Aim();
        }
        else if (!isAiming)
        {
            OutAim();
        }

    }

    private void Start()
    {

        amountOfAmmoType = ammoManager.GetAmmoCount(type);
        ReloadingFinished();
        Vector3 weaponPosition = transform.position;
        Vector3 scale = transform.localScale;
        transform.SetParent(defaultPosition.transform, false);
        transform.localScale = scale;

        Vector3.Lerp(weaponPosition, defaultPosition.transform.position, aimAnimationSpeed * Time.deltaTime);

    }
    private void Awake()
    {
        fpsCam = Camera.main;
        readyToShoot = true;
        spreadHolder = spread;

        if (this.gameObject.TryGetComponent<Renderer>(out Renderer renderer))
            renderer.material.SetFloat("_startClue", 1f);
    }

    private void Update()
    {
        ammoLeft = ammoManager.GetAmmoCount(type);

        text.SetText(bulletsLeftInMagazine + " / " + ammoLeft);
        if (weaponIsActive)
            MouseLook.slotFull = true;

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
    public void ReceiveInput(bool _isLeftMouseHeld)
    {
        isLeftMouseHeld = _isLeftMouseHeld;
        if (!reloading && isLeftMouseHeld && gameObject.activeInHierarchy && bulletsLeftInMagazine > 0)
        {
            Shoot();
        }
    }
    private void Shoot()
    {
        if (Time.time > nextFireTime)
        {
            Instantiate(muzzleFlash, attackPoint.position, attackPoint.rotation);


            bulletsActuallyFired = Mathf.Min(bulletsPerTap, bulletsLeftInMagazine);
            bulletsShot = bulletsActuallyFired;
            bulletsLeftInMagazine -= bulletsActuallyFired;


            for (int i = 0; i < bulletsActuallyFired; i++)
            {
                nextFireTime = Time.time + fireRate;

                float x = Random.Range(-spread, spread);
                float y = Random.Range(-spread, spread);

                Vector3 direction = fpsCam.transform.forward + new Vector3(x, y, 0);

                if (Physics.Raycast(fpsCam.transform.position, direction, out rayHit, range, hittable))
                {
                    if (rayHit.collider != null)
                    {
                        if (rayHit.collider.CompareTag("Enemy"))
                        {
                            if (rayHit.collider.gameObject.TryGetComponent<NewEnemyAI>(out NewEnemyAI newEnemyAI))
                            {
                                newEnemyAI.TakeDamage(damage);

                                // Vector3 forceDirection = rayHit.collider.transform.position - transform.position;
                                // rayHit.collider.transform.position += direction.normalized * pushForce * Time.deltaTime;
                                // if (null != enemyHole)
                                //     Destroy((Instantiate(enemyHole, rayHit.point + (rayHit.normal * 0.0005f), Quaternion.FromToRotation(Vector3.up, rayHit.normal), rayHit.transform)), 4);
                            }
                        }
                        else if (rayHit.collider.CompareTag("Interactable"))
                        {
                            Interactable interactable = rayHit.collider.GetComponent<Interactable>();
                            if (interactable.isItDestrucable == true)
                            {
                                interactable.destroyObject(damage);
                                Vector3 forceDirection = rayHit.collider.transform.position - transform.position;
                                rayHit.collider.transform.position += direction.normalized * pushForce * Time.deltaTime;
                            }
                            Destroy((Instantiate(bulletHole, rayHit.point + (rayHit.normal * 0.0005f), Quaternion.FromToRotation(Vector3.up, rayHit.normal), rayHit.transform)), 4);
                        }
                        else
                        {
                            Destroy((Instantiate(bulletHole, rayHit.point + (rayHit.normal * 0.0005f), Quaternion.FromToRotation(Vector3.up, rayHit.normal))), 4);
                        }
                    }



                }

            }
            AlarmEnemies();
            if (!resettingWeapon)
            {
                resettingWeapon = true;
                StartCoroutine(Recoil());
            }
            else
            {
                StopCoroutine("Recoil");
                StartCoroutine(Recoil());
            }

        }
    }


    private IEnumerator Recoil()
    {
        Vector3 recoilVector = new Vector3(0, 0, 1f);
        Vector3 recoilPosition = Vector3.zero - recoilVector;

        Quaternion recoilRotationAmount = Quaternion.Euler(
                    Quaternion.identity.x - 50,
                    Quaternion.identity.y,
                    Quaternion.identity.z
                );

        Quaternion zeroRotationAmount = Quaternion.Euler(
            Quaternion.identity.x,
            Quaternion.identity.y,
            Quaternion.identity.z
        );

        this.transform.localRotation = Quaternion.Lerp(this.transform.localRotation, recoilRotationAmount, 0.1f);
        this.transform.localPosition = Vector3.Lerp(this.transform.localPosition, recoilPosition, 0.1f);


        yield return new WaitForSeconds(0.2f);

        this.transform.localRotation = Quaternion.Lerp(this.transform.localRotation, zeroRotationAmount, 0.3f);
        this.transform.localPosition = Vector3.Lerp(this.transform.localPosition, Vector3.zero, 0.2f);

        yield return new WaitForSeconds(0.4f);
        this.transform.localRotation = Quaternion.identity;

        resettingWeapon = false;
    }

    public void AlarmEnemies()
    {
        Collider[] enemiesInRange = Physics.OverlapSphere(transform.position, range);


        foreach (Collider enemy in enemiesInRange)
        {
            if (enemy.CompareTag("Enemy"))
            {
                NewEnemyAI enemyAI = enemy.GetComponent<NewEnemyAI>();
                if (enemyAI != null)
                {
                    enemyAI.CheckNoise(this.transform.position);
                }
            }
        }
    }

    public void OnReloadPressed()
    {
        if (ammoLeft > 0 && bulletsLeftInMagazine < magazineSize && !reloading)
        {
            audioSource.Play();
            reloading = true;
            Invoke("ReloadingFinished", reloadTime);
        }

    }

    private void ReloadingFinished()
    {
        amountOfAmmoType = ammoManager.GetAmmoCount(type);
        int bulletsToFullMagazine = (magazineSize - bulletsLeftInMagazine);

        if (bulletsToFullMagazine <= amountOfAmmoType)
        {
            bulletsLeftInMagazine = magazineSize;
            ammoManager.UseAmmo(type, bulletsToFullMagazine);
        }
        else if (bulletsToFullMagazine >= amountOfAmmoType)
        {
            bulletsLeftInMagazine = bulletsLeftInMagazine + amountOfAmmoType;
            ammoManager.UseAmmo(type, amountOfAmmoType);
        }
        reloading = false;
    }

}