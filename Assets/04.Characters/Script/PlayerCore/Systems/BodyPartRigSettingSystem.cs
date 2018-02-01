using UnityEngine;

public class BodyPartRigSettingSystem : MonoBehaviour
{
    public ApplyRigDrag m_applyRigDrag;

    private void Update()
    {
        m_applyRigDrag.m_enabled = true;
    }
}
