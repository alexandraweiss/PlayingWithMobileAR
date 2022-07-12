using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class DebugOutput : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI debugText;

    public static DebugOutput instance;

    private bool showingDebugText = false;
    private const float debugTextTimer = 3f;
    private float debugTextStartTime = 0f;

    void Start()
    {
        if (instance == null) 
        {
            instance = this;
        }
    }

    void Update()
    {
        if (showingDebugText && Time.time - debugTextStartTime > debugTextTimer) 
        {
            resetDebugText();
        }
    }

    public void ShowMessage(string message) 
    {
        if (debugText != null)
        {
            debugText.text = message;
        }
        showingDebugText = true;
        debugTextStartTime = Time.time;
    }

    private void resetDebugText()
    {
        if (debugText != null)
        {
            debugText.text = "";
        }
        showingDebugText = false;
        debugTextStartTime = 0f;
    }
}
