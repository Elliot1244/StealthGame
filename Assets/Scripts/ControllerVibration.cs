using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using XInputDotNetPure;

public class ControllerVibration : MonoBehaviour
{
    PlayerIndex playerIndex;
    GamePadState state;
    GamePadState prevState;

    private void Update()
    {
        //GamePad.SetVibration(playerIndex, .5f, .5f);
        Gamepad.current.SetMotorSpeeds(0.25f, 0.75f);
    }
}
