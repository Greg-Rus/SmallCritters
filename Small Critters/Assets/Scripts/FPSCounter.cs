using UnityEngine;
using System.Collections;
using System.Text;
using UnityEngine.UI;

public class FPSCounter : MonoBehaviour {

    float deltaTime = 0.0f;
    public Text fpsText;
    private StringBuilder fpsTextBuilder;

    void Awake()
    {
        fpsTextBuilder = new StringBuilder();
    }

    void Update()
    {
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
        float msec = deltaTime * 1000.0f;
        float fps = 1.0f / deltaTime;
        //fpsText.text = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);
        fpsTextBuilder.Remove(0, fpsTextBuilder.Length);
        fpsTextBuilder.AppendFormat("{0:0.0} ms ({1:0.} fps)", msec, fps);
        fpsText.text = fpsTextBuilder.ToString();
    }
}
