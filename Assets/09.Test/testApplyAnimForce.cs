using UnityEngine;
using System.Collections;

public class testApplyAnimForce : MonoBehaviour
{
    public Rigidbody rig;

    public ForceMode forceMode;

    public bool apply = false;

    public Vector3 startForceDir;

    public Vector3 endForceDir;

    public float time;

    public bool isLocal;

    public bool useTorque = false;

    [Space]

    [SerializeField]
    private float coolTime = 0f;

    [SerializeField]
    private bool isApplying = false;

    [SerializeField]
    private Vector3 curForce;

    private void FixedUpdate()
    {
        if (apply && !isApplying)
        {
            apply = false;

            StartCoroutine(ApplyForce());
        }
    }

    IEnumerator ApplyForce()
    {
        curForce = startForceDir;
        float t = 0f;

        coolTime = 0f;
        isApplying = true;

        while (coolTime < time)
        {
            coolTime += Time.fixedDeltaTime;

            t = coolTime / time;
            curForce.x = Mathf.Lerp(startForceDir.x, endForceDir.x, t);
            curForce.y = Mathf.Lerp(startForceDir.y, endForceDir.y, t);
            curForce.z = Mathf.Lerp(startForceDir.z, endForceDir.z, t);

            if (rig)
            {
                if(useTorque)
                {
                    if (isLocal)
                    {
                        rig.AddRelativeTorque(curForce, forceMode);
                    }
                    else
                    {
                        rig.AddTorque(curForce, forceMode);
                    }
                }
                else
                {
                    if (isLocal)
                    {
                        rig.AddRelativeForce(curForce, forceMode);
                    }
                    else
                    {
                        rig.AddForce(curForce, forceMode);
                    }
                }                      
            }

            yield return new WaitForFixedUpdate();
        }

        isApplying = false;
        curForce.x = curForce.y = curForce.z = 0f;
    }

}
