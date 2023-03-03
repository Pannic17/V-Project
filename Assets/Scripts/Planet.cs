using System.Collections.Generic;
using UnityEngine;

public class Planet
{
    // Constant
    private const float LOCH_FLUID_CONSTANT = 2.243f;
    
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

    public Planet(Vector2 screenPosition, float screenDepth, float planetMass, float planetSize, float rotationVelocity, PlanetCore planetCore, PlanetCloud planetCloud, PlanetDust planetDust, List<Planet> gravityPlanets)
    {
        this.screenPosition = screenPosition;
        this.screenDepth = screenDepth;
        this.planetMass = planetMass;
        this.planetSize = planetSize;
        this.rotationVelocity = rotationVelocity;
        _planetCore = planetCore;
        _planetCloud = planetCloud;
        _planetDust = planetDust;
        this.gravityPlanets = gravityPlanets;
    }

    public void SetPosition(int x, int y, int z)
    {
        screenPosition.x = x;
        screenPosition.y = y;
        screenDepth = z;
    }

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
