using UnityEngine;

public class BindRenderSystem : MonoBehaviour
{
    public readonly static uint[] FilterComponents = new uint[]
    {
        ComponentType.RenderBinding
    };


    private void FixedUpdate()
    {
        foreach (var entity in ComponentAllocater.FilterEntities(FilterComponents))
        {
            var renderBinding = entity.GetComponent<RenderBinding>();

            if (renderBinding.BindPosition)
            {
                renderBinding.FollowerTransform.position =
                    renderBinding.OriginTransform.position + renderBinding.PositionOffset;
            }

            if (renderBinding.BindRotation)
            {
                renderBinding.FollowerTransform.rotation =
                    renderBinding.OriginTransform.rotation * renderBinding.RotationOffset;
            }
        }
    }
}
