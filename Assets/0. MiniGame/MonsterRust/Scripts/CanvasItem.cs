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
        public List<GameObject> weapons;
        public List<GameObject> skills;
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
            GameObject weaponGameObj = Instantiate(weapons[Random.Range(0, weapons.Count)], panelChoose.transform);


            GameObject effectGameObj = Instantiate(effects[Random.Range(0, effects.Count)], panelChoose.transform);

            ItemControlCanvas gemControl = effectGameObj.GetComponent<ItemControlCanvas>();

            if (gemControl != null )
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
