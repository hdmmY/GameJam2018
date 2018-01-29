using UnityEngine;

public class EntitesUtil : MonoBehaviour
{
    public static bool ContainComponents(uint[] entitiesComponents, params uint[] requestType)
    {
        if (entitiesComponents == null)
            return false;

        if (requestType == null)
            return true;

        int length1 = entitiesComponents.Length;
        int length2 = requestType.Length;
        int containNum = 0;

        if (length1 < length2)
            return false;

        for (int i = 0, j = 0; i < length1 && j < length2;)
        {
            if (entitiesComponents[i] < requestType[j])
            {
                i++;
            }
            else if (entitiesComponents[i] == requestType[j])
            {
                i++; j++; containNum++;
            }
            else
            {
                j++;
            }
        }

        return containNum == length2;
    }
}
