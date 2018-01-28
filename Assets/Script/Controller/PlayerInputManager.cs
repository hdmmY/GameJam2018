using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using InControl;

public class PlayerInputManager : MonoBehaviour
{
    public GameObject PlayerPrefab;
    public PlayerActions ControllerActions;
    public List<PlayerInputController> PlayerList = new List<PlayerInputController>();

    // Use this for initialization
    void Start()
    {

    }

    void OnEnable()
    {
        InputManager.OnDeviceDetached += InputManager_OnDeviceDetached;
        ControllerActions = PlayerActions.CreateController();
    }

    private void InputManager_OnDeviceDetached(InputDevice obj)
    {
        throw new System.NotImplementedException();
    }

    void OnDisable()
    {
        InputManager.OnDeviceDetached -= InputManager_OnDeviceDetached;

        ControllerActions.Destroy();
    }

    // Update is called once per frame
    void Update()
    {
        if (ControllerActions.Attack.WasPressed)
        {
            var inputDevice = InputManager.ActiveDevice;

            // Check whether there is a player using this device
            if (PlayerList.Where(player => player.Device == inputDevice).Count() == 0)
            {
                CreatePlayer(inputDevice);
            }
        }
    }

    public PlayerInputController CreatePlayer(InputDevice inputDevice)
    {
        var obj = Instantiate(PlayerPrefab);
        var player = obj.GetComponent<PlayerInputController>();
        PlayerList.Add(player);
        player.Actions = PlayerActions.CreateController();

        player.Actions.Device = inputDevice;
        player.Device = inputDevice;
        return player;
    }
}
