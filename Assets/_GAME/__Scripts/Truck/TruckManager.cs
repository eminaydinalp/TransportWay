using _GAME.__Scripts.Click;
using _GAME.__Scripts.Incremental;
using _GAME.__Scripts.Spline;
using DG.Tweening;
using Rentire.Core;
using TMPro;
using UnityEngine;

namespace _GAME.__Scripts.Truck
{
    public class TruckManager : Singleton<TruckManager>
    {
        public ClickSo clickSo;
        
        public float baseSpeedTimer = 1;
        public float currentSpeed;
        public float currentClickSpeed;
        
        public float currentSpeedTimer;

        [SerializeField] private IncrementalBase clickSpeedIncremental;

        public float currentSpeedUI;
        public float currentClickSpeedUI;

        public TMP_Text clickSpeedText;

        public string clickSpeedPrefUI;
        public string clickSpeedPref;

        public Transform truckParent;

        private void Start()
        {
            currentClickSpeedUI = LocalPrefs.GetFloat(clickSpeedPrefUI, clickSo.clickSpeedUI);
            currentClickSpeed = LocalPrefs.GetFloat(clickSpeedPref, clickSo.clickSpeed);
            currentSpeedUI = clickSo.baseSpeedUI;
        }

        private void OnEnable()
        {
            EventManager.OnClickSpeed += IncreaseSpeed;
        }
        
        private void OnDisable()
        {
            EventManager.OnClickSpeed -= IncreaseSpeed;
        }
        
        private void FixedUpdate()
        {
            if (gameState != GameState.Running)
                return;
            currentSpeedTimer -= Time.fixedDeltaTime;
            if ( currentSpeedTimer <= 0)
            {
                currentSpeed = currentSpeed.RLerp(clickSo.baseSpeed, Time.fixedDeltaTime * 5f);
                currentSpeedUI = currentSpeedUI.RLerp(clickSo.baseSpeedUI, Time.fixedDeltaTime * 5f);
                clickSpeedText.text = currentSpeedUI.ToString("F1") + "/km";
            }
        }

        public void SpeedUp()
        {
            if(SplineManager.Instance.activeSplinePointController == null) return;
            
            if(SplineManager.Instance.activeSplinePointController.isActive) return;
            
            if (currentSpeedTimer <= 0)
            {
                DOTween.To(() => currentSpeed ,x=> currentSpeed=x,currentClickSpeed,0.4f);
                DOVirtual.Float(currentSpeedUI, currentClickSpeedUI, 0.4f, value =>
                {
                    currentSpeedUI = value;
                    clickSpeedText.text = value.ToString("F1") + "/km";
                });
            }
            currentSpeedTimer = baseSpeedTimer;
        }

        private void IncreaseSpeed()
        {
            if(!clickSpeedIncremental.RequireMoney() && clickSo.clickSpeed <= clickSo.clickSpeedLimit) return;
            currentClickSpeed -= clickSo.numberOfDecreaseSpeed;
            currentClickSpeedUI += currentClickSpeedUI * clickSo.clickSpeedUIMultiply;
            LocalPrefs.SetFloat(clickSpeedPrefUI, currentClickSpeedUI);
            LocalPrefs.SetFloat(clickSpeedPref, currentClickSpeed);
        }
        
    }
}