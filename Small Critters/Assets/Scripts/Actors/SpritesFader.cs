using UnityEngine;
using System.Collections;
using System;

public class SpritesFader : MonoBehaviour {
    private Color newAlphaColor;

    public Trend fadeDirection;
    public float fadeSpeed;
    [Range(0,1)]
    public float minAlpha;
    public int maxFadeCycles = 6;
    public int fadeCyclesElapsed =0;
    SpriteRenderer[] spriteRenderers;
    Color[] originalColors;
    Action OnSequenceFinished;

    void Start () {
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>(true);
        originalColors = new Color[spriteRenderers.Length];
        newAlphaColor = Color.white;
    }

    public void StartFadeSequence(Action callback)
    {
        OnSequenceFinished = callback;
        fadeCyclesElapsed = 0;
        fadeDirection = Trend.Falling;
        SaveCurrentSpriteColors();
        StartCoroutine(FadeSequence());
    }
    private void SaveCurrentSpriteColors()
    {
        for (int i = 0; i < spriteRenderers.Length; ++i)
        {
            originalColors[i] = spriteRenderers[i].color;
        }
    }
    private void ResoreSpriteColors()
    {
        for (int i = 0; i < spriteRenderers.Length; ++i)
        {
            spriteRenderers[i].color = originalColors[i];
        }
    }
    IEnumerator FadeSequence()
    {
        while (fadeCyclesElapsed != maxFadeCycles)
        {
            FadeSprites();
            yield return null;
        }
        FinishSequence();
    }

    private void FinishSequence()
    {
        ResoreSpriteColors();
        OnSequenceFinished();
    }

    private void FadeSprites()
    {
        CheckIfMaxAlphaReached();
        CalculateNewColor();
        SetColorToAllSprites(newAlphaColor);
    }

    private void CheckIfMaxAlphaReached()
    {
        if (fadeDirection == Trend.Rising && newAlphaColor.a >= 1f)
        {
            fadeDirection = Trend.Falling;
            ++fadeCyclesElapsed;
        }
        else if (fadeDirection == Trend.Falling && newAlphaColor.a <= minAlpha)
        {
            fadeDirection = Trend.Rising;
        }
    }

    private void CalculateNewColor()
    {
        newAlphaColor.a += Time.deltaTime * fadeSpeed * (float)fadeDirection;
    }

    private void SetColorToAllSprites(Color color)
    {
        foreach (SpriteRenderer renderer in spriteRenderers)
        {
            renderer.color = color;
        }
    }
}
