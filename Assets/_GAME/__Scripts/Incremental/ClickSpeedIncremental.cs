using Rentire.Utils;

namespace _GAME.__Scripts.Incremental
{
    public class ClickSpeedIncremental : IncrementalBase
    {
        protected override void SetIncrementalText()
        {
            incrementalTextMoney.text = UserPrefs.GetTotalClickSpeedRequiredMoney().ToString("0");

        }
        protected override void GetRequiredMoney()
        {
            requiredMoney = UserPrefs.GetTotalClickSpeedRequiredMoney();
        }

        protected override void SetRequiredMoney()
        {
            UserPrefs.SetClickSpeedRequiredMoney(requiredMoney);
        }
    }
}