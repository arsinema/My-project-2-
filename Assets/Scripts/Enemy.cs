using UnityEngine;
using System.Collections;
using DamageNumbersPro;
using System;
using Unity.VisualScripting;
using Pathfinding;

public class Enemy : MonoBehaviour
{
    //Главные настройки персонажа
    [Header("Enemy preference")]
    [SerializeField] public static int enemyDamage = 5;
    [SerializeField] private DamageNumber damageNumber;
    
    //Очки перносажа
    [Header("Enemy points")]
    [SerializeField] public static int enemyHP = 100;
    [SerializeField] public static int enemySouls = 100;

    //Создание новога врага после убийства
    [Header("Create new enemy with new preference")]
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] GameObject[] spawPointsEnemy;
    private GameObject spawnPoint;
    

    
    public void HitReact()
    {
        Debug.Log("Враг убит");
        enemyHP = 90;
        enemyHP += 10;
        enemyDamage += 2;

        
        spawnPoint = spawPointsEnemy[UnityEngine.Random.Range(0, spawPointsEnemy.Length)];
        Instantiate(enemyPrefab, new Vector3(spawnPoint.transform.position.x, spawnPoint.transform.position.y, 0), Quaternion.identity);
        Destroy(gameObject);

    }
    public void Hitted(int damage, int HP, int vampire)
    {
        Player.playerHP += Player.vampire;
        Vector3 textSpawn = new Vector3(transform.position.x, transform.position.y + 1, 0);
        enemyHP -= damage;
        damageNumber.Spawn(textSpawn, damage, transform);
        return;
        
    }
}

