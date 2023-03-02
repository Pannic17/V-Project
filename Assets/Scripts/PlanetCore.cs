using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vector3 = System.Numerics.Vector3;

public class PlanetCore
{
    public Shader shaderGraph;


    public PlanetCore(Shader shaderGraph)
    {
        this.shaderGraph = shaderGraph;
    }

    // TODO: Update the gravity center point for each planet*
    public void UpdateCoreVertex(Vector3 position, float amplitude)
    {
        
    }
}
