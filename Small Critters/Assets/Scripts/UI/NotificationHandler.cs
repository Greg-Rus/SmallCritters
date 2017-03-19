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
    public bool multiKill = false;
    private int killCount = 0;
    public float multiKillTimeFrame = 0.5f;
    public string[] multiKillNotices;
    private IAudio myAudio;

	// Use this for initialization
	void Start ()
    {
        myAudio = ServiceLocator.getService<IAudio>();
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
        if (text == "shot")
        {
            if (!multiKill)
            {
                multiKill = true;
                StartCoroutine("MultiKilltimer");
            }
            else
            {
                StopCoroutine("MultiKilltimer");
                StartCoroutine("MultiKilltimer");
                
            }
            ++killCount;
            if(killCount >= multiKillNotices.Length) text = multiKillNotices[multiKillNotices.Length - 1];
            else text = multiKillNotices[killCount - 1];

        }
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
        myAudio.PlaySound(Sound.Notification);
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

    IEnumerator MultiKilltimer()
    {
        yield return new WaitForSeconds(multiKillTimeFrame);
        multiKill = false;
        killCount = 0;
    }
}
