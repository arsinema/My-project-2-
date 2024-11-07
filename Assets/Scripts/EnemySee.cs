using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EnemySee : MonoBehaviour
{
    [SerializeField] float radiysToSeeEnemy = 3.0f;
    [SerializeField] Transform playerTransform;
    [SerializeField] float timeToWait = 2.0f;
    private Color originalColor;
    private SpriteRenderer spriteRenderer;

    public void SeeEnemy()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(playerTransform.position ,radiysToSeeEnemy);

        foreach (Collider2D collider in colliders)
        {
            if(collider.tag == "enemy")
            {
                spriteRenderer = collider.GetComponent<SpriteRenderer>();
                originalColor = spriteRenderer.color;

                spriteRenderer.color = Color.Lerp(originalColor, Color.red, 1f);

                Invoke("RestoreColor", timeToWait);
            }
        }
    }

    private void RestoreColor()
    {
        spriteRenderer.color = originalColor;
    }
}
