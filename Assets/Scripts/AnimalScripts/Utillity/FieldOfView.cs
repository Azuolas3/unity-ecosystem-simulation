using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView 
{
    int plantLayerMask = (1 << 6);
    int animalLayerMask = (1 << 7);

    public Collider[] GetNearbyColliders(Vector3 center, float radius)
    {
        int layerMask = plantLayerMask | animalLayerMask;

        Collider[] hitColliders = Physics.OverlapSphere(center, radius, layerMask);
        return hitColliders;
    }
}
