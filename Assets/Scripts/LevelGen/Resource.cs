using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
/// <summary>
/// Simple Class to handle the data needed to draw an object on the screen
/// </summary>
public class Resource
{
    public Vector3 location;
    public Quaternion angle;
    public string resource;
    public Resource(Vector3 location, Quaternion angle, string resource)
    {
        this.location = location;
        this.angle = angle;
        this.resource = resource;
    }
}
