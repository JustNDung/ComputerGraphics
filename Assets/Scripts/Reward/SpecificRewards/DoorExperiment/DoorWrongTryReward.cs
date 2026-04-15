namespace DefaultNamespace.SpecificRewards
{
    using UnityEngine;
    [CreateAssetMenu(menuName = "Reward/Rules/Door/WrongTry")]
    public class DoorWrongTryReward : RewardRuleSO
    {
        public override bool Evaluate(RewardEvent e)
        {
            return e.experimentId == "door" && !e.success;
        }

        public override RewardData GetReward(RewardEvent e)
        {
            return new RewardData
            {
                knowledgePoints = 10,
                message = "Thử lại nhé! Gần trục quay sẽ khó mở hơn."
            };
        }
    }
}