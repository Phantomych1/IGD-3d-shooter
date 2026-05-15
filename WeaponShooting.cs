using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.UI;

public class WeaponShooting : MonoBehaviour
{
    [Header("Патроны и Перезарядка")]
    public int maxAmmo = 30;
    private int currentAmmo;
    public float reloadTime = 4.5f;
    private bool isReloading = false;

    [Header("Интерфейс")]
    public Slider hpSlider;
    public TextMeshProUGUI ammoDisplay;

    [Header("Настройки оружия")]
    public float range = 100f;
    public float damage = 20f;
    public float fireRate = 0.1f;
    private float nextTimeToFire = 0f;

    [Header("Ссылки")]
    public Animator anim;
    public Transform muzzle;
    public Light flashLight;
    public LineRenderer tracer;

    void Start()
    {
        currentAmmo = maxAmmo;
        UpdateUI();
    }

    void Update()
    {
        if (isReloading) return;

        if (currentAmmo <= 0)
        {
            StartCoroutine(Reload());
            return;
        }

        if (Input.GetKeyDown(KeyCode.R) && currentAmmo < maxAmmo)
        {
            StartCoroutine(Reload());
            return;
        }

        if (Input.GetMouseButton(0) && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + fireRate;
            StartCoroutine(SimpleShoot());
        }
    }

    IEnumerator SimpleShoot()
    {
        currentAmmo--;
        UpdateUI();

        if (anim != null)
        {
            try { anim.SetTrigger("Shoot"); } catch { }
        }

        if (flashLight != null) flashLight.enabled = true;

        RaycastHit hit;
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        Vector3 endPoint;

        if (Physics.Raycast(ray, out hit, range))
        {
            endPoint = hit.point;

            EnemyHealth target = hit.collider.GetComponentInParent<EnemyHealth>();
            if (target == null) target = hit.collider.GetComponentInChildren<EnemyHealth>();

            if (target != null)
            {
                target.TakeDamage(damage);
            }
        }
        else
        {
            endPoint = ray.GetPoint(range);
        }

        if (tracer != null)
        {
            tracer.enabled = true;
            tracer.SetPosition(0, muzzle.position);
            tracer.SetPosition(1, endPoint);
        }

        yield return new WaitForSeconds(0.05f);

        if (flashLight != null) flashLight.enabled = false;
        if (tracer != null) tracer.enabled = false;
    }

    IEnumerator Reload()
    {
        isReloading = true;

        if (anim != null)
        {
            try { anim.SetTrigger("Reload"); } catch { }
        }

        yield return new WaitForSeconds(reloadTime);
        currentAmmo = maxAmmo;
        isReloading = false;
        UpdateUI();
    }

    void UpdateUI()
    {
        if (ammoDisplay != null) ammoDisplay.text = currentAmmo + " / " + maxAmmo;
    }
}