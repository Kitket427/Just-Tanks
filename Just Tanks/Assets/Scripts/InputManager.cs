
using UnityEngine;

public class InputManager : MonoBehaviour
{
    internal TankControl control;
    public static InputManager inputManager;
    void Awake()
    {
        if (inputManager == null) inputManager = this;
        else
        {
            Debug.LogWarning("Fuck you, input(hueta)system, blyat XD");
            DestroyImmediate(gameObject);
        }
        control = new TankControl();
    }
}
