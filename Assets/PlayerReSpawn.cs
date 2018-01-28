using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerReSpawn : MonoBehaviour
{
    public float m_deathY;

    private PlayerInputController _inputController;

    private Rigidbody _torso;

    private void Start()
    {
        _inputController = GetComponent<PlayerInputController>();
        _torso = GetComponentInChildren<Torso>().GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (_torso.position.y < m_deathY)
        {
            Vector3 target = transform.root.GetComponent<Player>().Team.Spawner.SpawnPosition();

            Vector3 offset = target - _torso.position;

            foreach (var rig in GetComponentsInChildren<Rigidbody>())
            {
                rig.position += offset;
                rig.velocity = Vector3.zero;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(Vector3.up * m_deathY, new Vector3(100, 0.5f, 100));
    }

}
