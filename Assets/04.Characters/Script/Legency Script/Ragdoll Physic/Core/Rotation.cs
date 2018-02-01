using UnityEngine;
using Sirenix.OdinInspector;


public class Rotation : MonoBehaviour
{
    public Vector2 m_lookDirection;

    [Range(0, 1)]
    public float m_rotateFactor;

    private Rigidbody _torso;

    private void Start()
    {
        _torso = GetComponentInChildren<Torso>().GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (!Mathf.Approximately(m_lookDirection.x, 0) && !Mathf.Approximately(m_lookDirection.y, 0))
        {
            
        }
    }

}
