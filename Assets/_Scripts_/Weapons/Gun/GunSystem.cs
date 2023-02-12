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

    [Header("Ammo stats")]
    public AmmoType type;
    public int ammoUsage;
    public int bulletsActuallyFired;
    public AmmoManager ammoManager;
    public int amountOfAmmoType;
    int ammoLeft;


    [Header("Gun stats")]
    public int damage, magazineSize, bulletsPerTap;
    public float timeBetweenShooting, spread, aimSpread, spreadHolder, range, reloadTime, timeBetweenShots, aimAnimationSpeed, pushForce, shootWaveRange;
    int bulletsLeftInMagazine, bulletsShot;
    bool reloading, isLeftMouseHeld, isAiming, wasAiming;
    public bool turnOffCanvas, readyToShoot, coroutineUsed;
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
    public void OnShootPressed()
    {
        // startShooting = true;
        // readyToShoot = true;
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

    }

    private void Start()
    {
        amountOfAmmoType = ammoManager.GetAmmoCount(type);
        ReloadingFinished();

    }
    private void Awake()
    {

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
        Debug.Log("ReceiveInput called");
        if (!reloading && isLeftMouseHeld && gameObject.activeInHierarchy && readyToShoot && bulletsLeftInMagazine > 0)
        {
            Debug.Log("ReceiveInput ifs went fine");
            Shoot();
        }
    }
    private void Shoot()
    {
        Debug.Log("shoot call");
        readyToShoot = false;
        Instantiate(muzzleFlash, attackPoint.position, attackPoint.rotation);
        AlarmEnemies();

        // Vector3 recoil = isAiming ? aimRecoilAmout : recoilAmout;
        // transform.rotation = Quaternion.Euler(recoil, 0, 0);
        // StartCoroutine(Return());

        bulletsActuallyFired = Mathf.Min(bulletsPerTap, bulletsLeftInMagazine);
        bulletsShot = bulletsActuallyFired;

        for (int i = 0; i < bulletsActuallyFired; i++)
        {
            float x = Random.Range(-spread, spread);
            float y = Random.Range(-spread, spread);

            Vector3 direction = fpsCam.transform.forward + new Vector3(x, y, 0);

            if (Physics.Raycast(fpsCam.transform.position, direction, out rayHit, range, hittable))
            {
                if (rayHit.collider.CompareTag("Enemy"))
                {
                    NewEnemyAI enemy = rayHit.collider.GetComponent<NewEnemyAI>();
                    enemy.TakeDamage(damage);
                    Vector3 forceDirection = rayHit.collider.transform.position - transform.position;
                    rayHit.collider.transform.position += direction.normalized * pushForce * Time.deltaTime;
                    Destroy((Instantiate(enemyHole, rayHit.point + (rayHit.normal * 0.0005f), Quaternion.FromToRotation(Vector3.up, rayHit.normal), rayHit.transform)), 4);
                }
                else if (rayHit.collider.CompareTag("Interactable"))
                {
                    Interactable interactable = rayHit.collider.GetComponent<Interactable>();
                    interactable.destroyObject(damage);
                    Vector3 forceDirection = rayHit.collider.transform.position - transform.position;
                    rayHit.collider.transform.position += direction.normalized * pushForce * Time.deltaTime;
                    Destroy((Instantiate(bulletHole, rayHit.point + (rayHit.normal * 0.0005f), Quaternion.FromToRotation(Vector3.up, rayHit.normal), rayHit.transform)), 4);
                }
                else
                {
                    Destroy((Instantiate(bulletHole, rayHit.point + (rayHit.normal * 0.0005f), Quaternion.FromToRotation(Vector3.up, rayHit.normal))), 4);
                }
            }

            bulletsLeftInMagazine--;
            if (bulletsShot > 0)
                bulletsShot--;
        }
        coroutineUsed = false;
        if (bulletsShot == 0)
            StartCoroutine(ResetShoot());
    }

    private IEnumerator ResetShoot()
    {
        yield return new WaitForSeconds(timeBetweenShooting);
        coroutineUsed = true;
        readyToShoot = true;
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
        Collider[] enemiesInRange = Physics.OverlapSphere(transform.position, range);


        foreach (Collider enemy in enemiesInRange)
        {
            if (enemy.CompareTag("Enemy"))
            {
                enemy.GetComponent<NewEnemyAI>().CheckNoise(this.transform.position);
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