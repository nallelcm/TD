using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
/// <summary>
/// Each cell knows what cell it is adjacent to and whether or not there is a wall between them
/// aswell as information about drawing in 3d space: angle and locations
/// </summary>
public class Cell
{
    public List<Cell> adjacentCells;
    public bool[] walls;
    public Vector3 coordinates;
    public Quaternion angle;
    public bool flag;
    /// <summary>
    /// Create a new Cell
    /// </summary>
    /// <param name="x">x location in a grid, left and topmost being 0</param>
    /// <param name="y">y location in a grid, left and topmost being 0</param>
    /// <param name="bucket">a reference to a List of Cells to add itself to (used for the Wilson algorithm)</param>
    public Cell(int x, int y, ref List<Cell> bucket)
    {
        bucket.Add(this);
        coordinates = new Vector3(3 * x, -3 * y, GameVars.Z_COORD+1);
        angle = Quaternion.Euler(0, 0, 0);
        adjacentCells = new List<Cell>(4) { null, null, null, null };
        walls = new bool[4] { true, true, true, true };
        flag = false;
    }
    /// <summary>
    /// picks a random adjacent cell and returns it
    /// </summary>
    /// <param name="rand">takes in a Random object for proper random generation</param>
    /// <returns>A randomly adjacent Cell</returns>
    public Cell RandomConnectedCell(System.Random rand)
    {
        Cell choice;
        do { choice = adjacentCells[rand.Next(0, 4)]; } while (choice == null);
        return choice;
    }
    /// <summary>
    /// Set the wall between these two adjacent cells to false
    /// </summary>
    /// <param name="adjacentCell">an adjacent cell (it will not work if you send it a cell that is not adjacent to it)</param>
    public void BreakWall(Cell adjacentCell) { walls[adjacentCells.IndexOf(adjacentCell)] = false; }
    /// <summary>
    /// Gets the value of the wall at a certain direction
    /// </summary>
    /// <param name="direction">Which direction you want to check</param>
    /// <returns>True or False if the wall exists or not</returns>
    public bool wall(Directions direction) { return walls[(int)direction]; }
    /// <summary>
    /// Gets the Cell at a certain direction
    /// </summary>
    /// <param name="direction">Which direction you want to check</param>
    /// <returns>The Cell adjacent at the given direction</returns>
    public Cell adjacentCell(Directions direction) { return adjacentCells[(int)direction]; }
    /// <summary>
    /// Sets the given Direction to the given Cell
    /// </summary>
    /// <param name="direction">which Direction you want to connect</param>
    /// <param name="cell">What cell you would like to connect</param>
    public void ConnectCell(Directions direction, Cell cell) { adjacentCells[(int)direction] = cell; }
    /// <summary>
    /// Since this object is used as a key GetHashCode must return a unique integer
    /// </summary>
    /// <returns>A unique integer per Cell</returns>
    public override int GetHashCode() 
    { 
        return (int)((this.coordinates.y * this.coordinates.x) + this.coordinates.x); 
    }
    /// <summary>
    /// Since GetHashCode is overridden so must Equals, the hashcode and equals must be the same if it is the same Cell
    /// </summary>
    /// <param name="obj">Object to be compared to</param>
    /// <returns>True of False if the objects are equal(the same Vector 3 coordinates)</returns>
    public override bool Equals(object obj)
    {
        Cell fooItem = obj as Cell;
        return fooItem.coordinates == this.coordinates;
    }
}