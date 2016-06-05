using UnityEngine;
using System.Collections;
using System;

public class SpritesFader : MonoBehaviour {
    public Color newAlphaColor;
    public float fadeDirection;
    public float fadeSpeed;
    [Range(0,1)]
    public float minAlpha;
    public int maxFadeCycles = 6;
    public int fadeCyclesElapsed =0;
    SpriteRenderer[] spriteRenderers;
    Action OnSequenceFinished;
    // Use this for initialization
    void Start () {
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>(true);
        newAlphaColor = new Color(1f, 1f, 1f, 1f);
        //StartCoroutine(FadeSequence());
    }
	


    public void StartFadeSequence(Action callback)
    {
        OnSequenceFinished = callback;
        fadeCyclesElapsed = 0;
        fadeDirection = -1f;
        SaveCurrentSpriteColors();
        StartCoroutine(FadeSequence());
    }
    private void SaveCurrentSpriteColors()
    {
        //Save colors;
    }
    private void ResoreSpriteColors()
    {
        //restore colors;
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
        SetColorToAllSprites(new Color(1f,1f,1f,1f)); //TODO: remove this. Use RestoreSpriteColors();
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
        if (fadeDirection > 0 && newAlphaColor.a >= 1f)
        {
            fadeDirection = -fadeDirection;
            ++fadeCyclesElapsed;
        }
        else if (fadeDirection < 0 && newAlphaColor.a <= minAlpha)
        {
            fadeDirection *= fadeDirection;
            
        }
    }


    private void CalculateNewColor()
    {
        newAlphaColor.a += Time.deltaTime * fadeSpeed * fadeDirection;
    }

    private void SetColorToAllSprites(Color color)
    {
        foreach (SpriteRenderer renderer in spriteRenderers)
        {
            renderer.color = color;
        }
    }
}
