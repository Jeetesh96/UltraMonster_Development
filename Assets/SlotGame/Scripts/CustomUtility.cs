using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotGame
{
    public static class CustomUtility
    {
        public static Rect ToRect(this Bounds bounds)
        {
            return new Rect(bounds.min, bounds.size);
        }
        public static Vector2 InverseScale(this Vector2 origin, Vector2 other)
        {
            return new Vector2(origin.x / other.x, origin.y / other.y);
        }

        public static Rect Split(this Rect rect, int splitX, int splitY, int x, int y)
        {
            Vector2 rectSize = InverseScale(rect.size, new Vector2(splitX, splitY));
            Vector2 firstPos = (Vector2)rect.min + Vector2.Scale(rectSize, new Vector2(x, y));
            return new Rect(firstPos, rectSize);
        }
    }
}
