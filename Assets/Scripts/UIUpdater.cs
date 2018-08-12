using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIUpdater : MonoBehaviour {
    public Text stats;

	void Update ()
    {
        stats.text = "--Storage : " + PersistentData.curStorage + "kb--" + "\n--Bitcoins : " + (int)PersistentData.curBitcoins + "btc--";
	}
}
