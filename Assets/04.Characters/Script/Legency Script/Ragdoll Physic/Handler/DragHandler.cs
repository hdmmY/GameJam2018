using UnityEngine;
using Sirenix.OdinInspector;


public class DragHandler : MonoBehaviour
{
    [ReadOnly]
    public float m_startDrag;

    [ReadOnly]
    public float m_startAngularDrag;

    [ReadOnly]
    public float m_extraDrag;

    private Rigidbody _rb;

    private bool _isFallen;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if(_isFallen)
        {
            _rb.drag = Mathf.Lerp(_rb.drag, 0f, Time.fixedDeltaTime * 5f);
            _rb.angularDrag = Mathf.Lerp(_rb.angularDrag, 0f, Time.fixedDeltaTime * 5f);
        }
        else
        {
            _rb.drag = Mathf.Lerp(_rb.drag, m_startDrag + m_extraDrag, Time.fixedDeltaTime * 5f);
            _rb.angularDrag = Mathf.Lerp(_rb.angularDrag, m_startAngularDrag + m_extraDrag, Time.fixedDeltaTime * 5f);
        }

        if(m_extraDrag > 0f)
        {
            m_extraDrag -= Time.fixedDeltaTime;
        }
        else
        {
            m_extraDrag = 0f;
        }
    }

    public void SetStandingDrag()
    {
        _isFallen = false;
    }                     

    public void SetFallenDrag()
    {
        _isFallen = true;
    }
}
