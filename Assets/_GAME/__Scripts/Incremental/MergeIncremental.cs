using Rentire.Utils;

namespace _GAME.__Scripts.Incremental
{
    public class MergeIncremental : IncrementalBase
    {
        protected override void SetIncrementalText()
        {
            incrementalTextMoney.text = UserPrefs.GetTotalMergeRequiredMoney().ToString("0");
        }
        protected override void GetRequiredMoney()
        {
            requiredMoney = UserPrefs.GetTotalMergeRequiredMoney();
        }

        protected override void SetRequiredMoney()
        {
            UserPrefs.SetMergeRequiredMoney(requiredMoney);
        }
    }
}