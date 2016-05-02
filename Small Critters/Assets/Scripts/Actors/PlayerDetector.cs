using UnityEngine;
using System.Collections;

public class PlayerDetector : MonoBehaviour {

    IPlayerDetection myController;

    void Awake()
    {
        myController = GetComponentInParent<IPlayerDetection>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            myController.PlayerDetected(other.gameObject);
        }
    }
}
