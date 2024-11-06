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
    private Color originalColor;
    [SerializeField] private AIDestinationSetter destinationSetter;
    [SerializeField] private AIPath aIPath;
    [SerializeField] private SpriteRenderer spriteRenderer;
    private float timeToWaitAftherTakingDamage = 0.1f;
    public enum enemyType
    {
        Bomber = 1,
        Range = 2,
        Melee = 3
    };
    [SerializeField] private enemyType enemyT = Enemy.enemyType.Melee;

    [Header("Bomb enemy preferense")]
    [SerializeField] float explodeRadiys;
    [SerializeField] int explodeDamage;

    [Header("Range enemy preferense")]
    [SerializeField] int rangeDamage;
    [SerializeField] float rechargeTime = 3.0f;
    [SerializeField] GameObject shootPoint;
    private bool hited = true;

    //Очки перносажа
    [Header("Enemy points")]
    [SerializeField] public int enemyHP = 100;
    [SerializeField] public int enemySouls = 100;
    [SerializeField] public int enemyDamage = 5;

    //Создание новога врага после убийства
    [Header("Create new enemy with new preference")]
    [SerializeField] GameObject[] spawPointsEnemy;
    [SerializeField] GameObject prefab;
    [SerializeField] GameObject[] enemesPrefab;
    private GameObject spawnPoint;

    

    private void Start()
    {
        spawPointsEnemy[0] = GameObject.Find("Enemy spawn point");
        spawPointsEnemy[1] = GameObject.Find("Enemy spawn point");
        spawPointsEnemy[2] = GameObject.Find("Enemy spawn point (1)");
        spawPointsEnemy[3] = GameObject.Find("Enemy spawn point (2)");

        spriteRenderer = GetComponent<SpriteRenderer>();

        originalColor = spriteRenderer.color;
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
        ChangeCollorAfterTakingDamage();
        player.playerHP += player.vampire;
        Vector3 textSpawn = new Vector3(transform.position.x, transform.position.y + 1, 0);
        enemyHP -= damage;
        damageNumber.Spawn(textSpawn, damage, transform);
        return;
        
    }

    public void Dead()
    {
        int rand = UnityEngine.Random.Range(0, enemesPrefab.Length);
        spawnPoint = spawPointsEnemy[UnityEngine.Random.Range(0, spawPointsEnemy.Length)];
        Instantiate(enemesPrefab[rand], new Vector3(spawnPoint.transform.position.x, spawnPoint.transform.position.y, 0), Quaternion.identity);
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

        RaycastHit2D hit = Physics2D.Raycast(shootPoint.transform.position, diference, Mathf.Infinity, 7);

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

    private void ChangeCollorAfterTakingDamage()
    {
        spriteRenderer.color = Color.Lerp(originalColor, Color.red, 0.1f);

        
        Invoke("RestoreColor", timeToWaitAftherTakingDamage); 
    }

    private void RestoreColor()
    {
        spriteRenderer.color = originalColor;
    }
}