using Minigame.MonsterRush;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Minigame.MonsterRush
{
    public class CanvasItem : MonoBehaviour
    {
        public static CanvasItem instance;

        public GameObject panelParent;
        public GameObject panelChoose;
        [Header("--------- SpawnWeapons ---------")]
        public int rateSpawnGuns;
        [Header("-- SpawnMelees")]
        public List<GameObject> melees;
        [Header("-- SpawnGuns")]
        public List<GameObject> guns;

        [Header("--------- SpawnSkills ---------")]
        public List<GameObject> skills;
        [Header("--------- SpawnEffects ---------")]
        public List<GameObject> effects;

        private void Awake()
        {
            instance = this;
        }

        public void AddWeapon(GameObject weapon)
        {
            Controller.instance.playerScript.SetMelee(weapon);

            TurnOffPanel();
        }
        public void AddGun(GameObject gun)
        {
            Controller.instance.playerScript.SetGun(gun);

            TurnOffPanel();
        }
        public void AddMaxHeal()
        {
            Controller.instance.playerScript.AddMaxHeal();

            TurnOffPanel();
        }
        public void SpeedUp()
        {
            Controller.instance.playerScript.AddMaxHeal();

            TurnOffPanel();
        }

        public void TurnOnPanel()
        {
            Time.timeScale = 0;
            panelParent.SetActive(true);

            RandomItemCanvas();
        }
        public void TurnOffPanel()
        {
            Time.timeScale = 1;

            foreach (Transform child in panelChoose.transform)
            {
                Destroy(child.gameObject);
            }

            panelParent.SetActive(false);
        }
        public void RandomItemCanvas()
        {
            SpawnWeapon();

            SpawnEffect();
        }

        public void SpawnWeapon()
        {
            int getLevelGun = Controller.instance.playerScript.gun.levelGun;

            int rdWeapon = Random.Range(1, 101);

            if (rdWeapon <= rateSpawnGuns && getLevelGun < 3)
            {
                GameObject gunGameObj = Instantiate(guns[getLevelGun], panelChoose.transform);
            }
            else
            {
                GameObject meleeGameObj = Instantiate(melees[Random.Range(0, melees.Count)], panelChoose.transform);
            }
        }
        public void SpawnEffect()
        {
            GameObject effectGameObj = Instantiate(effects[Random.Range(0, effects.Count)], panelChoose.transform);

            ItemControlCanvas gemControl = effectGameObj.GetComponent<ItemControlCanvas>();

            if (gemControl != null)
            {
                gemControl.RandomGem();
            }
        }


        public void TakeGemCanvas(ItemControlCanvas gemControl)
        {
            Controller.instance.TakeGem(gemControl.GetGem());

            TurnOffPanel();
        }
    }

}
