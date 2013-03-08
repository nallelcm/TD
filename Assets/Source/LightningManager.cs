using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class LightningManager : MonoBehaviour {
	private List<LightningSettings> arcs;
	
	public void Start()
	{
		arcs = (FindObjectsOfType(typeof(LightningSettings)) as LightningSettings[]).ToList();
	}
	
	private int selected = 0;
	public void OnGUI()
	{
		GUILayout.BeginArea( new Rect( Screen.width - 270, 5, 260, 25 ) );

		var arcNames = new List<string>();
		foreach( var arc in arcs )
		{
			arcNames.Add( "Arc " + arcs.IndexOf( arc ) );
		}
		selected = GUILayout.Toolbar( selected, arcNames.ToArray() );
		
		GUILayout.EndArea();
		arcs[selected].DrawGUI();
		
	}
}
