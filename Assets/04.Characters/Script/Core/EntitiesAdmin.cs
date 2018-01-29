using UnityEngine;
using Sirenix.OdinInspector;

public class EntitiesAdmin : MonoBehaviour
{
    [ReadOnly, ListDrawerSettings(NumberOfItemsPerPage = 5)]
    public uint[] m_components = new uint[256];


    #region Monobehavior

    private void OnEnable()
    {
        ComponentAllocater.AddEntity(this);       
    }

    private void OnDisable()
    {
        ComponentAllocater.RemoveEntity(this);    
    }

    #endregion
}
