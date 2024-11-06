using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Camera_Switch_Color : MonoBehaviour
{
    private Camera _camera;
    [SerializeField] float time = 10f;
    [SerializeField] float transitionTime = 0.1f;

    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI HPText;
    [SerializeField] GameObject[] walls;

    private bool _enabled = true;

    private void Start()
    {
        _camera = GetComponent<Camera>();
        StartCoroutine(ColorSwitchWhite(time));
    }

    

    IEnumerator ColorSwitchWhite(float time)
    {
        while (true)
        {
            yield return new WaitForSeconds(time);
            Color targetColor = _enabled ? Color.white : Color.black;
            Color nonTargetColor = !_enabled ? Color.white : Color.black;
            float elapsedTime = 0f;

            while (elapsedTime < transitionTime)
            {
                _camera.backgroundColor = Color.Lerp(_camera.backgroundColor, targetColor, elapsedTime / transitionTime);
                foreach (GameObject wall in walls)
                {
                    SpriteRenderer spriteRenderer = wall.GetComponent<SpriteRenderer>();

                    spriteRenderer.color = Color.Lerp(_camera.backgroundColor, nonTargetColor, elapsedTime / transitionTime);
                }

                elapsedTime += Time.deltaTime;

                yield return null;
                
            }
            _camera.backgroundColor = targetColor;
            _enabled = !_enabled;
            
        }
    }
    
}
