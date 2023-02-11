using UnityEngine;

namespace _GAME.__Scripts.Incremental
{
    [CreateAssetMenu(menuName = "Incremental", fileName = "NewIncremental")]
    public class IncrementalSO : ScriptableObject
    {
        public float requiredMoney;
        
        public float multipleIncrease;

        public float plusValue;
    }
}