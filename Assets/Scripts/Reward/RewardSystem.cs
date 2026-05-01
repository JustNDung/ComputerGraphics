
namespace Reward
{
    using System.Collections.Generic;
    using UnityEngine;

    public class RewardSystem : MonoBehaviour
    {
        public static RewardSystem Instance;

        [SerializeField] private List<RewardRuleSO> rules;

        private int totalKP;

        private void Awake()
        {
            Instance = this;
        }

        public void ProcessEvent(RewardEvent e)
        {
            foreach (var rule in rules)
            {
                if (rule.Evaluate(e))
                {
                    var reward = rule.GetReward(e);
                    ApplyReward(reward);
                }
            }
        }

        private void ApplyReward(RewardData reward)
        {
            totalKP += reward.knowledgePoints;

            Debug.Log($"+{reward.knowledgePoints} KP: {reward.message}");

            // UIManager.Instance.ShowReward(reward);
        }
    }
}