using UnityEngine;

public class ComponentUtil : MonoBehaviour
{
    public static uint NameToComponentType(string name)
    {
        switch (name)
        {
            case "RenderBinding":
                return ComponentType.RenderBinding;
            default:
                return 0;
        }
    }

    public static string ComponentTypeToName(uint componentType)
    {
        if (componentType == ComponentType.RenderBinding)
        {
            return "RenderBinding";
        }

        return null;
    }

    public static System.Type ComponentTypeToType(uint componentType)
    {
        if (componentType == ComponentType.RenderBinding)
        {
            return typeof(RenderBinding);
        }

        return null;
    }
}
