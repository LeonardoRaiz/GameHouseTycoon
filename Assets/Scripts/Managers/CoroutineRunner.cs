using UnityEngine;
using System.Collections;

public class CoroutineRunner : MonoBehaviour
{
    private static CoroutineRunner _instance;

    public static CoroutineRunner Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject runner = new GameObject("CoroutineRunner");
                _instance = runner.AddComponent<CoroutineRunner>();
                DontDestroyOnLoad(runner);
            }
            return _instance;
        }
    }

    public static void Run(IEnumerator coroutine)
    {
        Instance.StartCoroutine(coroutine);
    }
}