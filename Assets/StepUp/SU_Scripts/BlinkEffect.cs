using System.Collections;
using UnityEngine;

public class BlinkEffect : MonoBehaviour
{
    public float closeDuration = 0.1f; // How long it takes to close the eyes
    public float openDuration = 0.1f; // How long it takes to open the eyes
    public float closedTime = 1.0f; // How long the eyes remain closed
    public float openedTime = 1.0f; // How long the eyes remain opened

    private SpriteRenderer spriteRenderer;
    private float timer;
    private bool isClosed;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        timer = openedTime;
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            if (isClosed)
            {
                // Time to open the eyes
                StartCoroutine(ChangeAlpha(0f, 1f, openDuration));
                timer = openedTime;
            }
            else
            {
                // Time to close the eyes
                StartCoroutine(ChangeAlpha(1f, 0f, closeDuration));
                timer = closedTime;
            }

            isClosed = !isClosed;
        }
    }

    IEnumerator ChangeAlpha(float startAlpha, float endAlpha, float duration)
    {
        float time = 0;
        while (time < duration)
        {
            float alpha = Mathf.Lerp(startAlpha, endAlpha, time / duration);
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, alpha);
            time += Time.deltaTime;
            yield return null;
        }

        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, endAlpha);
    }
}

