using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

    }

    private void Update()
    {
        ChangeCursorState();
    }

    public Vector3 GetMoveInput()
    {
        if (CheckCursorState())
        {
            Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
            move = Vector3.ClampMagnitude(move, 1f);
            return move;
        }
        return Vector3.zero;
    }
    public bool GetSprintInput()
    {
        if (CheckCursorState())
        {
            return Input.GetKey(KeyCode.LeftShift);
        }
        return false;
    }
    public bool GetCrouchInput()
    {
        if (CheckCursorState())
        {
            return Input.GetKey(KeyCode.LeftControl);
        }
        return false;
    }
    public bool GetJumpInput()
    {
        if (CheckCursorState())
        {
            return Input.GetKeyDown(KeyCode.Space);
        }
        return false;
    }
    
    public bool GetFireInputDown()
    {
        if (CheckCursorState())
        {
            return Input.GetMouseButtonDown(0);
        }
        return false;
    }
    public bool GetFireInputHeld()
    {
        if (CheckCursorState())
        {
            return Input.GetMouseButton(0);
        }
        return false;
    }
    public bool GetFireInputUp()
    {
        if (CheckCursorState())
        {
            return Input.GetMouseButtonUp(0);
        }
        return false;
    }
    public bool GetAimInputDown()
    {
        if (CheckCursorState())
        {
            return Input.GetMouseButtonDown(1);
        }
        return false;
    }
    public bool GetReloadInputDown()
    {
        if (CheckCursorState())
        {
            return Input.GetKeyDown(KeyCode.R);
        }
        return false;
    }
    public int GetSwitchWeapon()
    {
        if (CheckCursorState())
        {
            if (Input.GetKeyDown(KeyCode.Q)) return -1;
            else if (Input.GetKeyDown(KeyCode.E)) return 1;
        }
        return 0;
    }


    public float GetMouseXAxis()
    {
        if (CheckCursorState())
        {
            return Input.GetAxis("Mouse X");
        }
        return 0;
    }
    public float GetMouseYAxis()
    {
        if (CheckCursorState())
        {
            return Input.GetAxis("Mouse Y");
        }
        return 0;
    }
    public float GetMouseScrollWheel()
    {
        if (CheckCursorState())
        {
            return Input.GetAxis("Mouse ScrollWheel");
        }
        return 0;
    }

    private bool CheckCursorState()
    {
        if (Cursor.lockState == CursorLockMode.Locked)
            return true;
        return false;       
    }
    private void ChangeCursorState()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Cursor.lockState == CursorLockMode.None)
                Cursor.lockState = CursorLockMode.Locked;
            else
                Cursor.lockState = CursorLockMode.None;
        }
    }
}
