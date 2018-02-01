using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothFollow : MonoBehaviour
{
    public Transform m_target;

    public float m_smoothSpeed;

    public float m_leftBorder;

    public float m_rightBorder;

    public float m_upperBorder;

    public float m_underBorder;

    public bool m_useFixedUpdate;

    private Vector3 _offset;
    private Vector3 _curVelocity;
    private Vector3 _nextPosition;

    private void Start()
    {
        _curVelocity = Vector3.zero;
        _nextPosition = transform.position;
    }


    private void Update()
    {
        if (!m_useFixedUpdate)
        {
            UpdatePosition();
        }
    }

    private void FixedUpdate()
    {
        if (m_useFixedUpdate)
        {
            UpdatePosition();
        }
    }

    private void UpdatePosition()
    {
        Vector3 targetPos = m_target.position;

        if (targetPos.x < transform.position.x + m_leftBorder)
        {
            _nextPosition.x += targetPos.x - (transform.position.x + m_leftBorder);
        }
        else if (targetPos.x > transform.position.x + m_rightBorder)
        {
            _nextPosition.x += targetPos.x - (transform.position.x + m_rightBorder);
        }

        if (targetPos.y < transform.position.y + m_underBorder)
        {
            _nextPosition.y += targetPos.y - (transform.position.y + m_underBorder);
        }
        else if (targetPos.y > transform.position.y + m_upperBorder)
        {
            _nextPosition.y += targetPos.y - (transform.position.y + m_upperBorder);
        }

        float deltTime = m_useFixedUpdate ? Time.fixedDeltaTime : Time.deltaTime;
        transform.position = Vector3.SmoothDamp(transform.position, _nextPosition, ref _curVelocity, m_smoothSpeed * deltTime);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;

        Vector3 centre = transform.position;
        Vector3 left = new Vector3(centre.x + m_leftBorder, centre.y, 0);
        Vector3 right = new Vector3(centre.x + m_rightBorder, centre.y, 0);
        Vector3 upper = new Vector3(centre.x, centre.y + m_upperBorder, 0);
        Vector3 under = new Vector3(centre.x, centre.y + m_underBorder, 0);

        Gizmos.DrawLine(left - Vector3.up * 3, left + Vector3.up * 3);
        Gizmos.DrawLine(right - Vector3.up * 3, right + Vector3.up * 3);
        Gizmos.DrawLine(upper - Vector3.right * 3, upper + Vector3.right * 3);
        Gizmos.DrawLine(under - Vector3.right * 3, under + Vector3.right * 3);
    }
}
