using UnityEngine;
using System.Collections;

public class TextureScroll : MonoBehaviour {
    public float scrollSpeed = 3;
    public float direction = 1;
    public Renderer myRenderer;
    public Vector2 offset = Vector2.zero;
	// Use this for initialization
	void Start () {
        myRenderer.sortingLayerName = "Frog";
        myRenderer.sortingOrder = 100;

    }
	
	// Update is called once per frame
	void Update () {
        offset.x = Time.time * scrollSpeed * direction;
        myRenderer.material.SetTextureOffset("_MainTex", offset);
        if (offset.x >= 1) offset.x = 0;
	}
}
