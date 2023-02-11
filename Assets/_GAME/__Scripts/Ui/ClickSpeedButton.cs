using _GAME.__Scripts.Truck;
using Rentire.Utils;
using UnityEngine;

namespace _GAME.__Scripts.Ui
{
    public class ClickSpeedButton : IncrementalBaseButton
    {
        protected override float GetRequiredMoney()
        {
            return UserPrefs.GetTotalClickSpeedRequiredMoney();
        }

        public override void ClickButton()
        {
            EventManager.Instance.InvokeOnClickSpeed();
        }

        protected override void SetInteractableButton()
        {
            if (UserPrefs.GetTotalMoney() >= GetRequiredMoney() && TruckManager.Instance.clickSo.clickSpeed > TruckManager.Instance.clickSo.clickSpeedLimit)
            {
                incremantalButton.interactable = true;
            }
            else
            {
                incremantalButton.interactable = false;
            }
            
        }
    }
}