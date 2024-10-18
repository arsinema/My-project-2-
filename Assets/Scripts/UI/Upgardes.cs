using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Upgardes : MonoBehaviour
{
    [SerializeField] Canvas canvas;

    [SerializeField] Button upgradeFirst;
    [SerializeField] TextMeshProUGUI upgradeFirstText;

    [SerializeField] Button upgradeSeond;
    [SerializeField]TextMeshProUGUI upgradeSecondText;

    [SerializeField] Button upgradeThird;
    [SerializeField] TextMeshProUGUI upgradeThirdText;

    [SerializeField] Player player;

    public void upgrade1()
    {
        upgradeFirstText.text = "Базовый урон увеличивается на 20";
        player.basicDamage += 20;
    }
    public void upgrade2()
    {
        //Скорость увеличчивается на 20
        upgradeSecondText.text = "Скорость персонажа увеличивается на 2";
        player.normalSpeed += 2;
    }
    public void upgrade3()
    {
        //Вампиризм увеличчивается на 5
        upgradeThirdText.text = "Вампиризм от попадания увеличивается на 5";
        Player.vampire += 5;
        
    }
    public void OnButtonUp()
    {
        Destroy(canvas);
    }
}
