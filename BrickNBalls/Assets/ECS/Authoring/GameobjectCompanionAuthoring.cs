using Unity.Entities;
using UnityEngine;

class GameobjectCompanionAuthoring : MonoBehaviour
{
    public GameObject CompanionPrefab;
    class GameobjectCompanionAuthoringBaker : Baker<GameobjectCompanionAuthoring>
    {
        public override void Bake(GameobjectCompanionAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new CompanionPrefab { Prefab = authoring.CompanionPrefab });
        }
    }
}
public struct CompanionPrefab : IComponentData
{
    public UnityObjectRef<GameObject> Prefab;
}
public struct CompanionInstance : IComponentData
{
    public UnityObjectRef<GameObject> Instance;
    public UnityObjectRef<Renderer> Renderer;
}
public struct CompanionStaticTag : IComponentData
{
}
