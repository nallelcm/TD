using System.Collections.Generic;
/// <summary>
/// Generic Node object
/// </summary>
public class Node
{
    public Node parent;
    public Stack<Node> children;
    public Cell data;
    public Node(Node prev, Cell data)
    {
        this.parent = prev;
        this.data = data;
        children = new Stack<Node>(4);
    }
    /// <summary>
    /// Makes sure not to add its parent as a child
    /// </summary>
    /// <param name="child">child to be added to children</param>
    public void AddChild(Cell child)
    {
        if (parent == null || parent.data != child)
            children.Push(new Node(this, child));
    }
}