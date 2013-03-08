using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class GameManager : MonoBehaviour 
{   
	void Start () 
    {
        GameVars.init();
        GenerateLevel level = new GenerateLevel(5, 30);
        //foreach (Resource wall in level.resources) { Instantiate(Resources.Load(wall.resource), wall.location, wall.angle); }
        StartCoroutine("SpawnWave", new Wave());
	}
    void Update() 
    {
        if (Input.GetButtonUp("Jump")) { ChangeCameraAngle(); }
		if (Input.GetButton("Fire2")) { SpawnEnemy("Monster"); }
        Camera.mainCamera.transform.Translate(Input.GetAxisRaw("Horizontal") * Time.deltaTime * 20, Input.GetAxisRaw("Vertical") * Time.deltaTime * 20, Input.GetAxis("Mouse ScrollWheel") * 10, Space.World);
        Camera.mainCamera.orthographicSize -= Input.GetAxis("Mouse ScrollWheel")*10;
	}
    /// <summary>
    /// Changes the camera between 3d and top down
    /// </summary>
    void ChangeCameraAngle()
    {
        Camera.mainCamera.orthographic = !Camera.mainCamera.orthographic;
        if (Camera.mainCamera.orthographic) { Camera.mainCamera.transform.transform.localRotation = Quaternion.AngleAxis(0f, Vector3.left); }
        else { Camera.mainCamera.transform.transform.localRotation = Quaternion.AngleAxis(30f, Vector3.left); }
    }
    /// <summary>
    /// Spawns a wave of enemies
    /// </summary>
    /// <param name="wave">a Wave object containing enemies</param>
    /// <returns>A time in seconds to wait</returns>
    IEnumerator SpawnWave(Wave wave)
    {
        foreach (string enemy in wave.wave)
        {
            SpawnEnemy(enemy);
            yield return new WaitForSeconds(2);
        }
    }
    /// <summary>
    /// Spawns a single enemy
    /// </summary>
    /// <param name="enemyResource">name of prefab in "Enemies" folder</param>
    void SpawnEnemy(string enemyResource)
    {
        GameObject enemy = Resources.Load("Enemies/" + enemyResource) as GameObject;
        enemy.transform.position = GameVars.waypoints[0];
        Instantiate(enemy);
    }
}
