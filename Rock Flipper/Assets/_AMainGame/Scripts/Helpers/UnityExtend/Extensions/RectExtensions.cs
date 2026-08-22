using UnityEngine;

public static class RectExtensions
{
    /// <summary>
    /// Scales a source Rect to fit within a destination Rect, maintaining aspect ratio.
    /// </summary>
    /// <param name="source">The Rect to scale.</param>
    /// <param name="destination">The Rect to fit within.</param>
    /// <returns>A new Rect representing the scaled source Rect.</returns>
    public static Rect FitInside(this Rect source, Rect destination)
    {
        float sourceAspect = source.width / source.height;
        float destinationAspect = destination.width / destination.height;

        Rect result = new Rect();

        if (sourceAspect > destinationAspect) // Source is wider
        {
            result.width = destination.width;
            result.height = destination.width / sourceAspect;
            result.x = destination.x;
            result.y = destination.y + (destination.height - result.height) / 2; // Center vertically
        }
        else if (sourceAspect < destinationAspect) // Source is taller
        {
            result.height = destination.height;
            result.width = destination.height * sourceAspect;
            result.y = destination.y;
            result.x = destination.x + (destination.width - result.width) / 2; // Center horizontally
        }
        else // Aspect ratios are equal
        {
            result = destination; // Just use the destination rect
        }

        return result;
    }


    /// <summary>
    /// Scales a source Rect to fit *completely* inside a destination Rect, maintaining aspect ratio.
    /// This version ensures the entire source rect is visible within the destination.
    /// </summary>
    /// <param name="source">The Rect to scale.</param>
    /// <param name="destination">The Rect to fit within.</param>
    /// <returns>A new Rect representing the scaled source Rect.</returns>
    public static Rect FitCompletelyInside(this Rect source, Rect destination)
    {
        float sourceAspect = source.width / source.height;
        float destinationAspect = destination.width / destination.height;

        Rect result = new Rect();

        if (sourceAspect > destinationAspect) // Source is wider
        {
            result.width = destination.width;
            result.height = destination.width / sourceAspect;
            if (result.height > destination.height)
            { //If we have scaled too much, scale down
                result.height = destination.height;
                result.width = destination.height * sourceAspect;
            }
            result.x = destination.x + (destination.width - result.width) / 2; // Center horizontally
            result.y = destination.y + (destination.height - result.height) / 2; // Center vertically

        }
        else if (sourceAspect < destinationAspect) // Source is taller
        {
            result.height = destination.height;
            result.width = destination.height * sourceAspect;
            if (result.width > destination.width)
            { //If we have scaled too much, scale down
                result.width = destination.width;
                result.height = destination.width / sourceAspect;
            }
            result.y = destination.y + (destination.height - result.height) / 2; // Center vertically
            result.x = destination.x + (destination.width - result.width) / 2; // Center horizontally
        }
        else // Aspect ratios are equal
        {
            result = destination; // Just use the destination rect
        }

        return result;
    }

}