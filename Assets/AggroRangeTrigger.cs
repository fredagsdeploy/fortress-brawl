using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AggroRangeTrigger : MonoBehaviour
{

    public CombatUnitManager combatUnitManager;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Ground")) return;
        Debug.Log(other);
        combatUnitManager.OnAggroRangeEnter(other);
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Ground")) return;
        combatUnitManager.OnAggroRangeExit(other);
    }
}
