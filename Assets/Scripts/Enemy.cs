using UnityEngine;
using System.Collections;
using DamageNumbersPro;
using System;
using Unity.VisualScripting;
using Pathfinding;

public class Enemy : MonoBehaviour
{
    //������� ��������� ���������
    [Header("Enemy preference")]
    
    [SerializeField] private DamageNumber damageNumber;
    [SerializeField] private Player player;
    private GameObject playerGameObject;
    [SerializeField] private AIDestinationSetter destinationSetter;
    [SerializeField] private AIPath aIPath;

    [Header("Bomb enemy preferense")]
    [SerializeField] float explodeRadiys;
    [SerializeField] int explodeDamage;

    [Header("Range enemy preferense")]
    [SerializeField] int rangeDamage;
    [SerializeField] float rechargeTime = 3.0f;
    [SerializeField] GameObject shootPoint;
    private bool hited = true;
    public enum enemyType
    {
        Bomber = 1,
        Range = 2,
        Melee = 3
    };
    [SerializeField] private enemyType enemyT = Enemy.enemyType.Melee;
    
    //���� ���������
    [Header("Enemy points")]
    [SerializeField] public int enemyHP = 100;
    [SerializeField] public int enemySouls = 100;
    [SerializeField] public int enemyDamage = 5;

    //�������� ������ ����� ����� ��������
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
            aIPath.endReachedDistance = 2;
            aIPath.maxSpeed = 6;
            aIPath.whenCloseToDestination = CloseToDestinationMode.ContinueToExactDestination;
            if (Vector3.Distance(transform.position, playerGameObject.transform.position) <= 2)
            {
                Explode();
            }
        }
        else if (enemyT == enemyType.Range)
        {
            if (hited)
            {
                RangeAtack();
            }
            aIPath.maxSpeed = 2;
            aIPath.endReachedDistance = 4;
            aIPath.whenCloseToDestination = CloseToDestinationMode.Stop;
        }
        else if (enemyT == enemyType.Melee)
        {
            aIPath.maxSpeed = 4;
            aIPath.endReachedDistance = 2;
            aIPath.whenCloseToDestination = CloseToDestinationMode.ContinueToExactDestination;
        }    
    }

    public void HitReact()
    {
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
        int rand = UnityEngine.Random.Range(1, 3);

        

        Enemy enemy = new Enemy();
        if(rand == 1)
        {
            enemy.enemyT = enemyType.Bomber;
        }
        if (rand == 2)
        {
            enemy.enemyT = enemyType.Range;
        }
        if (rand == 3)
        {
            enemy.enemyT = enemyType.Melee;
        }

        spawnPoint = spawPointsEnemy[UnityEngine.Random.Range(0, spawPointsEnemy.Length)];
        Instantiate(enemyPrefab, new Vector3(spawnPoint.transform.position.x, spawnPoint.transform.position.y, 0), Quaternion.identity);
        Destroy(prefab);
    }

    private void Explode()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explodeRadiys);

        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("enemy"))
            {
                Enemy enemy = collider.gameObject.GetComponent<Enemy>();

                enemy.enemyHP -= explodeDamage;
            }
            else if (collider.CompareTag("Player"))
            {

                Player player = collider.gameObject.GetComponent<Player>();

                player.playerHP -= explodeDamage;
            }
            else {  }
        }
        Dead();
        Destroy(gameObject);
    }

    private void RangeAtack()
    {
        Vector3 diference = playerGameObject.transform.position - shootPoint.transform.position;

        RaycastHit2D hit = Physics2D.Raycast(shootPoint.transform.position, diference);

        Debug.DrawRay(shootPoint.transform.position, diference,Color.red, 3f);
        Debug.Log(hit.collider.tag);
        if (hited)
        {
            if (hit.collider.tag == "Player")
            {
                Debug.Log("Hit");
                Player player = hit.collider.gameObject.GetComponent<Player>();

                player.playerHP -= rangeDamage;
                
            }
        }
        hited = false;
        StartCoroutine(RechargeRangeAtack());

    }

    IEnumerator RechargeRangeAtack()
    {
        
        yield return new WaitForSeconds(rechargeTime);
        hited = true;
        
    }
}

