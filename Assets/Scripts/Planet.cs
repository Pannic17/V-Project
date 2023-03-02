using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Planet
{
    // Constant
    readonly float LOCH_FLUID_CONSTANT = 2.243f;
    
    // Planet Properties
    public Vector2 screenPosition;
    public float screenDepth;
    public float planetMass;
    public float planetSize;
    public float rotationVelocity;
    
    // Planet Sub-parts
    private PlanetCore _planetCore;
    private PlanetCloud _planetCloud;
    private PlanetDust _planetDust;
    
    // Planet Movements
    public List<Planet> gravityPlanets;
    
    float CalculateDistance(Vector3 target)
    {
        return Vector3.Distance(target, new Vector3(screenPosition.x, screenPosition.y, screenDepth));
    }

    public float CalculateEquilibrium(float moonMass, float distance)
    {
        double lochLimit = LOCH_FLUID_CONSTANT * planetSize * (planetMass / moonMass);
        float limit = (float)lochLimit;
        if (limit <= distance)
        {
            return distance * (moonMass / (planetMass + moonMass));
        }
        return 0f;
    }

    public void AssignDustGravity()
    {
        List<Vector3> points = new List<Vector3>();
        List<float> distances = new List<float>();
        for (int i = 0; i < 3; i++)
        {
            Vector3 position = new Vector3(
                gravityPlanets[i].screenPosition.x, 
                gravityPlanets[i].screenPosition.y,
                gravityPlanets[i].screenDepth);
            points.Insert(i, position);
            distances.Insert(i, CalculateEquilibrium(gravityPlanets[i].planetMass, CalculateDistance(position)));
        }
        _planetDust.UpdateParticle(points, distances);
    }
}
