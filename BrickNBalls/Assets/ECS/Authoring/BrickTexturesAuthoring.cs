using Unity.Entities;
using UnityEngine;

class BrickTexturesAuthoring : MonoBehaviour
{
    public Texture2D Texture1HP;
    public Texture2D Texture2HP;
    public Texture2D Texture3HP;
    class BrickTexturesAuthoringBaker : Baker<BrickTexturesAuthoring>
    {
        public override void Bake(BrickTexturesAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.None);

            AddComponentObject(entity, new BrickTextures
            {
                Texture1HP = authoring.Texture1HP,
                Texture2HP = authoring.Texture2HP,
                Texture3HP = authoring.Texture3HP
            });
        }
    }
}
public class BrickTextures : IComponentData
{
    public Texture2D Texture1HP;
    public Texture2D Texture2HP;
    public Texture2D Texture3HP;
}


