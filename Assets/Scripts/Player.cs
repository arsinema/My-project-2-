using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [Header("Player preference")]
    [SerializeField] public float speed = 0f;
    [SerializeField] public int damage;
    [SerializeField] public static int playerHP = 100;
    [SerializeField] float jumpForce = 1f;
    [SerializeField] bool isGrounded = true;
    public static int playerSouls = 0;
    [SerializeField] bool isSprinting = false;
    [SerializeField] float sprintSpeed = 10;
    [SerializeField] bool isMouseLookLeft = false;
    [SerializeField] bool isStay = true;
    public static bool enemyHit = false;
    [SerializeField] public float normalSpeed = 2;
    [SerializeField] public static int vampire = 0;

    [Header("Player damage")]
    [SerializeField] public int basicDamage = 40;


    [Header("Player shooting setings")]
    [SerializeField] GameObject shotPoint;


    [Header("Player UI")]
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI HPText;
    [SerializeField] GameObject canvasPrefab;

    float Hor = 0f;
    Vector2 pos;

    Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        speed = 0f;
        
    }

    private void Update()
    {
        LoockAtTouch();
        pos = Camera.main.WorldToScreenPoint(transform.position);
        Shot();
        isSprinting = Sprinting();
        Move();

        if (playerHP <= 0)
        {
            playerHP = 100;
            SceneManager.LoadScene(0);
        }

        if (playerSouls == 2000)
        {
            basicDamage += 10;
            playerSouls = 0;
            Instantiate(canvasPrefab);
        }

        scoreText.text = ("Души: " + playerSouls.ToString());
        HPText.text = ("Здоровье: " + playerHP.ToString());
    }

    public void Jump()
    {
        if (isGrounded)
        {
            rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            isGrounded = false;
            isStay = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "ground")
        {
            isGrounded = true;
            isStay = true;
        }

        if (collision.gameObject.tag == "enemy")
        {
            playerHP -= Enemy.enemyDamage;
        }

    }

    public void Move()
    {
        transform.Translate((speed * Time.deltaTime), 0, 0);
    }

    public bool Sprinting()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void Shot()
    {

        if (Input.touchCount == 1 || Input.GetMouseButtonDown(0))
        {
            Touch touch = Input.GetTouch(0);
            Vector3 diference = Camera.main.ScreenToWorldPoint(touch.position) - shotPoint.transform.position;

            RaycastHit2D hit = Physics2D.Raycast(shotPoint.transform.position, diference);

            if (touch.phase == TouchPhase.Began)
            {
                switch (hit.collider.tag)
                {
                    case "enemy":
                        {

                            Enemy Enemy = hit.collider.gameObject.GetComponent<Enemy>();

                            if (Enemy.enemyHP > 0)
                            {
                                Enemy.Hitted(Damage(isStay));
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
    public void Flip()
    {
        isMouseLookLeft = !isMouseLookLeft;
        Vector3 Scaler = transform.localScale;
        Scaler.x *= -1;
        transform.localScale = Scaler;
    }

    public int Damage(bool Stay)
    {
        if (Stay)
        {
            damage = UnityEngine.Random.Range(basicDamage - 10, basicDamage);
            return damage;
        }
        if (!Stay)
        {
            damage = UnityEngine.Random.Range(basicDamage - 20, basicDamage - 10);
            return damage;
        }
        else { return damage; }
    }

    public void OnRightButtonDown()
    {
        if(speed >= 0) 
        { 
            speed += normalSpeed;
        }
        
    }
    public void OnLeftButtonDown()
    {
        if (speed <= 0)
        {
            speed -= normalSpeed;
        }
    }
    public void OnButtonUp()
    {
        speed = 0f;
    }
}
