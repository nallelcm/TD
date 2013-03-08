using UnityEngine;
using System.Collections.Generic;
using System.Linq;
public enum Directions { North = 0, South, West, East, NorthWest, NorthEast, SouthWest, SouthEast };
/// <summary>
/// Calls on the Wilson class to make a maze 
/// then traverses the maze and makes a tree data structure from it / Also adds each cell and its offset cells to the resources list
/// then picks the longest tree branch and retraces the path to create the enemy waypoints
/// </summary>
public class GenerateLevel{
    private System.Random rand;
    private Dictionary<Directions, Vector3> offsets;
    private Dictionary<Directions, Vector3[]> faceVectors;
    private Vector3[] topFaceVectors;
    private Material atlasMap;
    public GenerateLevel(int width, int height)
    {
        offsets = new Dictionary<Directions, Vector3>() 
        {
            { Directions.NorthWest, new Vector3(-1, 1, 0) },
            { Directions.NorthEast, new Vector3(1, 1, 0) },
            { Directions.SouthWest, new Vector3(-1, -1, 0) },
            { Directions.SouthEast, new Vector3(1, -1, 0) },
            { Directions.North, new Vector3(0, 1, 0) },
            { Directions.South, new Vector3(0, -1, 0) },
            { Directions.West, new Vector3(-1, 0, 0) },
            { Directions.East, new Vector3(1, 0, 0) }
        };
        topFaceVectors = new Vector3[] { new Vector3(-0.5f, -0.5f, -0.5f), new Vector3(0.5f, -0.5f, -0.5f), new Vector3(-0.5f, 0.5f, -0.5f), new Vector3(0.5f, 0.5f, -0.5f) };
        faceVectors = new Dictionary<Directions, Vector3[]>()
        {
            { Directions.North, new Vector3[]{new Vector3(0.5f,0.5f,0.5f),new Vector3(-0.5f,0.5f,0.5f),new Vector3(0.5f,0.5f,-0.5f),new Vector3(-0.5f,0.5f,-0.5f)} },
            { Directions.South, new Vector3[]{new Vector3(-0.5f,-0.5f,0.5f),new Vector3(0.5f,-0.5f,0.5f),new Vector3(-0.5f,-0.5f,-0.5f),new Vector3(0.5f,-0.5f,-0.5f)} },
            { Directions.West, new Vector3[]{new Vector3(-0.5f,-0.5f,0.5f),new Vector3(-0.5f,-0.5f,-0.5f),new Vector3(-0.5f,0.5f,0.5f),new Vector3(-0.5f,0.5f,-0.5f)} },
            { Directions.East, new Vector3[]{new Vector3(0.5f,-0.5f,-0.5f),new Vector3(0.5f,-0.5f,0.5f),new Vector3(0.5f,0.5f,-0.5f),new Vector3(0.5f,0.5f,0.5f)} }
        };
        atlasMap = (Material)Resources.Load("Environment/Terrain", typeof(Material));
        Wilson maze = new Wilson(width, height);
        CreateTreeFromMaze(maze.start);
        GameObject plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
        plane.transform.localScale = new Vector3(height * .3f, 1, width * .3f);
        plane.transform.position = new Vector3(height * 1.5f-1.5f, width * -1.5f+1.5f, 0.5f);
        plane.transform.rotation = Quaternion.Euler(270, 0, 0);
        plane.renderer.material = (Material)Resources.Load("Environment/Floor", typeof(Material));
    }
    /// <summary>
    /// Creates a path of waypoints for enemies to follow (inserts into GameVars.waypoints)
    /// </summary>
    /// <param name="node">Starting Node to begin traveling up the tree from</param>
    private void CreateEnemyWaypoints(Node node)
    {
        while (node.parent != null)
        {
            GameVars.waypoints.Insert(0, node.data.coordinates);
            node = node.parent;
        }
        GameVars.waypoints.Insert(0, node.data.coordinates);
    }
    /// <summary>
    /// Builds a Tree from the given start Cell (Cells points to other cells)
    /// Also calls CreateEnemyWaypoints with the last Node it visits to ensure the deepest path
    /// </summary>
    /// <param name="maze">The starting Cell</param>
    private void CreateTreeFromMaze(Cell start)
    {
        Queue<Node> fringe = new Queue<Node>();
        Node currentNode = new Node(null, start);
        fringe.Enqueue(currentNode);
        do
        {
            currentNode = fringe.Dequeue();
            CreateResourcesFromCell(currentNode.data);
            for (int i = 0; i < 4; i++) { if (currentNode.data.adjacentCells[i] != null && !currentNode.data.walls[i]) { currentNode.AddChild(currentNode.data.adjacentCells[i]); } }
            foreach (Node child in currentNode.children) { fringe.Enqueue(child); }
        } while (fringe.Count > 0);
        CreateEnemyWaypoints(currentNode);
    }
    /// <summary>
    /// Adds the given Cell and all its neighbours (if the wall is up) to the resources List to be drawn later.
    /// Neighbours consist of north, west, south, east, northwest, northeast, southwest, southeast
    /// </summary>
    /// <param name="cell">Cell to be added</param>
    private void CreateResourcesFromCell(Cell cell)
    {
        if(cell.wall(Directions.North)){ CreateGameobjectFromWall(new List<Directions> { Directions.South }, cell.coordinates + offsets[Directions.North], cell.angle, Directions.North.ToString()); };
        if (cell.wall(Directions.East)) { CreateGameobjectFromWall(new List<Directions> { Directions.West }, cell.coordinates + offsets[Directions.East], cell.angle, Directions.East.ToString()); };
        if (cell.wall(Directions.West)) { CreateGameobjectFromWall(new List<Directions> { Directions.East }, cell.coordinates + offsets[Directions.West], cell.angle, Directions.West.ToString()); };
        if (cell.wall(Directions.South)) { CreateGameobjectFromWall(new List<Directions> { Directions.North }, cell.coordinates + offsets[Directions.South], cell.angle, Directions.South.ToString()); };
        List<Directions> faces = new List<Directions>(2) {  Directions.East, Directions.South };
        if (cell.wall(Directions.West)) { faces.Remove(Directions.South); }
        if (cell.wall(Directions.North)) { faces.Remove(Directions.East); }
        CreateGameobjectFromWall(faces, cell.coordinates + offsets[Directions.NorthWest], cell.angle, Directions.NorthWest.ToString());
        faces = new List<Directions>(2) { Directions.West, Directions.South };
        if (cell.wall(Directions.North)) { faces.Remove(Directions.West); }
        if (cell.wall(Directions.East)) { faces.Remove(Directions.South); }
        CreateGameobjectFromWall(faces, cell.coordinates + offsets[Directions.NorthEast], cell.angle, Directions.NorthEast.ToString());
        faces = new List<Directions>(2) { Directions.East, Directions.North };
        if (cell.wall(Directions.West)) { faces.Remove(Directions.North); }
        if (cell.wall(Directions.South)) { faces.Remove(Directions.East); }
        CreateGameobjectFromWall(faces, cell.coordinates + offsets[Directions.SouthWest], cell.angle, Directions.SouthWest.ToString());
        faces = new List<Directions>(2) { Directions.West, Directions.North };
        if (cell.wall(Directions.East)) { faces.Remove(Directions.North); }
        if (cell.wall(Directions.South)) { faces.Remove(Directions.West); }
        CreateGameobjectFromWall(faces, cell.coordinates + offsets[Directions.SouthEast], cell.angle, Directions.SouthEast.ToString());
    }
    private void CreateFaceMesh(Vector3[] faceVertices, ref int vIndex, ref int tIndex, ref int uvIndex, ref Vector3[] vertices, ref Vector2[] uvs, ref int[] triangles)
    {
        CreateFaceMesh(faceVertices, ref vIndex, ref tIndex, ref uvIndex,ref vertices,ref  uvs, ref triangles, false);
    }
    private void CreateFaceMesh(Vector3[] faceVertices, ref int vIndex, ref int tIndex, ref int uvIndex, ref Vector3[] vertices, ref Vector2[] uvs, ref int[] triangles, bool top)
    {
        #region face
        #region verts
        vertices[vIndex++] = faceVertices[0];//new Vector3(-0.5f, -0.5f, 0);
        vertices[vIndex++] = faceVertices[1];//new Vector3(0.5f, -0.5f, 0);
        vertices[vIndex++] = faceVertices[2];//new Vector3(-0.5f, 0.5f, 0);
        vertices[vIndex++] = faceVertices[3];//new Vector3(0.5f, 0.5f, 0);
        #endregion
        #region uvs
        if (top)
        {
            uvs[uvIndex++] = new Vector2(1, 6);
            uvs[uvIndex++] = new Vector2(2, 6);
            uvs[uvIndex++] = new Vector2(1, 7);
            uvs[uvIndex++] = new Vector2(2, 7);
        }
        else
        {
            uvs[uvIndex++] = new Vector2(3, 15);
            uvs[uvIndex++] = new Vector2(4, 15);
            uvs[uvIndex++] = new Vector2(3, 16);
            
        }
        #endregion
        int v = vIndex - 4;
        #region tris
        triangles[tIndex++] = v + 3;
        triangles[tIndex++] = v + 1;
        triangles[tIndex++] = v;

        triangles[tIndex++] = v;
        triangles[tIndex++] = v + 2;
        triangles[tIndex++] = v + 3;
        #endregion
        #endregion
    }
    private void CreateGameobjectFromWall(List<Directions> faces, Vector3 location, Quaternion angle, string name)
    {
        GameObject wall = new GameObject("wall");

        Vector3[] vertices = new Vector3[(faces.Count+1)*4];
        Vector2[] uvs = new Vector2[(faces.Count+1)*4];
        int[] triangles = new int[(faces.Count + 1) * 6];
        Mesh mesh = new Mesh();
        int vIndex = 0;
        int tIndex = 0;
        int uvIndex = 0;
        CreateFaceMesh(topFaceVectors, ref vIndex, ref tIndex, ref uvIndex, ref vertices, ref uvs, ref triangles , true);
        foreach (Directions direction in faces){ CreateFaceMesh(faceVectors[direction], ref vIndex, ref tIndex, ref uvIndex, ref vertices, ref uvs, ref triangles);}

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        wall.AddComponent<MeshFilter>().mesh = mesh;
        wall.AddComponent<MeshCollider>().sharedMesh = mesh;
        wall.AddComponent<MeshRenderer>();
        wall.AddComponent<WallSpace>();
        wall.transform.position = location;
        wall.transform.rotation = angle;
        wall.renderer.material = atlasMap;
        wall.name = name;
    }
}