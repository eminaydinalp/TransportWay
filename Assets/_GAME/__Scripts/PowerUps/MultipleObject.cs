using _GAME.__Scripts.Truck;
using _GAME.__Scripts.Ui;

namespace _GAME.__Scripts.Spawner
{
    public class MultipleObject : PowerUpBase
    {
        protected override void IncreaseProcess(TruckController truckController)
        {
            MoneyManager.Instance.IncreaseMoney(moneyAmount * truckController.increaseMoney);
        }
    }
}