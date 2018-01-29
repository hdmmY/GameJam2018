using UnityEngine;
using Sirenix.OdinInspector;

public class BindRenderUtil : MonoBehaviour
{
    [Button("Bind Renders Rotation", ButtonSizes.Medium)]
    public void ResetBindRendersRotationInEditor()
    {
        foreach (var bindRenderComp in FindObjectsOfType<RenderBinding>())
        {
            bindRenderComp.RotationOffset = bindRenderComp.FollowerTransform.rotation;
        }
    }

    public static void ResetBindRenderRotation()
    {
        foreach (var bindRenderComp in FindObjectsOfType<RenderBinding>())
        {
            bindRenderComp.RotationOffset = bindRenderComp.FollowerTransform.rotation;
        }
    }
}
