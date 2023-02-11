using System.Linq;
using _GAME.__Scripts.Home;
using _GAME.__Scripts.Truck;
using Rentire.Utils;
using TMPro;
using UnityEngine;

namespace _GAME.__Scripts.Ui
{
    public class AddTruckButton : IncrementalBaseButton
    {
        private HomeController _firstHome;

        [SerializeField] private TruckSo truckSo;
        [SerializeField] private TMP_Text maxText;
        [SerializeField] private TMP_Text nameText;
        [SerializeField] private GameObject coinArea;

        
        private void Start()
        {
            _firstHome = FindObjectsOfType<HomeController>().ToList().FirstOrDefault(x => x.isFirst);
            base.Start();
        }
        
        protected override float GetRequiredMoney()
        {
            return UserPrefs.GetTotalAddCarRequiredMoney();
        }

        public override void ClickButton()
        {
            EventManager.Instance.InvokeOnAddNewTruck();
        }

        protected override void SetInteractableButton()
        {
            if (UserPrefs.GetTotalMoney() >= GetRequiredMoney() && _firstHome.truckCount <= truckSo.maxAddCar)
            {
                incremantalButton.interactable = true;
            }
            else
            {
                incremantalButton.interactable = false;
            }

            if (_firstHome.truckCount > truckSo.maxAddCar)
            {
                maxText.gameObject.SetActive(true);
                nameText.gameObject.SetActive(false);
                coinArea.SetActive(false);
            }
            else
            {
                maxText.gameObject.SetActive(false);
                nameText.gameObject.SetActive(true);
                coinArea.SetActive(true);
            }
        }
    }
}
