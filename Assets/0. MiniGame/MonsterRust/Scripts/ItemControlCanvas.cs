using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Minigame.MonsterRush
{
    public class ItemControlCanvas : MonoBehaviour
    {
        int gem;

        public Text textGem;

        public void RandomGem()
        {
            gem = Random.Range(5, 21);

            textGem.text = $"+{gem} Gem";
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
    }

}
