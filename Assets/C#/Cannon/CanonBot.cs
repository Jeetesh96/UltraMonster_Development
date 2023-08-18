using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanonBot : CannonParentClass
{

    [SerializeField] float turnRate;

    private void Start()
    {
        CreateBulletPool();
        //StartCoroutine(FiringControl());
    }


    float turnT = 0;
    override protected void ControlTurret()
    {
        turnT += Time.deltaTime;
        if(turnT > turnRate)
        {
            int random = Random.Range(-75, 75);
            turret.transform.localRotation = Quaternion.Euler(0 , 0, random);
            turnT = 0;
        }
    }

    IEnumerator FiringControl()
    {
        while(true)
        {
            float random = Random.Range(4, 8);
            yield return new WaitForSeconds(random);
            Firing();
        }
        
    }

    protected override void Update()
    {
        Firing();
        ControlTurret();
    }
}
