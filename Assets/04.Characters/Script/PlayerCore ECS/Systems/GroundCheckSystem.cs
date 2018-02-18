// using Sirenix.OdinInspector;
// using System.Collections.Generic;
// using UnityEngine;

// public class GroundCheckSystem : MonoBehaviour
// {
//     public CharacterProperty m_character;

//     [ListDrawerSettings (AlwaysAddDefaultValue = true)]
//     public List<EnvironmentCollisionChecker> m_collisionCheckers;

//     private void Update ()
//     {
//         float minAngle = 1024; // means nothing

//         foreach (var checker in m_collisionCheckers)
//         {
//             if (checker.HasCollision)
//             {
//                 if (minAngle > checker.CollisionAngle)
//                 {
//                     minAngle = Vector3.Angle (Vector3.up, checker.CollisionNormal);
//                 }
//             }
//         }

//         if (minAngle < 70)
//         {
//             if (!m_character.HasState (CharacterProperty.State.BeingGrabbed))
//             {
//                 m_character.m_state &= ~CharacterProperty.State.InAir;
//                 m_character.m_state &= ~CharacterProperty.State.Wall;
//                 m_character.m_state |= CharacterProperty.State.Ground;
//             }
//         }
//         else if (minAngle == 1024)
//         {
//             m_character.m_state &= ~CharacterProperty.State.Wall;
//             m_character.m_state &= ~CharacterProperty.State.Ground;
//             m_character.m_state |= CharacterProperty.State.InAir;
//         }
//         else
//         {
//             m_character.m_state &= ~CharacterProperty.State.Ground;
//             m_character.m_state &= ~CharacterProperty.State.InAir;
//             m_character.m_state |= CharacterProperty.State.Wall;
//         }
//     }
// }