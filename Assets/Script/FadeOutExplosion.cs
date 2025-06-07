using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeOutExplosion : MonoBehaviour
{
    public float fadeDuration = 0.5f;
    private SpriteRenderer spriteRenderer;
    private float startAlpha;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        startAlpha = spriteRenderer.color.a;
        StartCoroutine(FadeOut());
    }

    System.Collections.IEnumerator FadeOut()
    {
        float timer = 0f;
        while (timer < fadeDuration)
        {
            float alpha = Mathf.Lerp(startAlpha, 0f, timer / fadeDuration);
            Color c = spriteRenderer.color;
            c.a = alpha;
            spriteRenderer.color = c;
            timer += Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);
    }
}
