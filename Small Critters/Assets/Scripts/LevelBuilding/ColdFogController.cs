using UnityEngine;
using System.Collections;

public class ColdFogController : MonoBehaviour
{
    public float speed;
    Vector3 newPosition;
    public GameObject frog;
    public float baseSpeed;
    public float speedDivisor;
    public LevelData levelData;

    // Use this for initialization

    // Update is called once per frame
    void Update()
    {
        ModifySpeedBasedOnDistanceToFrog();
        MoveUp();
    }
    void ModifySpeedBasedOnDistanceToFrog()
    {
        float distanceToFrog = (frog.transform.position - this.transform.position).magnitude;
        speed = distanceToFrog / speedDivisor + baseSpeed;
    }

    void MoveUp()
    {
        if (this.transform.position.y < levelData.levelTop - levelData.levelLength)
        {
            newPosition = this.transform.position;
            newPosition.y = levelData.levelTop - levelData.levelLength;
            //Debug.Log("Skipping rows to catch up to player");
        }
        else
        {
            newPosition = this.transform.position + Vector3.up * speed * Time.deltaTime;
        }
        this.transform.position = newPosition;
    }

}
