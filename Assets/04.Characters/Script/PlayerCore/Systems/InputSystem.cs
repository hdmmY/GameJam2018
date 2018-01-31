using UnityEngine;
using InControl;


/*
 
    Just a simple input system, it will be restruct soon
     
*/
public class InputSystem : MonoBehaviour
{
    public InputProperty m_inputProperty;

    private void Awake()
    {
        m_inputProperty.BindActions(CharacterActions.CreateWithKeyboardBindings(), InputManager.ActiveDevice);
    }
}
