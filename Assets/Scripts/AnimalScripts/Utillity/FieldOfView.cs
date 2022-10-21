using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView 
{
    private readonly int _plantLayerMask = (1 << 6);
    private readonly int _predatorLayerMask = (1 << 7);
    private readonly int _preyLayerMask = (1 << 8);
    private readonly int _waterLayerMask = (1 << 4);
    private readonly int _terrainLayerMask = (1 << 3);


    public int TerrainLayerMask { get { return _terrainLayerMask; } }
    public int WaterLayerMask { get { return _waterLayerMask; } }
    public int PlantLayerMask { get { return _plantLayerMask; } }
    public int PredatorLayerMask { get { return _predatorLayerMask; } }
    public int PreyLayerMask { get { return _preyLayerMask; } }

    public Collider[] GetNearbyColliders(Vector3 center, float radius, int layerMask)
    {
        Collider[] hitColliders = Physics.OverlapSphere(center, radius, layerMask);
        return hitColliders;
    }
}
