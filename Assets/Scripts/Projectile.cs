using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour
{
    public GameObject target;
    protected float speed;
    public float range;
    protected float startDistance;
    protected float currentDistance;
    protected float height;
    void Start()
    {
        transform.LookAt(target.transform);
        startDistance = Vector3.Distance(transform.position, target.transform.position);
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            gameObject.SendMessageUpwards("ShotHit", other.gameObject);
            Destroy(this.gameObject);
        }
    }
}