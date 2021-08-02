using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    float timer, refresh, avgFramerate;
    string display = "{0} FPS";
    [SerializeField]
    Text m_text;


    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        DisplayFramerate();
    }
    public void StartPrototype()
    {
        Time.timeScale = 1f;
    }
    private void DisplayFramerate()
    {
        float timelapse = Time.smoothDeltaTime;
        timer = timer <= 0 ? refresh : timer -= timelapse;

        if (timer <= 0) avgFramerate = (int)(1f / timelapse);
        m_text.text = string.Format(display, avgFramerate.ToString());
    }
}
