using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ThroughTheWoods
{
    public enum MonsterType
    {
        Skeleton,
        SkeletonArcher,
        DevilWitch
    }
    public class MissionManager : MonoBehaviour
    {
        [Header("Mission List")]
        public List<Mission> missions; // Danh sách nhiệm vụ
        Mission currentMission; // Nhiệm vụ hiện tại

        [Header("UI Elements")]
        public Text missionListText; // Text hiển thị danh sách nhiệm vụ

        void Start()
        {
            // Khởi tạo một vài nhiệm vụ mẫu
            // missions.Add(new Mission("Defeat 10 Monsters", 10, 100, 50));
            // missions.Add(new Mission("Defeat 5 Bosses", 5, 500, 200));

            currentMission = GetCurrentMission(); // Lấy nhiệm vụ hiện tại
            UpdateMissionUI(currentMission); // Cập nhật UI với nhiệm vụ hiện tại
        }

        public Mission GetCurrentMission()
        {
            foreach (var mission in missions)
            {
                if (!mission.isMissionCompleted)
                {
                    return mission;
                }
            }
            return null;
        }
        public void OnMonsterKilled(MonsterType monsterDeadType)
        {
            currentMission = GetCurrentMission();
            if (currentMission == null) return; // Nếu không có nhiệm vụ nào còn lại
            currentMission.OnMonsterKilled(monsterDeadType);
            UpdateMissionUI(currentMission);
        }

        private void UpdateMissionUI(Mission mission)
        {
            if (missionListText == null) return;

            missionListText.text = ""; // Xóa nội dung cũ
            string status = ""; // Trạng thái nhiệm vụ
            if (currentMission == null)
            {
                missionListText.text += $"+ Completed All Missions\n";
                return;
            }

            if (mission.isMissionCompleted)
            {
                status = "Completed";
                Invoke(nameof(NextMission), 3f); // Chuyển sang nhiệm vụ tiếp theo sau 3 giây
            }
            else
            {
                status = $"{mission.currentKillCount}/{mission.targetKillCount}";
            }
            missionListText.text += $"+ {mission.missionName}: {status}\n";
        }

        public void NextMission()
        {
            currentMission = GetCurrentMission();
            UpdateMissionUI(currentMission);
        }
    }
}