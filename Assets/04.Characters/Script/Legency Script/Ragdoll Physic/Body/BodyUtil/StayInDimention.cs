
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StayInDimention : MonoBehaviour
{
    public enum Type
    {
        Position,
        Rotation,
        PositionAndRotation
    };

    public Type m_stayType;

    private void LateUpdate()
    {
        if(m_stayType == Type.Position || m_stayType == Type.PositionAndRotation)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, 0f);
        }

        if(m_stayType == Type.Rotation || m_stayType == Type.PositionAndRotation)
        {
            transform.rotation = Quaternion.LookRotation(new Vector3(transform.forward.x, transform.forward.y, 0f));
        }
    }


}
