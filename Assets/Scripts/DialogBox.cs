using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogBox : MonoBehaviour {
    public KeyCode confirm;
    public KeyCode cancel;
    public Text text;
    public System.Action<bool> callback;

    public void Update()
    {
        if (Input.GetKeyUp(confirm))
        {
            Destroy(gameObject);
            if (callback != null)
                callback(true);
        }
        if (Input.GetKeyUp(cancel))
        {
            Destroy(gameObject);
            if (callback != null)
                callback(false);
        }
    }
}
