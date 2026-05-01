

namespace Reward
{
    using UnityEngine;

    public abstract class RewardRuleSO : ScriptableObject
    {
        public string ruleName;

        public abstract bool Evaluate(RewardEvent e);
        public abstract RewardData GetReward(RewardEvent e);
    }
}