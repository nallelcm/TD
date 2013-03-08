using UnityEngine;
using System.Collections;

public class WallSpace : MonoBehaviour 
{
    public GameObject turret;
    private Color defaultColor;
    private bool highlight;
	void Start () {
        turret = null;
        defaultColor = renderer.material.color;
        highlight = false;
	}
	void Update () 
    {
        if (GameVars.wallToPlaceTurret == gameObject) 
		{ 
			renderer.material.color = Color.white; 
		}
        else if ((GameVars.selectedTurret == this.turret) && (this.turret != null))
        { 
			renderer.material.color = Color.red; 
		}
        else if (highlight)
        { 
			renderer.material.color = Color.white; 
		}
        else 
		{ 
			renderer.material.color = defaultColor; 
		}


	}
    public void AddTurret(GameObject t)
    {
        if (turret == null)
        {
            t.transform.position = new Vector3(transform.position.x, transform.position.y, GameVars.Z_COORD);
            GameObject turretInstance = Instantiate(t) as GameObject;
			turret = turretInstance;
            turretInstance.transform.parent = transform;
            defaultColor = Color.yellow;
            renderer.material.color = defaultColor;
        }
    }
    public void OnMouseOver()
    {
        //(gameObject.GetComponent("Halo") as Behaviour).enabled = true;
        if ((GameVars.turretToPlace != null) && (GameVars.wallToPlaceTurret == null) && (this.turret == null)) { highlight = true; }
        if (this.turret != null) { BroadcastMessage("ShowRange"); }
    }
    public void OnMouseExit()
    {
        highlight = false;
        if (this.turret != null) { BroadcastMessage("HideRange"); }
    }
    public void OnMouseDown()
    {
        if (GameVars.turretToPlace != null) { GameVars.wallToPlaceTurret = gameObject; }
        else
        {
            Debug.Log("TEST");
            GameVars.selectedTurret = this.turret;

        }
    }
}
