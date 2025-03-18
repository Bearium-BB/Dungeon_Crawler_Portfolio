using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class ManagerCombatAction : MonoBehaviour
{
    public CombatActionSO baseCombatAction;
    public ScriptableObjectCharacterControls inputActions;

    private void Awake()
    {
        inputActions.value.Player.Enable();
        inputActions.value.Player.BaseAttack.performed += CastAbility;

    }

    private void CastAbility(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            baseCombatAction.CallAction();
        }
    }
}
