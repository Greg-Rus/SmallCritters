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

    void Update()
    {
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
        }
        else
        {
            newPosition = this.transform.position + Vector3.up * speed * Time.deltaTime;
        }
        this.transform.position = newPosition;
    }

}
