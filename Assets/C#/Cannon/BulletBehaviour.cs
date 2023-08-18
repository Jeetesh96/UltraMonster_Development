using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    void Update()
    {
        int layerMask = 1 << 6;
        Vector3 from = transform.GetChild(0).transform.position;
        RaycastHit hit;
        if(Physics.Raycast(from , Vector3.forward , out hit , Mathf.Infinity , layerMask))
        {
            //Debug.Log(hit.collider);
            hit.collider.gameObject.GetComponent<FishController>().RecieveDamage();
            GameObject webGameObject = Resources.Load<GameObject>("Web");
            //Instantiate(webGameObject, this.transform.position, this.transform.rotation);
            WebEffectPool.instance.GetObjectFromPool(this.transform.position);
            transform.parent.GetComponent<Canon>().PutObjectInPool(gameObject);
        }

    }
}
