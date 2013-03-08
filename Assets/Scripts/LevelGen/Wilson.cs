using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;
/// <summary>
/// Wilson's maze algorithm implemented as a class
/// </summary>
public class Wilson
{
    protected int height;
    protected int width;
    public List<Cell> bucket;
    public Cell start;
    public Wilson(int height, int width)
    {
        this.height = height;
        this.width = width;
        bucket = new List<Cell>(height * width);
        LinkCells();
        Run();
    }
    /// <summary>
    /// Connects adjacent(north,east,south,west) cells to eachother(safe pointers) and edges are null
    /// </summary>
    public void LinkCells()
    {
        Cell current = new Cell(0, 0, ref bucket);
        start = current;
        for (int y = 0; y < height - 1; y++)
        {
            Cell firstInRow = current;
            for (int x = 0; x < width - 1; x++)
            {
                Cell south, east, southeast;

                if (y == 0) { east = new Cell(x + 1, y, ref bucket); }
                else { east = current.adjacentCell(Directions.East); }
                if (x == 0) { south = new Cell(x, y + 1, ref bucket); }
                else { south = current.adjacentCell(Directions.South); }

                southeast = new Cell(x + 1, y + 1, ref bucket);

                current.ConnectCell(Directions.South, south);
                current.ConnectCell(Directions.East, east);

                east.ConnectCell(Directions.West, current);
                east.ConnectCell(Directions.South, southeast);

                south.ConnectCell(Directions.East, southeast);
                south.ConnectCell(Directions.North, current);

                southeast.ConnectCell(Directions.North, east);
                southeast.ConnectCell(Directions.West, south);

                current = east;
            }
            current = firstInRow.adjacentCell(Directions.South);
        }
    }
    private Cell RandomCell(System.Random rand) { return bucket[rand.Next(0, bucket.Count)]; }
    /// <summary>
    /// Adds a random cell to the maze(using the cell.flag bool)
    /// picks a random cell to start from and randomly walks along the maze untill it finds a cell that is added to the maze
    /// breaks the path between those two cells (settings all the cells walls to false in those directions)
    /// repeats untill all cells have been added to the maze
    /// </summary>
    public void Run()
    {
        System.Random rand = new System.Random();

        Cell current = RandomCell(rand);
        bucket.Remove(current);
        current.flag = true;
        current = RandomCell(rand);
        Cell next;
        Dictionary<Cell, Cell> path = new Dictionary<Cell, Cell>();

        while (true)
        {
            next = current.RandomConnectedCell(rand);
            path[current] = next;
            if (next.flag)
            {
                foreach (KeyValuePair<Cell, Cell> connection in path)
                {
                    connection.Value.BreakWall(connection.Key);
                    connection.Key.BreakWall(connection.Value);
                    connection.Key.flag = true;
                    bucket.Remove(connection.Key);
                }
                if (bucket.Count < 1) { break; }
                path.Clear();
                current = RandomCell(rand);
            }
            else { current = next; }
        }
    }
}