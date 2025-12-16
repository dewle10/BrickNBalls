using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;

public struct RotationSpeed : IComponentData
{
    public float RadiansPerSecond;
}