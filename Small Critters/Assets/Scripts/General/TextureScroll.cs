using UnityEngine;
using System.Collections;

public class TextureScroll : MonoBehaviour {
    public float scrollSpeed = 3;
    public float direction = 1;
    public Renderer myRenderer;
    public Vector2 offset = Vector2.zero;

	void Start () {
        myRenderer.sortingLayerName = "Frog";
        myRenderer.sortingOrder = 100;

    }
	
	void Update () {
        offset.x = Time.time * scrollSpeed * direction;
        myRenderer.material.SetTextureOffset("_MainTex", offset);
        if (offset.x >= 1) offset.x = 0;
	}
}
