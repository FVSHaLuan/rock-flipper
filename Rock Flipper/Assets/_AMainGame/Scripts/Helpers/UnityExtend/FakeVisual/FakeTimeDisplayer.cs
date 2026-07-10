using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(UnifiedText))]
public class FakeTimeDisplayer : MonoBehaviour
{
    [SerializeField]
    private UTimeSpan additionTimeSpan;
    [SerializeField]
    private string format = "{0} : {1} : {2} : {3}";

    private UnifiedText unifiedText;

    protected void Update()
    {
        ///
        if (unifiedText == null)
        {
            unifiedText = GetComponent<UnifiedText>();
        }

        ///
        var time = System.DateTime.Now + additionTimeSpan;

        ///
        var text = string.Format(format, time.Hour, time.Minute, time.Second, time.Millisecond);

        ///
        unifiedText.Text = text;
    }
}
