using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System.Linq;

public class ComponentAllocater : MonoBehaviour
{
    #region Public Variables

    public static ComponentAllocater Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GameObject("ComponentAllocater").AddComponent<ComponentAllocater>();
            }
            return _instance;
        }
    }

    #endregion

    #region Private Variables

    private static ComponentAllocater _instance;
    private static List<EntitiesAdmin> _entities = new List<EntitiesAdmin>();

    #endregion



    #region MonoBehavior

    private void Awake()
    {
        _instance = this;
    }

    #endregion


    #region Public Func

    public static void AddEntity(EntitiesAdmin entity)
    {
        if (!_entities.Contains(entity))
        {
            _entities.Add(entity);
        }
    }

    public static void RemoveEntity(EntitiesAdmin entity)
    {
        if (_entities.Contains(entity))
        {
            _entities.Remove(entity);
        }
    }

    public static EntitiesAdmin[] FilterEntities(uint[] components)
    {
        return _entities.Where(
            x => EntitesUtil.ContainComponents(x.m_components, components) == true).
            ToArray();
    }           


    #endregion


}


