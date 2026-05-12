using Reward;
using UnityEngine;
using System;
using System.Collections.Generic;

namespace Quest
{
    public class QuestManager : MonoBehaviour
    {
        public static QuestManager Instance;

        [SerializeField] private List<QuestSO> startQuests;

        private List<QuestInstance> activeQuests =
            new List<QuestInstance>();

        public Action OnQuestUpdated;

        // NEW
        public Action<QuestInstance> OnQuestCompleted;

        public IReadOnlyList<QuestInstance> ActiveQuests => activeQuests;

        private void Awake()
        {
            Instance = this;

            foreach (var q in startQuests)
                AddQuest(q);
        }

        public void AddQuest(QuestSO quest)
        {
            QuestInstance instance = new QuestInstance(quest);

            instance.OnQuestCompleted += HandleQuestCompleted;

            activeQuests.Add(instance);

            OnQuestUpdated?.Invoke();
        }

        private void HandleQuestCompleted(QuestInstance quest)
        {
            CompleteQuest(quest);

            OnQuestCompleted?.Invoke(quest);
        }

        public void ProcessEvent(RewardEvent e)
        {
            foreach (var quest in activeQuests)
            {
                quest.ProcessEvent(e);
            }

            OnQuestUpdated?.Invoke();
        }

        void CompleteQuest(QuestInstance quest)
        {
            RewardSystem.Instance.ProcessEvent(new RewardEvent
            {
                experimentId = "Quest",
                actionId = quest.data.questId,
                success = true
            });

            Debug.Log("Quest Complete: " + quest.data.title);
        }
    }
}