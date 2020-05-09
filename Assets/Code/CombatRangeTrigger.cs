using UnityEngine;

namespace Code
{
    public class CombatRangeTrigger : MonoBehaviour
    {

        public CombatUnitManager combatUnitManager;

        private void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag("Ground")) return;
            combatUnitManager.OnCombatRangeEnter(other);
        }

        private void OnTriggerExit(Collider other)
        {
        
            if(other.CompareTag("Ground")) return;
            combatUnitManager.OnCombatRangeExit(other);
        }
    }
}
