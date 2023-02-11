using UnityEngine;

namespace _GAME.__Scripts.Click
{
    [CreateAssetMenu(menuName = "ClickSpeed", fileName = "NewClickSpeed")]
    public class ClickSo : ScriptableObject
    {
        [Header("Click Speed")]
        public float clickSpeedLimit = 2f;
        public float baseSpeed = 5;
        public float clickSpeed = 3;
        public float numberOfDecreaseSpeed = 0.1f;
        
        [Header("Click Speed UI")]
        
        public float baseSpeedUI = 10;
        public float clickSpeedUI = 15;
        public float clickSpeedUIMultiply = 0.1f;

        
        
    }
}
