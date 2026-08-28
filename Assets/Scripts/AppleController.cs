using System;
using UnityEngine;

public class AppleController : MonoBehaviour
{
    public static event Action<int> _applePicked;

    private int _appleCount = 0;

    public void ApplePickedCommand()
    {
        _appleCount++;
        _applePicked?.Invoke(_appleCount);
        Debug.Log("aaaaaple");
    }
}
