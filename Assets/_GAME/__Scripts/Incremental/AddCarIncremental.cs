using Rentire.Utils;

namespace _GAME.__Scripts.Incremental
{
    public class AddCarIncremental : IncrementalBase
    {
        protected override void SetIncrementalText()
        {
            incrementalTextMoney.text = UserPrefs.GetTotalAddCarRequiredMoney().ToString("0");
        }
        protected override void GetRequiredMoney()
        {
            requiredMoney = UserPrefs.GetTotalAddCarRequiredMoney();
        }

        protected override void SetRequiredMoney()
        {
            UserPrefs.SetAddCarRequiredMoney(requiredMoney);
        }
    }
}