using UnityEngine;

public class GeneralGizmos : MonoBehaviour
{
#if UNITY_EDITOR
    [SerializeField]
    private Color gizmoColor = Color.red;
    [SerializeField]
    private DrawMode drawMode = DrawMode.Always;

    protected enum DrawMode
    {
        Always,
        Selected,
        ChildSelected
    }

    protected virtual void DrawGizmosSpecific()
    {
        // This method can be overridden in derived classes to draw specific gizmos.
    }

    protected void OnDrawGizmos()
    {
        ///
        bool shouldDraw = false;

        ///
        if (drawMode == DrawMode.Always)
        {
            shouldDraw = true;
        }
        if (drawMode == DrawMode.ChildSelected)
        {
            var selectedGameObject = UnityEditor.Selection.activeGameObject;
            if (selectedGameObject != null && (selectedGameObject == gameObject || selectedGameObject.transform.IsChildOf(transform)))
            {
                shouldDraw = true;
            }
        }

        ///
        if (shouldDraw)
        {
            DrawGizmos();
        }
    }

    protected void OnDrawGizmosSelected()
    {
        if (drawMode == DrawMode.Selected)
        {
            DrawGizmos();
        }
    }

    private void DrawGizmos()
    {
        ///
        if (!enabled)
        {
            return;
        }

        ///
        Gizmos.color = gizmoColor;
        DrawGizmosSpecific();
    }
#endif
}
