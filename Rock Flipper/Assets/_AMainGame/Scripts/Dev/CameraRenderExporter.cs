using UnityEngine;
using System.IO;

[RequireComponent(typeof(Camera))]
public class CameraRenderExporter : MonoBehaviour
{
    [SerializeField]
    private string filePath;

    private Camera cam;

    public Camera Cam
    {
        get
        {
            if (cam == null)
            {
                cam = GetComponent<Camera>();
            }
            return cam;
        }
    }

    public void ExportImage(string filePath)
    {
        RenderTexture rt = Cam.targetTexture;

        if (rt == null)
        {
            Debug.LogError("Camera has no target RenderTexture assigned.");
            return;
        }

        // Save current active RT
        RenderTexture currentRT = RenderTexture.active;

        // Set camera RT active
        RenderTexture.active = rt;

        // Create texture to copy pixels into
        Texture2D tex = new Texture2D(rt.width, rt.height, TextureFormat.RGB24, false);

        // Read pixels
        tex.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
        tex.Apply();

        // Encode to PNG
        byte[] bytes = tex.EncodeToPNG();

        // Write file
        File.WriteAllBytes(filePath, bytes);

        // Cleanup
        RenderTexture.active = currentRT;
        if (Application.isPlaying)
        {
            Destroy(tex);
        }
        else
        {
            DestroyImmediate(tex);
        }

        Debug.Log("Image exported to: " + filePath);
    }

    [ContextMenu("Export")]
    public void ExportImage()
    {
        ExportImage(filePath);
    }
}