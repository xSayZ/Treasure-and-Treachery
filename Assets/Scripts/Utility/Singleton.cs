using System;
using UnityEngine;

/// <summary>
/// Be aware this will not prevent a non singleton constructor
///   such as `T myT = new T();`
/// To prevent that, add `protected T () {}` to your singleton class.
/// </summary>
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    /// <summary>
    ///     <c>true</c> if this Singleton Awake() method has already been called by Unity; otherwise, <c>false</c>.
    /// </summary>
    public static bool IsAwakened { get; private set; }

    private static object _lock = new object();
    
    public static T Instance
    {
        get
        {
            if (applicationIsQuitting)
            {
                Debug.LogWarning("[Singleton] Instance '" + typeof(T) +
                    "' already destroyed on application quit." +
                    " Won't create again - returning null.");
                return null;
            }

            lock (_lock)
            {
                if (_instance == null)
                {
                    _instance = (T)FindObjectOfType(typeof(T));

                    if (FindObjectsOfType(typeof(T)).Length > 1)
                    {
                        Debug.LogError("[Singleton] Something went really wrong " +
                            " - there should never be more than 1 singleton!" +
                            " Reopening the scene might fix it.");
                        return _instance;
                    }

                    if (_instance == null)
                    {
                        GameObject singleton = new GameObject();
                        _instance = singleton.AddComponent<T>();
                        singleton.name = "(singleton) " + typeof(T).ToString();

                        Debug.Log("[Singleton] An instance of " + typeof(T) +
                            " is needed in the scene, so '" + singleton +
                            "' was created.");
                    }
                    else
                    {
                        //Debug.Log("[Singleton] Using instance already created: " + _instance.gameObject.name);
                    }
                }

                return _instance;
            }
        }
    }
    
    private static T _instance;
    
    private void Awake() {
        if (!IsAwakened) {
            Debug.Log($"Awake() Singleton with type {this.GetType()} and name {this.gameObject.name}");

            SingletonAwakened();
            IsAwakened = true;
        }
    }
    
    /// <summary>
    ///     Unity3D Awake method.
    /// </summary>
    /// <remarks>
    ///     This method will only be called once even if multiple instances of the
    ///     singleton MonoBehaviour exist in the scene.
    ///     You can override this method in derived classes to customize the initialization of your MonoBehaviour
    /// </remarks>
    protected virtual void SingletonAwakened() {}

    private static bool IsDontDestroyOnLoad()
    {
        if (_instance == null)
        {
            return false;
        }
        // Object exists independent of Scene lifecycle, assume that means it has DontDestroyOnLoad set
        if ((_instance.gameObject.hideFlags & HideFlags.DontSave) == HideFlags.DontSave)
        {
            return true;
        }
        return false;
    }

    private static bool applicationIsQuitting = false;
    /// <summary>
    /// When Unity quits, it destroys objects in a random order.
    /// In principle, a Singleton is only destroyed when application quits.
    /// If any script calls Instance after it have been destroyed, 
    ///   it will create a buggy ghost object that will stay on the Editor scene
    ///   even after stopping playing the Application. Really bad!
    /// So, this was made to be sure we're not creating that buggy ghost object.
    /// </summary>
    public void OnDestroy()
    {
        if (IsDontDestroyOnLoad())
        {
            applicationIsQuitting = true;
        }
    }
}