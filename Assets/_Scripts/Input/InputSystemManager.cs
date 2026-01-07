using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputSystemManager : MonoBehaviour
{
    public Action OnControlsChanged;
    public ControllerStylesList stylesList;
    public PlayerInput playerInput;

    public void DeviceLost(PlayerInput playerInput)
    {
        /// Saltar pantalla diciendo que se desconecto el control
        Debug.LogWarning($"Player <b>#{playerInput.user.index + 1}</b> Device: <b>{playerInput.devices[0].displayName}</b> Lost");
    }

    public void DeviceRegained(PlayerInput playerInput)
    {
        /// QUITAR pantalla de pausa, el control se reconecto
        Debug.LogWarning($"Player <b>#{playerInput.user.index + 1}</b> Regained");
    }

    public void ControlsChanged(PlayerInput playerInput)
    {
        //Debug.LogWarning("-- ControlsChanged --");

        OnControlsChanged?.Invoke();
    }
}