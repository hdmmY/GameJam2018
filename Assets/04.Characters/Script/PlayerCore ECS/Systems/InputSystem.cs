using System.Collections.Generic;
using InControl;
using UnityEngine;

namespace PlayerCoreECS
{
    /*
     
        Just a simple input system, it will be restruct soon
         
    */
    public class InputSystem : MonoBehaviour
    {
        public List<InputProperty> m_entities;

        private void Awake ()
        {
            foreach (var entity in m_entities)
            {
                entity.BindActions (CharacterActions.CreateWithControllerBindings (), InputManager.ActiveDevice);
            }
        }
    }
}