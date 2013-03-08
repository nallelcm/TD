using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Enemy : MonoBehaviour
{
    public float speed;
    private float hp;
    private float maxHP;
    public string hitAnimation;
    public string moveAnimation;
    public string deathAnimation;
    public int currentWaypoint;
    void Start()
    {
        maxHP = 100f;
        hp = maxHP;
        currentWaypoint = 1;
    }
    public float CurrentHP { get { return hp; } }
    void FixedUpdate()
    {
        if (hp > 0)
        {
            transform.position = Vector3.MoveTowards(transform.position, GameVars.waypoints[currentWaypoint], speed * Time.deltaTime);
            if (!animation.isPlaying) { animation.Play(moveAnimation); }
            if (transform.position == GameVars.waypoints[currentWaypoint])
            {
                if (currentWaypoint == GameVars.waypoints.Count - 1) { Kill(); }
                else
                {
                    currentWaypoint++;
                    transform.LookAt(GameVars.waypoints[currentWaypoint], Vector3.back);
                }
            }
        }
    }
    void TakeDamage(float damage)
    {
        if (!animation.IsPlaying(hitAnimation)) { animation.Play(hitAnimation); }
        this.hp -= damage;
        if (hp <= 0) { StartCoroutine("Kill"); }
    }
    private IEnumerator Kill()
    {
        tag = "Untagged";
        animation.Play(deathAnimation);
        //Instantiate(Resources.Load("Environment/Detonator-Base"), this.transform.position, Quaternion.identity);
        yield return new WaitForSeconds(5);
        Destroy(this.gameObject);
    }
    void OnGUI()
    {
        int hpWidth = 25;
        Vector3 pos = Camera.mainCamera.WorldToScreenPoint(this.transform.position + new Vector3(0, 0, -.5f));
        //GUI.Box (new Rect(pos.x-hpWidth/2,Screen.height - pos.y,hpWidth,5), "");
        //@TODO: fix this shit
        //GUI.Box (new Rect(pos.x-hpWidth/2,Screen.height - pos.y,(int)(hp/maxHP*hpWidth), 5), "");
    }
}