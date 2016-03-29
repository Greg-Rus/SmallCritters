using UnityEngine;
using System.Collections;

public class FloorHandler : MonoBehaviour {
	public float floorShiftOffset;
	public MeshRenderer leftWallMeshRenderer;
	public MeshRenderer rightWallMeshRenderer;
	public MeshRenderer floorMeshRenderer;
	public int wallOrderInLayer;
	public int floorOderInLayer;
	public Texture2D floorTiles;
	private Material floorMaterial;
	
	void Start()
	{
		leftWallMeshRenderer.sortingOrder = wallOrderInLayer;
		rightWallMeshRenderer.sortingOrder = wallOrderInLayer;
		floorMeshRenderer.sortingOrder = floorOderInLayer;
		floorMaterial = floorMeshRenderer.material;
		floorMaterial.mainTexture = RandomizeFloorTexture();
	}
	
	void OnTriggerEnter2D(Collider2D other) {
		Vector3 newPosition = this.transform.position;
		newPosition.y = newPosition.y + floorShiftOffset;
		this.transform.position = newPosition;
	}
	
	private Texture2D RandomizeFloorTexture()
	{
		Texture2D newTexture = new Texture2D(7*128,36*128);
		for(int i = 0; i< 7; ++i)
		{
			for (int j = 0; j < 36; ++j)
			{
				newTexture.SetPixels32(i*128,j*128,128,128,GetTile());
				
				//Debug.Log (i+"  " +j);
			}
		}
		newTexture.Apply(true);
		return newTexture;
	}
	
	private Color32[] GetTile()
	{
		int tileOffsetX = Random.Range(0,3) * 128;
		int tileOffsetY = Random.Range(0,3) * 128;
//		Debug.Log (tileOffsetX+"  "+tileOffsetY);
		Color[] tile = floorTiles.GetPixels(tileOffsetX,tileOffsetY,128,128);
		Color32[] tile32 = new Color32[tile.Length];
		int i =0;
		foreach(Color color in tile)
		{
			tile32[i++] = (Color32)color;
		}
		return tile32;
	}
}
