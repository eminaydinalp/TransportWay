using _GAME.__Scripts.Truck;
using DG.Tweening;
using Rentire.Utils;
using UnityEngine;

namespace _GAME.__Scripts.Ui
{
    public class MergeButton : IncrementalBaseButton
    {
        protected override float GetRequiredMoney()
        {
            return UserPrefs.GetTotalMergeRequiredMoney();
        }

        public override void ClickButton()
        {
            TutorialManager.Instance.DisableTutorialVignette();
            EventManager.Instance.InvokeOnMergeButtonClick();
            CallMethodWithDelay(()=> transform.DOScale(Vector3.one, 0.1f) , 1);
        }

        protected override void SetInteractableButton()
        {
            if (UserPrefs.GetTotalMoney() >= GetRequiredMoney() && MergeManager.Instance.IsMergeAble())
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