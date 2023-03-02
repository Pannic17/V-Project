using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.VFX;

public class PlanetDust
{
    public List<PlanetDustProperties> gravityPoints;
    public VisualEffect vfxGraph;

    public PlanetDust(List<PlanetDustProperties> gravityPoints, VisualEffect vfxGraph)
    {
        this.gravityPoints = gravityPoints;
        this.vfxGraph = vfxGraph;
    }

    // TODO: Update the gravity center point for each planet*
    public void UpdateParticle(List<Vector3> points, List<float> distances)
    {
        
    }
}
