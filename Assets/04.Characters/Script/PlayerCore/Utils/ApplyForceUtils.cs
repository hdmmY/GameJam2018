using UnityEngine;

public static class ApplyForceUtils
{
    /// <summary>
    /// Drag two body to different direction to straighten the body
    /// </summary>
    /// <param name="direction">straighten direction</param>
    public static void Straighten(BodyInfo bodyA, BodyInfo bodyB, Vector3 direction, float force, ForceMode forceMode)
    {
        if(bodyA.BodyRigid != null && bodyB.BodyRigid != null)
        {
            bodyA.BodyRigid.AddForce(direction * force, forceMode);
            bodyB.BodyRigid.AddForce(-direction * force, forceMode);
        }
    }   

    /// <summary>
    /// Aligh body's forward direction to desire direction
    /// </summary>
    public static void AlignToVector(BodyInfo body, Vector3 direction, float force, ForceMode forceMode)
    {
        if(body.BodyRigid != null)
        {
            Vector3 curDirection = body.BodyTransform.forward;
            
            Vector3 pos1 = body.BodyTransform.position + curDirection;
            Vector3 pos2 = pos1 - 2 * curDirection;

            direction = direction.normalized;

            body.BodyRigid.AddForceAtPosition(direction * force, pos1, forceMode);
            body.BodyRigid.AddForceAtPosition(-direction * force, pos2, forceMode);
        }
    }
}