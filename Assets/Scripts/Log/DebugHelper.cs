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
        
        public static void LogController(string message, string controller)
        {
            Debug.Log($"<color=#f3FF30>[DEBUG{controller.ToUpper()}]</color> {message}");
        }

        public static void WarnController(string message, string controller)
        {
            Debug.LogWarning($"<color=#FFA500>[WARNING{controller.ToUpper()}]</color> {message}");
        }

        public static void ErrorController(string message, string controller)
        {
            Debug.LogError($"<color=#FF15AD>[ERROR{controller.ToUpper()}]</color> {message}");
        }
#endif
    }
}
