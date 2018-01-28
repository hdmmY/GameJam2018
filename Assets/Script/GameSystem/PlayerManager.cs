using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using InControl;

public class PlayerManager : MonoBehaviour {
    public GameObject PlayerPrefab;
    public PlayerActions ControllerActions;
    public List<Player> PlayerList = new List<Player>();

	// Use this for initialization
	void Start () {
		
	}

    void OnEnable()
    {
        InputManager.OnDeviceDetached += InputManager_OnDeviceDetached;
        ControllerActions = PlayerActions.CreateController();
    }

    private void InputManager_OnDeviceDetached(InputDevice device)
    {
        var player = PlayerList.Where(p => p.GetComponent<PlayerInputController>().Device == device).FirstOrDefault();
        if (player)
        {
            GetComponent<TeamManager>().LeaveTeam(player);
            PlayerList.Remove(player);
            Destroy(player);
        }
    }

    void OnDisable()
    {
        InputManager.OnDeviceDetached -= InputManager_OnDeviceDetached;

        ControllerActions.Destroy();
    }

    // Update is called once per frame
    void Update () {
        if (ControllerActions.Attack.WasPressed || ControllerActions.Jump.WasPressed)
        {
            var inputDevice = InputManager.ActiveDevice;

            // Check whether there is a player using this device
            if(PlayerList.Where(player=>player.GetComponent<PlayerInputController>().Device == inputDevice).Count() == 0)
            {
                CreatePlayer(inputDevice);
            }
        }
	}

    public PlayerInputController CreatePlayer(InputDevice inputDevice)
    {
        var obj = Instantiate(PlayerPrefab);
        var playerInput = obj.GetComponent<PlayerInputController>();
        var player = obj.GetComponent<Player>();

        PlayerList.Add(player);
        playerInput.Actions = PlayerActions.CreateController();


        playerInput.Actions.Device = inputDevice;
        playerInput.Device = inputDevice;
        var team = GameObject.Find("GameSystem").GetComponent<TeamManager>().JoinTeam(player);
        player.GetComponentInChildren<SkinnedMeshRenderer>().material = team.AvailableSkin;

        player.transform.position = team.Spawner.SpawnPosition();
        return playerInput;
    }
}
