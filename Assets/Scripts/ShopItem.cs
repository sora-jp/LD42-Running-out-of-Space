using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ShopItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    MenuManager m;
    Variables v;

    Color desc, bBT;
    Color[] a;

    bool lastState;

    public void Init()
    {
        Debug.Log("Init");
        m = GetComponentInParent<MenuManager>();
        v = GetComponent<Variables>();

        desc = v.desc.color;
        bBT = v.buyButtonText.color;

        a = new Color[v.graphics.Length];
        for (int i = 0; i < a.Length; i++)
        {
            a[i] = v.graphics[i].color;
        }
        UpdateColors(!v.thisItem.CanBuyItem());
    }

    private void Update()
    {
        bool b = !v.thisItem.CanBuyItem();

        if (b != lastState)
        {
            UpdateColors(b);
        }

        lastState = b;
    }

    void UpdateColors(bool error)
    {
        if (error)
        {
            for (int i = 0; i < v.graphics.Length; i++)
            {
                v.graphics[i].color = m.error;
            }
            v.desc.color = m.error;
            v.buyButtonText.color = m.error;
        }
        else
        {
            for (int i = 0; i < v.graphics.Length; i++)
            {
                v.graphics[i].color = a[i];
            }
            v.desc.color = desc;
            v.buyButtonText.color = bBT;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        m.sidePanel.text = v.thisItem.desc + "\n\n" + v.thisItem.cost + (v.thisItem.takesStorage ? "kb" : "btc");
        m.effect.text = v.thisItem.effect;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        m.sidePanel.text = "";
        m.effect.text = "";
    }
}
