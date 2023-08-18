using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonParentClass : MonoBehaviour
{
    protected internal List<GameObject> bulletPool = new List<GameObject>();
    [SerializeField] protected internal float firingRate;
    [SerializeField] protected Camera cam;
    [SerializeField] protected GameObject turret;
    [SerializeField] protected float turretTurnSpeed;
    [SerializeField] protected Transform firingPoint;


    [SerializeField] protected GameObject bulletPrefab;
    [SerializeField] protected int bulletPoolAmount;

    [SerializeField] protected float bulletSpeed;


    #region Bullet Pooling
    protected internal void CreateBulletPool()
    {
        for (int i = 0; i < bulletPoolAmount; i++)
        {
            GameObject gb = Instantiate(bulletPrefab, transform);
            gb.SetActive(false);
            bulletPool.Add(gb);
        }
    }

    protected internal GameObject GetBulletFromPool()
    {
        foreach (GameObject bullet in bulletPool)
        {
            if (!bullet.activeInHierarchy)
                return bullet;
        }

        return null;
    }

    public void PutObjectInPool(GameObject _bullet)
    {
        _bullet.SetActive(false);
        _bullet.transform.position = Vector3.zero;
        _bullet.transform.rotation = Quaternion.identity;
    }
    #endregion


    float t = 0;
    protected internal void Firing()
    {
            t += Time.deltaTime;
            if (t > firingRate)
            {
                //Debug.Log("firing");
                GameObject bullet = GetBulletFromPool();

                if (bullet != null)
                {
                    bullet.transform.position = firingPoint.position;
                    bullet.transform.rotation = firingPoint.rotation;
                    bullet.SetActive(true);
                    bullet.GetComponent<Rigidbody2D>().AddForce(firingPoint.up * bulletSpeed, ForceMode2D.Impulse);
                    bullet.AddComponent<BulletBehaviour>();
                    t = 0;
                }

            }
       
    }

    float maxLength = 0;

    protected internal void Firing(LineRenderer laser, float laserDistance)
    {
        laser.gameObject.SetActive(true);
        laser.SetPosition(1, new Vector3(laserDistance, 0, 0));
       
    }


    public void IncreaseFireRate()
    {
        Debug.Log("Increased");
        if (firingRate > 0.08f)
            firingRate -= 0.05f;
        else
            firingRate = 0.08f;

    }

    public void DecreaseFireRate()
    {
        if (firingRate < 0.2f)
            firingRate += 0.05f;
        else
            firingRate = 0.2f;
    }

    protected virtual void ControlTurret()
    {
    }

    protected virtual void Update()
    {
       
    }

}
