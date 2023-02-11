using _GAME.__Scripts.Truck;
using _GAME.__Scripts.Ui;

namespace _GAME.__Scripts.Spawner
{
    public class PlusObject : PowerUpBase
    {
        protected override void IncreaseProcess(TruckController truckController)
        {
            MoneyManager.Instance.IncreaseMoney(moneyAmount);
        }
    }
}