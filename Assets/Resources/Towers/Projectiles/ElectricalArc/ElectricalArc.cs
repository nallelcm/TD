using UnityEngine;
using System.Collections;

public class ElectricalArc : Projectile {
    void Start()
    {
        transform.right = transform.parent.position - target.transform.position;
        currentDistance = Vector3.Distance(transform.position, target.transform.position);
        transform.localScale = new Vector3(currentDistance * 0.05f, currentDistance * 0.05f, currentDistance * 0.05f); 
    }
    void OnEnable()
    {
        speed = 50f;  
    }
    void FixedUpdate()
    {
        
        if (target != null && target.tag == "Enemy")
        {
            transform.right = transform.parent.position - target.transform.position;
            currentDistance = Vector3.Distance(transform.position, target.transform.position);
            transform.localScale = new Vector3(currentDistance * 0.05f, currentDistance * 0.05f, currentDistance * 0.05f);
            gameObject.SendMessageUpwards("ShotHit", target);
            if (currentDistance > transform.parent.gameObject.GetComponent<Tower>().Range) { Destroy(this.gameObject); }
        }
        else { Destroy(this.gameObject); }
    }
}
