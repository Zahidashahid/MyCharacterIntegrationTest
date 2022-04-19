using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    PlayerController controls;
    public int damage = 40;
    public Transform firePoint;
    public GameObject impactEffect;
    public LineRenderer lineRenderer;
    void Awake()
    {
        controls = new PlayerController();
        controls.Gameplay.GunShoot.performed += ctx => Shoot();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void Shoot()
    {
        RaycastHit2D hitInfo = Physics2D.Raycast(firePoint.position, firePoint.right);
        if (hitInfo)
        {
            Debug.Log(hitInfo.transform.name);
            SkeletonEnemyMovement enemy = hitInfo.transform.GetComponent<SkeletonEnemyMovement>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
           GameObject impactGameObject = Instantiate(impactEffect, hitInfo.point, Quaternion.identity);
            // Instantiate(projectile, shootPoint.position, transform.rotation);
            Debug.Log(firePoint.position);
            lineRenderer.SetPosition(0, firePoint.position);
            lineRenderer.SetPosition(1, hitInfo.point);
            Destroy(impactGameObject, 3f);
        }
        else
        {
            lineRenderer.SetPosition(0, firePoint.position);
            lineRenderer.SetPosition(1, firePoint.position + firePoint.right * 70);
        }
        StartCoroutine(ShootLineRenderer());
        
    }
    IEnumerator ShootLineRenderer()
    {
        lineRenderer.enabled = true;
        yield return new WaitForSeconds(0.4f);
        lineRenderer.enabled = false;
    }
    private void OnEnable()
    {
        controls.Gameplay.Enable();
    }
    private void OnDisable()
    {
        controls.Gameplay.Disable();
    }
}
