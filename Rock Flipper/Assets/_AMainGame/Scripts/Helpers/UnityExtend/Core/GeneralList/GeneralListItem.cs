using UnityEngine;
using UnityEngine.UI;

public class GeneralListItem : MonoBehaviour
{
    [SerializeField]
    private Image icon;
    [SerializeField]
    private UnifiedText text;

    public Image Icon => icon;

    public UnifiedText Text => text;

}
