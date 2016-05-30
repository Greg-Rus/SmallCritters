using UnityEngine;
using System.Collections;

public class CostumeSwitcher : MonoBehaviour {
    public Sprite[] costumeSprites;
    public Sprite[] originalSprites;
    public SpriteRenderer[] bodyParts;
    public GameObject helmet;
    public GameObject gun;
    // Use this for initialization

    public void PutOnCostume()
    {
        SwapSprites(costumeSprites);
        helmet.SetActive(true);
        gun.SetActive(true);
    }

    public void TakeOffCostume()
    {
        SwapSprites(originalSprites);
        helmet.SetActive(false);
        gun.SetActive(false);
    }

    private void SwapSprites(Sprite[] sprites)
    {
        for (int i = 0; i < bodyParts.Length; ++i)
        {
            bodyParts[i].sprite = sprites[i];
        }
    }
}
