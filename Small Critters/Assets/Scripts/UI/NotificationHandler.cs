using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NotificationHandler : MonoBehaviour {
    //public Text[] notifications;
    private Queue<NotificationController> freeNotificationObjects;
    private Queue<NotificationController> activeNotificationObjects;
    //public float shrinkSpeed = 0.1f;
    //public float minScale = 0.1f;
    //public float dropSpeed = 10f;
    public float dropDistance;
    public bool test = false;

	// Use this for initialization
	void Start ()
    {
        var rectTransforms = GetComponentsInChildren<NotificationController>();
        freeNotificationObjects = new Queue<NotificationController>();
        activeNotificationObjects = new Queue<NotificationController>();
        foreach (var rect in rectTransforms)
        {
            freeNotificationObjects.Enqueue(rect);
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (test)
        {
            test = false;
            ShowNotification("TOASTY!!");
        }
	}

    public void ShowNotification(string text)
    {
        NotificationController rect;
        if (freeNotificationObjects.Count > 0)
        {
            rect = freeNotificationObjects.Dequeue();
            activeNotificationObjects.Enqueue(rect);
        }
        else
        {
            rect = activeNotificationObjects.Dequeue();
            rect.Reset();
            activeNotificationObjects.Enqueue(rect);
        }
        rect.StartUp(OnNotificationExpired);
        rect.SetNotificationText(text);
        MoveDownActiveNotifications();
    }

    private void MoveDownActiveNotifications()
    {
        float count = activeNotificationObjects.Count;
        foreach (var notification in activeNotificationObjects)
        {
            notification.MoveDown(count * dropDistance);
            --count;
        }
    }

    private void OnNotificationExpired()
    {
        var rect = activeNotificationObjects.Dequeue();
        rect.Reset();
        freeNotificationObjects.Enqueue(rect);
    }
}
