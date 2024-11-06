using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [Header("Player preference")]
    [SerializeField] public float speed = 0f;
    [SerializeField] public int damage;
    [SerializeField] public int playerHP = 100;
    [SerializeField] public int playerSouls = 0;
    [SerializeField] float sprintSpeed = 10;
    [SerializeField] bool isMouseLookLeft = false;
    [SerializeField] public int vampire = 0;
    [SerializeField] public GameObject Upgardes;
    private Vector2 moveVector;

    [Header("Player damage")]
    [SerializeField] public int basicDamage = 40;


    [Header("Player shooting setings")]
    [SerializeField] GameObject shotPoint;


    [Header("Player UI")]
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI HPText;
    [SerializeField] GameObject canvasPrefab;
    [SerializeField] VariableJoystick joystick;
    Vector2 pos;

    Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        
        LoockAtTouch();
        pos = Camera.main.WorldToScreenPoint(transform.position);
        Shot();
        
        if (playerHP <= 0)
        {
            playerHP = 100;
            SceneManager.LoadScene(0);
        }

        if (playerSouls == 2000)
        {
            Time.timeScale = 0f;
            basicDamage += 10;
            playerSouls = 0;
            Upgardes.SetActive(true);
        }

        scoreText.text = ("Души: " + playerSouls.ToString());
        HPText.text = ("Здоровье: " + playerHP.ToString());
        
    }

    private void FixedUpdate()
    {
        moveVector.x = joystick.Horizontal;
        moveVector.y = joystick.Vertical;
        rb.MovePosition(rb.position + moveVector * speed * Time.deltaTime);
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        Enemy enemy = new Enemy();
        if (collision.gameObject.tag == "enemy")
        {
            playerHP -= enemy.enemyDamage;
        }

    }

    
    public void Shot()
    {

        if (Input.touchCount == 1 || Input.GetMouseButtonDown(0))
        {
            Touch touch = Input.GetTouch(0);
            Vector3 diference = Camera.main.ScreenToWorldPoint(touch.position) - shotPoint.transform.position;

            RaycastHit2D hit = Physics2D.Raycast(shotPoint.transform.position, diference);
            Debug.DrawRay(shotPoint.transform.position, diference);

            if (touch.phase == TouchPhase.Began)
            {
                switch (hit.collider.tag)
                {
                    case "enemy":
                        {

                            Enemy Enemy = hit.collider.gameObject.GetComponent<Enemy>();

                            if (Enemy.enemyHP > 0)
                            {
                                Enemy.Hitted(Damage());
                            }
                            if (Enemy.enemyHP <= 0)
                            {
                                playerSouls += Enemy.enemySouls;
                                Enemy.HitReact();
                            }

                            break;
                        }
                    case "ground":
                        {
                            Debug.Log("Косой");
                            break;
                        }
                    case "Player":
                        {
                            Debug.Log("Stop Hiting yourself, Stop Hiting yourself, Stop Hiting yourself");
                            break;
                        }
                    case null:
                        {
                            Debug.Log("Вообще никуда");
                            break;
                        }

                    default:

                        Debug.Log("Вообще никуда");

                        break;
                }
            }
        }
    }
    public void LoockAtTouch()
    {
        
        if(Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.tapCount >= 0 && touch.phase == TouchPhase.Began){
                if (touch.position.x > pos.x && isMouseLookLeft)
                {
                    Flip();
                }
                else if (touch.position.x < pos.x && !isMouseLookLeft)
                {
                    Flip();
                }
            }
        }

        
    }
    public void Flip()
    {
        isMouseLookLeft = !isMouseLookLeft;
        Vector3 Scaler = transform.localScale;
        Scaler.x *= -1;
        transform.localScale = Scaler;
    }

    public int Damage()
    {
        damage = UnityEngine.Random.Range(basicDamage - 10, basicDamage);
        return damage;
    }
}
