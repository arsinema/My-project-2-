using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EnemySee : MonoBehaviour
{
    [SerializeField] float radiysToSeeEnemy = 3.0f;
    [SerializeField] Transform playerTransform;
    [SerializeField] float timeToWait = 2.0f;

    public void SeeEnemy()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(playerTransform.position ,radiysToSeeEnemy);
        SpriteRenderer spriteRenderer;

        foreach (Collider2D collider in colliders)
        {
            if(collider.tag == "enemy")
            {
                spriteRenderer = collider.GetComponent<SpriteRenderer>();
                Color originalColor = spriteRenderer.color;

                spriteRenderer.color = Color.Lerp(originalColor, Color.red, 0.1f);

                StartCoroutine(TimeToWait());
                
                RestoreColor(spriteRenderer, originalColor);
            }
        }
    }

    private void RestoreColor(SpriteRenderer spriteRenderer, Color originalColor)
    {
        spriteRenderer.color = originalColor;
    }

    IEnumerator TimeToWait()
    {
        yield return new WaitForSeconds(timeToWait);
    }
}
