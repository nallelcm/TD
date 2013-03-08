using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public enum TargetMode { Farthest=0, Soonest, Weakest, Random }; 
public class Tower : MonoBehaviour {
	public TargetMode targetingMode;
	public int numberOFTargets;
    public float MaxRange;
    public float AttackSpeed;
    public float damage;
    protected int level;
    public GameObject projectile;
	public string name;
    private LineRenderer lineRenderer;
    protected float nextAttack;

	private float currentDmg;
	private float currentSpd;
	public int maxPoints;
    
	// Use this for initialization
	void Start () {
        level = 1;
		maxPoints = 100;
        ResetAttackTimer();
		lineRenderer = gameObject.AddComponent<LineRenderer>();
        CreateRange();
	}
    protected void CreateRange()
    {
        int size = 180;
        
        lineRenderer.material = new Material(Shader.Find("Particles/Alpha Blended"));
        lineRenderer.SetColors(new Color(1, 1, 1, .5f), new Color(1, 1, 1, .5f));
        lineRenderer.SetWidth(.1f, .1f);
        lineRenderer.SetVertexCount(size + 1);
        
        float x, y;
        float theta = 0;
        for (int i = 0; i <= size; i++)
        {
            theta += Mathf.PI * 2 / size;
            x = (MaxRange * Mathf.Cos(theta)) + transform.position.x;
            y = (MaxRange * Mathf.Sin(theta)) + transform.position.y;

            Vector3 pos = new Vector3(x, y, GameVars.Z_COORD);
            lineRenderer.SetPosition(i, pos);
        }
        HideRange();
    }
    public void ShowRange() 
	{ 
		lineRenderer.enabled = true; 
	}
    public void HideRange() 
	{ 
		lineRenderer.enabled = false; 
	}
    void FixedUpdate()
    {
        if ((Time.time >= nextAttack) || (Speed == 0))
        {
			Enemy[] targets = GetTarget ();
            foreach (Enemy target in targets) 
			{ 
				// I cant read code when its all one one line
				Shoot(target.gameObject); 
			}
        }
		if (GameVars.selectedTurret == this.gameObject)
		{
			ShowRange();
		}
		else
		{
			HideRange();
		}
    }
	protected Enemy[] GetTarget()
	{
		List<Enemy> enemies = new List<Enemy> ();
		Collider[] enemiesInRange = Physics.OverlapSphere(transform.position, Range);

		foreach (Collider enemyCollider in enemiesInRange)
		{
			GameObject enemy = enemyCollider.gameObject;
            if (enemy.tag == "Enemy") 
			{ 
				enemies.Add(enemy.GetComponent<Enemy>()); 
			}
		}

		switch(targetingMode)
		{
			case TargetMode.Soonest:
                enemies.Sort(delegate(Enemy x, Enemy y) { return x.currentWaypoint.CompareTo(y.currentWaypoint); });
				break;
			case TargetMode.Farthest:
                enemies.Sort(delegate(Enemy x, Enemy y) { return y.currentWaypoint.CompareTo(x.currentWaypoint); });
				break;
			case TargetMode.Weakest:
                enemies.Sort(delegate(Enemy x, Enemy y) { return y.CurrentHP.CompareTo(x.CurrentHP); });
				break;
			// not touching random ATM, but since i dont know how the colliders
			// get returned, that's random enough for me :: lolwat
		}
        //will return the full list of enemies or the max number of targers
        return enemies.GetRange(0, (numberOFTargets > enemies.Count) ? enemies.Count : numberOFTargets).ToArray();

	}
    protected void Shoot(GameObject enemy)
    {
		if ((AttackSpeed == 0) && (transform.childCount >= numberOFTargets)) return; 

	    projectile.GetComponent<Projectile>().target = enemy;

	    projectile.transform.position = transform.position;

       	GameObject shotInstance = Instantiate(projectile) as GameObject;
        shotInstance.transform.parent = this.transform;

        ResetAttackTimer();
    }
    protected void ResetAttackTimer() { nextAttack = Time.time + 20f/AttackSpeed; }
    public float Range 
    { 
        get { return MaxRange; }
        set 
		{ 
			MaxRange = value; 
			CreateRange();
		}
    }
    public float Speed 
    { 
        get { return AttackSpeed; }
        set 
        {
            ResetAttackTimer();
            AttackSpeed = value; 
        }
    }
    public float Damage 
    {
        get { return damage; }
        set { damage = value; } 
    }
    protected void ShotHit(GameObject target) 
	{ 
		if (target != null) { target.BroadcastMessage("TakeDamage", damage); } 
	} // if something else hasn't killed it
}