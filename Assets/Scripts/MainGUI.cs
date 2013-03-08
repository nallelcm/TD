using UnityEngine;
using System.Collections;
/// <summary>
/// Handles all the mainscreen GUI drawing: the top bar, bottom left/right boxes, and bottom middle details box and the turret placement GUI
/// </summary>
public class MainGUI : MonoBehaviour {
    private TargetMode targetMode;
    private string[] selectionStrings = { "Farthest", "Soonest", "Lowest", "Random" };
    private float damage;
    private float speed;
    private float range;
    private int buttonHeight = 20;
    private int buttonWidth = 130;
    public Texture2D bgImage;
    public Texture2D midImage;
    public Texture2D topImage;
    public GUISkin center;
    public GUISkin leftRight;
    public GUISkin top;
    void OnGUI()
    {
        Top();
        BottomLeft();
        BottomMiddle();
        BottomRight();
        if (GameVars.turretToPlace != null) { PlaceTurret(); }
    }
    private void PlaceTurret()
    {
        GUI.Box(new Rect(Screen.width - 150, 0, 150, 85), "PLACE TURRET");
        GUI.Box(new Rect(Screen.width - 150, 25, 150, 25), GameVars.turretToPlace.name);
        if (GUI.Button(new Rect(Screen.width - 120, 55, 100, 25), "Cancel")) { GameVars.turretToPlace = GameVars.wallToPlaceTurret = null; }
        if (GameVars.wallToPlaceTurret != null)
        {
            Vector2 point = Camera.main.WorldToScreenPoint(GameVars.wallToPlaceTurret.transform.position);

            if (GUI.Button(new Rect(point.x - 50, Screen.height - point.y, 100, 25), "Place"))
            {
                GameVars.wallToPlaceTurret.BroadcastMessage("AddTurret", GameVars.turretToPlace);
                GameVars.wallToPlaceTurret = GameVars.turretToPlace = null;
            }
        }
    }
    private void Top()
    {
        GUI.skin = center;
        GUI.BeginGroup(new Rect(0, 0, Screen.width, 23));
        GUI.DrawTexture(new Rect(0, 0, Screen.width, 23), topImage, ScaleMode.StretchToFill);
        GUI.Label(new Rect(10,5,30,5), "Level: 0");
        GUI.EndGroup();
    }
    private void BottomMiddle()
    {
        GUI.skin = center;
        GUI.BeginGroup(new Rect(300, Screen.height - 200, Screen.width - 200, Screen.height));
        GUI.DrawTexture(new Rect(0, 0, Screen.width - 600, 200), midImage, ScaleMode.StretchToFill);
        GUI.Label(new Rect(20, 20, 600, 25), "WASD (or arrow keys to scroll).   Spacebar to change from 2D to 3D.");
        GUI.EndGroup();
    }
    private void BottomLeft()
    {
        GUI.skin = leftRight;
        GUI.BeginGroup(new Rect(0, Screen.height - 300, 300, 300));
        GUI.DrawTexture(new Rect(0, 0, 300, 300), bgImage, ScaleMode.StretchToFill);
        if (GUI.Button(new Rect(40, 40, buttonWidth, buttonHeight), "Small Turret"))
        {
            GameVars.turretToPlace = Resources.Load("Towers/SmallTurret") as GameObject;
        }
        if (GUI.Button(new Rect(40, 65, buttonWidth, buttonHeight), "AOE Turret"))
        {
            GameVars.turretToPlace = Resources.Load("Towers/AOETurret") as GameObject;
        }
        if (GUI.Button(new Rect(40, 90, buttonWidth, buttonHeight), "Lightning Tower"))
        {
            GameVars.turretToPlace = Resources.Load("Towers/LightningTower") as GameObject;
        }
        GUI.EndGroup();
    }
    private void BottomRight()
    {
        GUI.skin = leftRight;
        GUI.BeginGroup(new Rect(Screen.width - 300, Screen.height - 300, 300, 300));
        GUI.DrawTexture(new Rect(0, 0, 300, 300), bgImage, ScaleMode.StretchToFill);

        if (GameVars.selectedTurret != null)
        {
            Tower selectedTower = GameVars.selectedTurret.GetComponent<Tower>();
            targetMode = (TargetMode)GUI.SelectionGrid(new Rect(40, 40, 180, 50), (int)targetMode, selectionStrings, 2);
            damage = GUI.HorizontalSlider(new Rect(30, 100, 130, 30), selectedTower.Damage, 1.0f, selectedTower.maxPoints);
            speed = GUI.HorizontalSlider(new Rect(30, 130, 130, 30), selectedTower.Speed, 1.0f, selectedTower.maxPoints);
            range = GUI.HorizontalSlider(new Rect(30, 160, 130, 30), selectedTower.Range, 1.0f, selectedTower.maxPoints);
            GUI.Label(new Rect(170, 90, 200, 100), "Damage: " + (int)damage);
            GUI.Label(new Rect(170, 120, 200, 100), "Speed: " + (int)speed);
            GUI.Label(new Rect(170, 150, 200, 100), "Range: " + (int)range);

            //if anything has changed, update it
            if (damage != selectedTower.Damage) { GameVars.selectedTurret.GetComponent<Tower>().Damage = damage; }
            if (speed != selectedTower.Speed) { GameVars.selectedTurret.GetComponent<Tower>().Speed = speed; }
            if (range != selectedTower.Range) { GameVars.selectedTurret.GetComponent<Tower>().Range = range; }
            if (targetMode != selectedTower.targetingMode) { GameVars.selectedTurret.GetComponent<Tower>().targetingMode = targetMode; }
        }
        GUI.EndGroup();
    }
}