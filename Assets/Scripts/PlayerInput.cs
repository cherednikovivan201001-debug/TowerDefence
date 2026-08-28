using UnityEngine;
using System;
using static UnityEngine.InputSystem.InputAction;   

public class PlayerInput : MonoBehaviour
{
    public static event Action<Vector2> _onMoveCallback;

    public static event Action<Vector2> _onLookCallback;

    public static event Action _OnDance;

    public static event Action _onPlays;

    public static event Action<bool> _OnShoot;

    public void onMovePressed(CallbackContext context)
    {
        if (context.performed)
        {
            _onMoveCallback?.Invoke(context.ReadValue<Vector2>());
        }
        else
        {
            var zero = new Vector2(0, 0);
            _onMoveCallback?.Invoke(zero);
        }
    }

    public void OnLook(CallbackContext context)
    {
        Vector2 lookInput = context.ReadValue<Vector2>();
        
        if(lookInput.sqrMagnitude >= 3)
        {
            _onLookCallback?.Invoke(context.ReadValue<Vector2>());
        }
        else
        {
            Vector2 zero = new Vector2(0, 0);
            _onLookCallback?.Invoke(zero); 
        }

            //Debug.Log(context.ReadValue<Vector2>());
    }

    public void OnPlays(CallbackContext context)
    {
        if (context.performed)
        {
            _onPlays?.Invoke();
        }
    }

    public void OnDance(CallbackContext ctx)
    {
        if (ctx.performed)
        {
            _OnDance?.Invoke();
        }
    }

    public void OnShoot(CallbackContext ctx)
    {
        if (ctx.started)
        {
            _OnShoot?.Invoke(true);
        }
        if (ctx.canceled)
        {
            _OnShoot?.Invoke(false);
        }

    }
}
    