using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[ExecuteInEditMode]
public class SetLineRenderers : MonoBehaviour
{
    [ListDrawerSettings(AlwaysAddDefaultValue = true)]
    public List<Transform> m_positions;

    private LineRenderer _lineRenderer;

    private void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.positionCount = m_positions.Count;

        Update();
    }

    private void Update()
    {
        if (m_positions != null && m_positions.Count > 0)
        {
            _lineRenderer.positionCount = m_positions.Count;
            for (int i = 0; i < m_positions.Count; i++)
            {
                Vector3 pos = m_positions[i].position;
                pos.z = 0f;
                _lineRenderer.SetPosition(i, pos);
            }
            _lineRenderer.enabled = true;
        }
        else
        {
            _lineRenderer.positionCount = 0;
        }
    }    
}
