using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EnemySee : MonoBehaviour
{
    [SerializeField] float radiysToSeeEnemy = 7.0f;
    [SerializeField] Transform playerTransform;
    [SerializeField] float timeToWait = 2.0f;
    [SerializeField] float timeToWaitAfterUp;
    private Color originalColor;
    private SpriteRenderer spriteRenderer;
    private bool hasUp = true;

    public void SeeEnemy()
    {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(playerTransform.position, radiysToSeeEnemy);

            foreach (Collider2D collider in colliders)
            {
                if (collider.tag == "enemy")
                {
                    spriteRenderer = collider.GetComponent<SpriteRenderer>();
                    originalColor = spriteRenderer.color;

                    spriteRenderer.color = Color.Lerp(originalColor, Color.red, 1f);

                    Invoke("RestoreColor", timeToWait);
                }
            }
    }
    IEnumerator Wait()
    {
        yield return new WaitForSeconds(timeToWaitAfterUp);
    }

    private void RestoreColor()
    {
        spriteRenderer.color = originalColor;
    }
}
