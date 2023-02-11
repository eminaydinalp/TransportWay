using Rentire.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace _GAME.__Scripts.Ui
{
    public abstract class IncrementalBaseButton : RMonoBehaviour
    {
        [SerializeField] protected Button incremantalButton;
        
        private void OnEnable()
        {
            if (eventManager)
            {
                eventManager.event_CollectionUpdated += SetInteractableButton;
            }
            EventManager.OnMergeFinish += SetInteractableButton;
            EventManager.OnAddNewTruck += SetInteractableButton;
            EventManager.OnClickSpeed += SetInteractableButton;
        }
        
        private void OnDisable()
        {
            if(eventManager) eventManager.event_CollectionUpdated -= SetInteractableButton;

            EventManager.OnMergeFinish -= SetInteractableButton;
            EventManager.OnAddNewTruck -= SetInteractableButton;
            EventManager.OnClickSpeed -= SetInteractableButton;
        }

        protected virtual void Start()
        {
            SetInteractableButton();
        }


        protected virtual void SetInteractableButton()
        {
            if (UserPrefs.GetTotalMoney() >= GetRequiredMoney())
            {
                incremantalButton.interactable = true;
            }
            else
            {
                incremantalButton.interactable = false;
            }
        }

        protected abstract float GetRequiredMoney();
        
        public abstract void ClickButton();

    }
}