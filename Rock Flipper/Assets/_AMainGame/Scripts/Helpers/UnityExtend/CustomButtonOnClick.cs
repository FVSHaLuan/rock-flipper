using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class CustomButtonOnClick : MonoBehaviour
{
    [SerializeField]
    private bool callAtEnabledFramed = true;

    [SerializeField]
    private UnityEvent onCustomOnClick;

    private int lastEnabledFrame = -1;

    protected void Awake()
    {
        GetComponent<Button>().onClick.AddListener(OnButtonClicked);
    }

    protected void OnEnable()
    {
        lastEnabledFrame = Time.frameCount;
    }

    protected void OnDisable()
    {
        lastEnabledFrame = -1;
    }

    private void OnButtonClicked()
    {
        if (!callAtEnabledFramed)
        {
            ///
            if (lastEnabledFrame < 0)
            {
                return;
            }

            ///
            if (lastEnabledFrame == Time.frameCount)
            {
                return;
            }
        }

        ///
        onCustomOnClick?.Invoke();
    }
}
