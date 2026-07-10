using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using Agame.Meta;

[RequireComponent(typeof(TextMeshProUGUI))]
public class TextMeshProLinkHandler : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private Camera uiCamera;

    private TextMeshProUGUI _textMeshPro;

    void Awake()
    {
        _textMeshPro = GetComponent<TextMeshProUGUI>();
        if (uiCamera == null)
        {
            uiCamera = _textMeshPro.canvas.worldCamera;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        ///
        if (uiCamera == null)
        {
            Debug.LogError("uiCamera can not be null", this);
            return;
        }

        // Convert click position to world point and get link index
        int linkIndex = TMP_TextUtilities.FindIntersectingLink(_textMeshPro, eventData.position, uiCamera);

        if (linkIndex != -1)
        {
            TMP_LinkInfo linkInfo = _textMeshPro.textInfo.linkInfo[linkIndex];
            string linkId = linkInfo.GetLinkID();

            HandleLinkClick(linkId);
        }
    }

    private void HandleLinkClick(string linkId)
    {
        // Example behavior: open a URL if it's a valid one
        if (linkId.StartsWith("http"))
        {
            MetaUrlLauncher.TryOpenUrlWithSteam(linkId);
        }
        else if (linkId.StartsWith("mailto"))
        {
            Application.OpenURL(linkId);
        }
        else
        {
            Debug.Log($"Unhandled link ID: {linkId}");
        }
    }
}
