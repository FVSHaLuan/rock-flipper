using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class LinearObjectArrangement : MonoBehaviour
{
    [SerializeField]
    private Vector3 interval;

    [Space]
    [SerializeField]
    private bool useLocalSpace;

#if UNITY_EDITOR
    [ContextMenu("Editor_Arrange")]
    public void Editor_Arrange()
    {
        ///
        List<Transform> transforms = new List<Transform>();
        for (int i = 0; i < transform.childCount; i++)
        {
            transforms.Add(transform.GetChild(i));
        }

        ///
        if (transforms.Count == 0)
        {
            return;
        }

        ///
        Undo.RecordObjects(transforms.ToArray(), "Editor_Arrange");

        ///
        var currentPos = useLocalSpace ? transforms[0].localPosition : transforms[0].position;
        for (int i = 0; i < transforms.Count; i++)
        {
            ///
            var currentTransform = transforms[i];
            if (useLocalSpace)
            {
                currentTransform.localPosition = currentPos;
            }
            else
            {
                currentTransform.position = currentPos;
            }

            ///
            currentPos += interval;
        }
    }
#endif
}
