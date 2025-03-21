using UnityEngine;

namespace Log
{
    public static class DebugHelper 
    {
#if UNITY_EDITOR
        public static void Log(string message)
        {
            Debug.Log($"<color=#00FF00>[DEBUG]</color> {message}");
        }

        public static void Warn(string message)
        {
            Debug.LogWarning($"<color=#FFA500>[WARNING]</color> {message}");
        }

        public static void Error(string message)
        {
            Debug.LogError($"<color=#FF0000>[ERROR]</color>{message}");
        }
#endif
    }
}
