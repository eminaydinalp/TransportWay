using UnityEngine;

namespace _GAME.__Scripts.Ui
{
    [CreateAssetMenu(menuName = "TargetPackage")]
    public class TargetPackageSo : ScriptableObject
    {
        public int targetCount;
        public int multipleIncrease;

        public int plusValue;

    }
}