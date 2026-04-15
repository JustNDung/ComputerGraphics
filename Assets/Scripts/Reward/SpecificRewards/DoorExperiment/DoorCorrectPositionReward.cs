namespace DefaultNamespace.SpecificRewards
{
    using UnityEngine;
    [CreateAssetMenu(menuName = "Reward/Rules/Door/CorrectPosition")]
    public class DoorCorrectPositionReward : RewardRuleSO
    {
        public override bool Evaluate(RewardEvent e)
        {
            return e.experimentId == "door"
                   && e.actionId == "handle"
                   && e.success;
        }

        public override RewardData GetReward(RewardEvent e)
        {
            return new RewardData
            {
                knowledgePoints = 20,
                message = "Tác dụng lực xa trục giúp mở cửa dễ hơn!"
            };
        }
    }
}