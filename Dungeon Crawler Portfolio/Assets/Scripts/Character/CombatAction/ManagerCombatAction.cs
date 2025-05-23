using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class ManagerCombatAction : MonoBehaviour
{
    public CombatActionSO baseCombatAction;
    public PlayerInput playerInput;

    private void Awake()
    {

    }

    private void CastAbility(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            baseCombatAction.CallAction();
        }
    }

    private void OnEnable()
    {
        playerInput.actions.FindActionMap("Player").Enable();
        playerInput.actions.FindActionMap("Player").FindAction("BaseAttack").performed += CastAbility;
    }
    private void OnDisable()
    {
        playerInput.actions.FindActionMap("Player").Disable();
        playerInput.actions.FindActionMap("Player").FindAction("BaseAttack").performed -= CastAbility;
    }
}
