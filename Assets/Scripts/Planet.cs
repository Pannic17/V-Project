using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Planet : MonoBehaviour
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
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    float CalculateDistance(Vector3 target)
    {
        return Vector3.Distance(target, new Vector3(screenPosition.x, screenPosition.y, screenDepth));
    }

    float CalculateEquilibrium(Planet planet, float distance)
    {
        double lochLimit = LOCH_FLUID_CONSTANT * planetSize * (planetMass / planet.planetMass);
        float limit = (float)lochLimit;
        if (limit <= distance)
        {
            return distance * (planet.planetMass / (planetMass + planet.planetMass));
        }
        return 0f;
    }

    void AssignDustGravity()
    {
        
    }
}
