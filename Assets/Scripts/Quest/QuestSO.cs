using System.Collections.Generic;
using Reward;
using UnityEngine;

namespace Quest
{
    [CreateAssetMenu(menuName="Quest/Quest")]
    public class QuestSO : ScriptableObject
    {
        public TriggerZone.ZoneType zoneType;
        public bool isMainQuest = true;
        public string questId;
        public string title;
        public string description;

        public List<ObjectiveSO> objectives;

        public RewardData completionReward;
    }
}