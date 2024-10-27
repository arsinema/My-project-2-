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
    
    [SerializeField] private DamageNumber damageNumber;
    [SerializeField] private Player player;
    private GameObject playerGameObject;
    [SerializeField] private AIDestinationSetter destinationSetter;
    [SerializeField] private AIPath aIPath;

    [SerializeField] float explodeRadiys;
    [SerializeField] int explodeDamage;
    public enum enemyType
    {
        Bomber = 1,
        Range = 2,
        Melee = 3
    };
    [SerializeField] private enemyType enemyT = Enemy.enemyType.Melee;
    
    //Очки перносажа
    [Header("Enemy points")]
    [SerializeField] public int enemyHP = 100;
    [SerializeField] public int enemySouls = 100;
    [SerializeField] public int enemyDamage = 5;

    //Создание новога врага после убийства
    [Header("Create new enemy with new preference")]
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] GameObject[] spawPointsEnemy;
    [SerializeField] GameObject prefab;
    private GameObject spawnPoint;

    

    private void Start()
    {
        spawPointsEnemy[0] = GameObject.Find("Enemy spawn point");
        spawPointsEnemy[1] = GameObject.Find("Enemy spawn point");
        spawPointsEnemy[2] = GameObject.Find("Enemy spawn point (1)");
        spawPointsEnemy[3] = GameObject.Find("Enemy spawn point (2)");

    }

    

    private void Update()
    {
        playerGameObject = GameObject.Find("Player");
       
        destinationSetter.target = playerGameObject.transform;

        if (enemyT == enemyType.Bomber)
        {
            Debug.Log("Bomber");
            aIPath.endReachedDistance = 2;
            aIPath.maxSpeed = 6;
            if (Vector3.Distance(transform.position, playerGameObject.transform.position) <= 2)
            {
                Explode();
            }
        }
        else if (enemyT == enemyType.Range)
        {
            aIPath.maxSpeed = 2;
            aIPath.endReachedDistance = 4;
        }
        else if (enemyT == enemyType.Melee)
        {
            aIPath.maxSpeed = 4;
            aIPath.endReachedDistance = 2;
        }    
    }

    public void HitReact()
    {
        Debug.Log("Враг убит");
        enemyHP = 90;
        enemyDamage += 2;

        Dead();
    }
    public void Hitted(int damage)
    {

        player.playerHP += player.vampire;
        Vector3 textSpawn = new Vector3(transform.position.x, transform.position.y + 1, 0);
        enemyHP -= damage;
        damageNumber.Spawn(textSpawn, damage, transform);
        return;
        
    }

    public void Dead()
    {
        spawnPoint = spawPointsEnemy[UnityEngine.Random.Range(0, spawPointsEnemy.Length)];
        Instantiate(enemyPrefab, new Vector3(spawnPoint.transform.position.x, spawnPoint.transform.position.y, 0), Quaternion.identity);
        Destroy(prefab);
    }

    private void Explode()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explodeRadiys);

        Debug.Log(colliders == null);

        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("enemy"))
            {
                Enemy enemy = collider.gameObject.GetComponent<Enemy>();

                enemy.enemyHP -= explodeDamage;
                Debug.Log("Enemy explode");
            }
            else if (collider.CompareTag("Player"))
            {
                Debug.Log("Player explode");

                Player player = collider.gameObject.GetComponent<Player>();

                player.playerHP -= explodeDamage;
            }
            else {  }
        }
        Dead();
        Destroy(gameObject);
    }
}

