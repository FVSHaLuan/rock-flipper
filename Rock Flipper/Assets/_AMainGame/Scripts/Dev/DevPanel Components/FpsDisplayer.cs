using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FpsDisplayer : MonoBehaviour
{
    [SerializeField]
    private UnifiedText text;

    // Attach this to any object to make a frames/second indicator.
    //
    // It calculates frames/second over each updateInterval,
    // so the display does not keep changing wildly.
    //
    // It is also fairly accurate at very low FPS counts (<10).
    // We do this not by simply counting frames per interval, but
    // by accumulating FPS for each frame. This way we end up with
    // corstartRect overall FPS even if the interval renders something like
    // 5.5 frames.

    public float frequency = 0.5F; // The update frequency of the fps
    public int nbDecimal = 1; // How many decimal do you want to display

    private float accum = 0f; // FPS accumulated over the interval
    private int frames = 0; // Frames drawn over the interval
    private Color color = Color.white; // The color of the GUI, depending of the FPS ( R < 10, Y < 30, G >= 30 )  
    private GUIStyle style; // The style the text will be displayed at, based en defaultSkin.label.

    private string formatString;

    void Start()
    {
        formatString = "f" + Mathf.Clamp(nbDecimal, 0, 10);
    }

    protected void OnDisable()
    {
        StopAllCoroutines();
    }

    protected void OnEnable()
    {
        ///
        StartCoroutine(FPS());
    }

    void Update()
    {
        accum += 1 / Time.unscaledDeltaTime;
        ++frames;
    }

    IEnumerator FPS()
    {
        // Infinite loop executed every "frenquency" secondes.
        while (true)
        {
            // Update the FPS
            float fps = accum / frames;
            text.Text = fps.ToString(formatString);

            //Update the color
            color = (fps >= 30) ? Color.green : ((fps > 10) ? Color.red : Color.yellow);

            accum = 0.0F;
            frames = 0;

            yield return new WaitForSecondsRealtime(frequency);
        }
    }
}
