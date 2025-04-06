using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ThroughTheWoods
{
    public class Controller : MonoBehaviour
    {
        public static Controller instance;
        [Header("Player")]
        public SPUM_PlayerController playerScript;
        [Header("Layer")]
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
    }
}
