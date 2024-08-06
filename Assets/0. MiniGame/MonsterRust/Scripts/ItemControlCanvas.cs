using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Minigame.MonsterRush
{
    public class ItemControlCanvas : MonoBehaviour
    {
        int gem;

        public Text textSet;

        int[] speedLv = { 0, 2, 4, 6, 8, 11, 15 };

        public void RandomGem()
        {
            gem = Random.Range(5, 21);

            textSet.text = $"+{gem} Gem";
        }
        public void SetTextSpeedCanvas()
        {
            textSet.text = $"+{speedLv[Controller.instance.playerScript.GetLevelSpeed()]}% -> " +
                $"{speedLv[Controller.instance.playerScript.GetLevelSpeed() + 1]}";
        }

        public int GetGem() {  return gem; }

        public void AddWeapon(GameObject weapon)
        {
            CanvasItem.instance.AddWeapon(weapon);
        }
        public void TakeGemCanvas(ItemControlCanvas gemControl)
        {
            CanvasItem.instance.TakeGemCanvas(gemControl);
        }
        public void AddGun(GameObject gun)
        {
            CanvasItem.instance.AddGun(gun);
        }
        public void AddMaxHeal()
        {
            CanvasItem.instance.AddMaxHeal();
        }
        public void SpeedUp()
        {
            CanvasItem.instance.SpeedUp();
        }
    }

}
