using OneLine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Selectable)), DisallowMultipleComponent]
public class NavigationOverrider : MonoBehaviour
{
    [SerializeField, OneLineWithHeader]
    private NavigationTarget upTarget;
    [SerializeField, OneLineWithHeader]
    private NavigationTarget downTarget;
    [SerializeField, OneLineWithHeader]
    private NavigationTarget leftTarget;
    [SerializeField, OneLineWithHeader]
    private NavigationTarget rightTarget;

    [System.Serializable]
    public struct NavigationTarget
    {
        public Mode mode;
        public Selectable explicitTarget;

        public Selectable GetSelectable(Selectable main, Vector3 direction)
        {
            if (mode == Mode.Explicit)
            {
                return explicitTarget;
            }

            ///
            return main.FindSelectable(direction);
        }
    }

    public enum Mode
    {
        Explicit = 0,
        Automatic = 1,
    }

    private void UpdateTargets()
    {
        ///
        var selectable = GetComponent<Selectable>();

        ///
        selectable.navigation = new Navigation()
        {
            mode = Navigation.Mode.Explicit,
            wrapAround = false,
            selectOnUp = upTarget.GetSelectable(selectable, Vector3.up),
            selectOnDown = downTarget.GetSelectable(selectable, Vector3.down),
            selectOnLeft = leftTarget.GetSelectable(selectable, Vector3.left),
            selectOnRight = rightTarget.GetSelectable(selectable, Vector3.right),
        };
    }

    protected void OnEnable()
    {
        StartCoroutine(UpdateTargetsDelay());
    }

    private IEnumerator UpdateTargetsDelay()
    {
        yield return new WaitForEndOfFrame();

        ///
        UpdateTargets();
    }
}
