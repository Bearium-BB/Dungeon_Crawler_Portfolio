using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class ManagerCombatAction : MonoBehaviour
{
    public CombatActionSO baseCombatAction;
    private CharacterControls inputActions;

    private void Awake()
    {
        inputActions = new CharacterControls();
        inputActions.Player.Enable();
        inputActions.Player.BaseAttack.performed += CastAbility;

    }

    private void CastAbility(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            baseCombatAction.CallAction();
        }
    }
}
