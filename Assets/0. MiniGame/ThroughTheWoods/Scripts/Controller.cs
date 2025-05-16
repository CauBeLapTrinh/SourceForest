using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ThroughTheWoods
{
    public class Controller : MonoBehaviour
    {
        public static Controller instance;
        [Header("Player")]
        public SPUM_PlayerController playerScript;
        [Header("Layer")]
        public LayerMask playerLayer;
        public LayerMask enemyLayer;
        [Header("Control")]
        public ControlPrefabs controlPrefabs;
        public ControlCanvasUI controlCanvasUI;
        public MissionManager missionManager;
        public List<SkillUI> skillUI;
        void Awake()
        {
            instance = this;
        }
        void OnDestroy()
        {
            instance = null;
        }
        // Start is called before the first frame update
        void Start()
        {
            if (PlayerPrefs.GetInt("ThroughTheWoods_IsHaveData", 0) == 1)
            {
                LoadPlayerData(playerScript);
            }
            else
            {
                playerScript.LoadCharacterBar();
                Debug.Log("No data found, creating new player data.");
            }
        }

        // Update is called once per frame
        void Update()
        {
            ListenerSkillUI();
        }
        public void ListenerSkillUI()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                skillUI[0].OnClickSkillButton();
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                skillUI[1].OnClickSkillButton();
            }
        }
        public void SetSkillUI(int index)
        {
            skillUI[index].OnClickSkillButton();
        }
        public void SetWeaponPlayer(int indexWeapon, SkillUI skillUISet)
        {
            playerScript.SetWeaponSprite(indexWeapon, skillUISet);
        }
        public void SavePlayerPrefs()
        {
            PlayerData data = playerScript.GetPlayerData();
            PlayerPrefs.SetInt("ThroughTheWoods_IsHaveData", 1);
            // Lưu chỉ số cơ bản của người chơi
            PlayerPrefs.SetInt("ThroughTheWoods_DamageDefault", data.damageDefault);
            PlayerPrefs.SetInt("ThroughTheWoods_Cristical", data.cristical);
            PlayerPrefs.SetInt("ThroughTheWoods_AttributeCount", data.attributeCount);
            PlayerPrefs.SetInt("ThroughTheWoods_CurAttributeHeal", data.curAttributeHeal);
            PlayerPrefs.SetInt("ThroughTheWoods_CurAttributeDamage", data.curAttributeDamage);
            PlayerPrefs.SetInt("ThroughTheWoods_CurAttributeCristical", data.curAttributeCristical);
            PlayerPrefs.SetInt("ThroughTheWoods_Level", data.curLevel);
            PlayerPrefs.SetInt("ThroughTheWoods_Exp", data.curExp);
            PlayerPrefs.SetFloat("ThroughTheWoods_MaxHp", data.maxHp);
            PlayerPrefs.SetFloat("ThroughTheWoods_CurrentHp", data.currentHp);
            PlayerPrefs.SetFloat("ThroughTheWoods_MaxMp", data.maxMp);
            PlayerPrefs.SetFloat("ThroughTheWoods_CurrentMp", data.currentMp);
            Debug.Log("data.curMp: " + data.currentMp);

            // Lưu vị trí của người chơi
            Vector3 playerPosition = data.position;
            PlayerPrefs.SetFloat("ThroughTheWoods_PlayerPosX", playerPosition.x);
            PlayerPrefs.SetFloat("ThroughTheWoods_PlayerPosY", playerPosition.y);
            PlayerPrefs.SetFloat("ThroughTheWoods_PlayerPosZ", playerPosition.z);

            // Lưu trạng thái các nhiệm vụ
            if (missionManager != null)
            {
                for (int i = 0; i < missionManager.missions.Count; i++)
                {
                    Mission mission = missionManager.missions[i];
                    PlayerPrefs.SetInt($"ThroughTheWoods_Mission_{i}_CurrentKillCount", mission.currentKillCount);
                    PlayerPrefs.SetInt($"ThroughTheWoods_Mission_{i}_IsCompleted", mission.isMissionCompleted ? 1 : 0);
                }
            }

            // Lưu dữ liệu
            PlayerPrefs.Save();
            Debug.Log("Game data saved successfully!");
            Time.timeScale = 1;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        }
        public void LoadPlayerData(SPUM_PlayerController player)
        {
            PlayerData data = new()
            {
                damageDefault = PlayerPrefs.GetInt("ThroughTheWoods_DamageDefault", 50),
                cristical = PlayerPrefs.GetInt("ThroughTheWoods_Cristical", 10),
                attributeCount = PlayerPrefs.GetInt("ThroughTheWoods_AttributeCount", 0),
                curAttributeHeal = PlayerPrefs.GetInt("ThroughTheWoods_CurAttributeHeal", 0),
                curAttributeDamage = PlayerPrefs.GetInt("ThroughTheWoods_CurAttributeDamage", 0),
                curAttributeCristical = PlayerPrefs.GetInt("ThroughTheWoods_CurAttributeCristical", 0),
                curLevel = PlayerPrefs.GetInt("ThroughTheWoods_Level", 1),
                curExp = PlayerPrefs.GetInt("ThroughTheWoods_Exp", 0),
                maxHp = PlayerPrefs.GetFloat("ThroughTheWoods_MaxHp", 100),
                currentHp = PlayerPrefs.GetFloat("ThroughTheWoods_CurrentHp", 100),
                maxMp = PlayerPrefs.GetFloat("ThroughTheWoods_MaxMp", 50),
                currentMp = PlayerPrefs.GetFloat("ThroughTheWoods_CurrentMp", 50),
                position = new Vector3(
                    PlayerPrefs.GetFloat("ThroughTheWoods_PlayerPosX", 0),
                    PlayerPrefs.GetFloat("ThroughTheWoods_PlayerPosY", 0),
                    PlayerPrefs.GetFloat("ThroughTheWoods_PlayerPosZ", 0)
                )
            };
            Debug.Log("data.curMp: " + data.currentMp);

            player.LoadData(data);
        }
    }
}
