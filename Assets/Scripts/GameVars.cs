using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/// <summary>
/// Static class containing Game variables
/// </summary>
public class GameVars {
    public static List<Vector3> waypoints;
    public const float Z_COORD = -1f;

    public static GameObject turretToPlace;
    public static GameObject wallToPlaceTurret;
	public static GameObject selectedTurret;

    public static void init() { waypoints = new List<Vector3>(); }
}