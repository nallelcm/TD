using UnityEngine;
using System.Collections;

public class Shot : Projectile {

    void OnEnable()
    {
        speed = 20f;
        height = -3f;
    }
    void FixedUpdate()
    {
        if (target != null)
        {
            //transform.transform.Translate(Vector3.forward * speed * Time.deltaTime);
            //transform.position = Vector3.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);
            currentDistance = Vector3.Distance(transform.position, target.transform.position);
            if (transform.position.z < -2f || currentDistance < startDistance / 2)
                height = 0;
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(target.transform.position.x, target.transform.position.y, height), speed * Time.deltaTime);
        }
        else { Destroy(this.gameObject); }
    }

}
