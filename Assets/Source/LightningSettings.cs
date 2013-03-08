using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class LightningSettings : MonoBehaviour {
	private Color color;
	private float minBrightness;
	private float maxBrightness;
	private float sharpness;
	private float motionRate;
	private float frequency;
	private float scale;
	
	private bool shouldExecute = false;
	public void Start()
	{
		var localMaterial = gameObject.renderer.material;
		color = localMaterial.GetColor( "_Color" );
		minBrightness = localMaterial.GetFloat( "_Brightness_Min" );
		maxBrightness = localMaterial.GetFloat( "_Brightness_Max" );
		sharpness = localMaterial.GetFloat( "_ElectricitySharpness" );
		motionRate = localMaterial.GetFloat( "_NoiseMotionRate" );
		frequency = localMaterial.GetFloat( "_NoiseFrequency" );
		scale = localMaterial.GetFloat( "_NoiseScale" );
		shouldExecute = true;
	}
	
	public void Update()
	{
		if( !shouldExecute ) 
			return;
	
			var localMaterial = gameObject.renderer.material;
			localMaterial.SetColor( "_Color", color );
			localMaterial.SetFloat( "_Brightness_Min", minBrightness );
			localMaterial.SetFloat( "_Brightness_Max", maxBrightness );
			localMaterial.SetFloat( "_ElectricitySharpness", sharpness );
			localMaterial.SetFloat( "_NoiseMotionRate", motionRate );
			localMaterial.SetFloat( "_NoiseFrequency", frequency );
			localMaterial.SetFloat( "_NoiseScale", scale );
	}
	
	public void DrawGUI()
	{
		GUI.Box( new Rect( Screen.width - 270, 30, 260, 250 ), "" );
		GUILayout.BeginArea( new Rect( Screen.width - 270, 30, 260, 250 ) );
		GUILayout.Label( "Color:" );
		
		GUILayout.BeginHorizontal();
		GUILayout.Label( "R:" );
		color.r = GUILayout.HorizontalSlider( color.r, 0f, 1f, GUILayout.Width( 150 ) );
		GUILayout.EndHorizontal();;
		
		GUILayout.BeginHorizontal();
		GUILayout.Label( "G:" );
		color.g = GUILayout.HorizontalSlider( color.g, 0f, 1f, GUILayout.Width( 150 ) );
		GUILayout.EndHorizontal();;
		
		GUILayout.BeginHorizontal();
		GUILayout.Label( "B:" );
		color.b = GUILayout.HorizontalSlider( color.b, 0f, 1f, GUILayout.Width( 150 ) );
		GUILayout.EndHorizontal();
		
		GUILayout.BeginHorizontal();
		GUILayout.Label( "Min Brightness:" );
		minBrightness = GUILayout.HorizontalSlider( minBrightness, 0f, 3f, GUILayout.Width( 150 ) );
		GUILayout.EndHorizontal();
		
		GUILayout.BeginHorizontal();
		GUILayout.Label( "Max Brightness:" );
		maxBrightness = GUILayout.HorizontalSlider( maxBrightness, 0f, 3f, GUILayout.Width( 150 ) );
		GUILayout.EndHorizontal();
		
		GUILayout.BeginHorizontal();
		GUILayout.Label( "Sharpness:" );
		sharpness = GUILayout.HorizontalSlider( sharpness, 0f, 10f, GUILayout.Width( 150 ) );
		GUILayout.EndHorizontal();
		
		GUILayout.BeginHorizontal();
		GUILayout.Label( "Motion Rate:" );
		motionRate = GUILayout.HorizontalSlider( motionRate, 0f, 2f, GUILayout.Width( 150 ) );
		GUILayout.EndHorizontal();
		
		GUILayout.BeginHorizontal();
		GUILayout.Label( "Frequency:" );
		frequency = GUILayout.HorizontalSlider( frequency, 0f, 2f, GUILayout.Width( 150 ) );
		GUILayout.EndHorizontal();
		
		GUILayout.BeginHorizontal();
		GUILayout.Label( "Scale:" );
		scale = GUILayout.HorizontalSlider( scale, 0f, 3f, GUILayout.Width( 150 ) );
		GUILayout.EndHorizontal();
		GUILayout.FlexibleSpace();
		GUILayout.EndArea();
	}
}
