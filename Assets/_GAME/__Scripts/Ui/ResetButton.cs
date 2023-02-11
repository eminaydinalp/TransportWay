using UnityEngine;

namespace _GAME.__Scripts.Ui
{
    public class ResetButton : MonoBehaviour
    {
        [SerializeField] private GameObject clickSpeed;
        public void ResetSpline()
        {
            clickSpeed.SetActive(false);
            EventManager.Instance.InvokeOnSplineReset();
        }
    }
}
