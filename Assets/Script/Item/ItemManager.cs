using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public List<GameObject> m_itemPrefabs;

    public int m_minCout;

    public int m_maxCount;

    public Spawner m_spawner;

    public float m_coolDown = 5f;

    private float _timer = 0f;

    private void Update()
    {
        int curCount = GameObject.FindObjectsOfType<ItemProperty>().Length;

        if (_timer > m_coolDown)
        {
            if (curCount < m_minCout)
            {
                int count = Random.Range(m_minCout, m_maxCount);

                for (int i = curCount; i < count; i++)
                {
                    var item = Instantiate(m_itemPrefabs[Random.Range(0, m_itemPrefabs.Count - 1)]);
                    item.transform.position = m_spawner.SpawnPosition();
                }

                _timer = 0f;
            }
        }

        _timer += Time.deltaTime;
    }
}
