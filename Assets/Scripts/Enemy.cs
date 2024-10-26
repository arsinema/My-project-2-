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
    [SerializeField] public int enemyDamage = 5;
    [SerializeField] private DamageNumber damageNumber;
    [SerializeField] private Player player;
    private GameObject playerGameObject;
    [SerializeField] private AIDestinationSetter destinationSetter;
    private AIPath aIPath;
    [SerializeField] private enum enemyType
    {
        Bomber,
        Range,
        Melee
    };
    
    //Очки перносажа
    [Header("Enemy points")]
    [SerializeField] public int enemyHP = 100;
    [SerializeField] public int enemySouls = 100;

    //Создание новога врага после убийства
    [Header("Create new enemy with new preference")]
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] GameObject[] spawPointsEnemy;
    [SerializeField] GameObject prefab;
    private GameObject spawnPoint;

    private void Start()
    {
        aIPath = GetComponent<AIPath>();
        spawPointsEnemy[0] = GameObject.Find("Enemy spawn point");
        spawPointsEnemy[1] = GameObject.Find("Enemy spawn point");
        spawPointsEnemy[2] = GameObject.Find("Enemy spawn point (1)");
        spawPointsEnemy[3] = GameObject.Find("Enemy spawn point (2)");

    }

    private void Update()
    {
        playerGameObject = GameObject.Find("Player");
       
        destinationSetter.target = playerGameObject.transform;

        switch (enemyType)
        {
            case enemyType.Bomber:
                aIPath.endReachedDistance = 1;
                aIPath.maxSpeed = 6;
                break;

            case enemyType.Range:
                aIPath.endReachedDistance = 4;
                break;

            case enemyType.Melee:
                aIPath.endReachedDistance = 1;
                break;
            default:
                break;
        }

    }

    public void HitReact()
    {
        Debug.Log("Враг убит");
        enemyHP = 90;
        enemyDamage += 2;

        spawnPoint = spawPointsEnemy[UnityEngine.Random.Range(0, spawPointsEnemy.Length)];
        Instantiate(enemyPrefab, new Vector3(spawnPoint.transform.position.x, spawnPoint.transform.position.y, 0), Quaternion.identity);
        Destroy(prefab);


    }
    public void Hitted(int damage)
    {

        player.playerHP += player.vampire;
        Vector3 textSpawn = new Vector3(transform.position.x, transform.position.y + 1, 0);
        enemyHP -= damage;
        damageNumber.Spawn(textSpawn, damage, transform);
        return;
        
    }
}

