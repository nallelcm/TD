using UnityEngine;
using System.Collections;

public class Lobbob : Projectile {

    private Vector3 targetLocation;
    void OnEnable()
    {
        speed = 10f;
        height = -5f;
        targetLocation = target.transform.position;
    }
    void Start()
    {
        
    }
    void FixedUpdate()
    {
        if (target != null)
        {
            //transform.transform.Translate(Vector3.forward * speed * Time.deltaTime);
            //transform.position = Vector3.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);
            currentDistance = Vector3.Distance(transform.position, target.transform.position);
            if (transform.position.z < -4f) { height = 0; }
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(targetLocation.x, targetLocation.y, height), speed * Time.deltaTime);
            if (transform.position == targetLocation) { Destroy(this.gameObject); }
        }
        else { Destroy(this.gameObject); }
    }
}
