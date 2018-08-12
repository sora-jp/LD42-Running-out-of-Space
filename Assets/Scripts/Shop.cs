using UnityEngine;

public interface IFuckCSharp {}

[System.Serializable]
public class ShopItemDesc : IFuckCSharp
{
    public string name;
    [Multiline]
    public string desc;
    public string effect;
    public string slug;
    public int cost;
    public bool isVirus;
    public bool takesStorage;
    public bool isOneShot;
    public float virusChance;

    /// <summary>
    /// Make no mistake. why
    /// </summary>
    /// <returns></returns>
    public bool CanBuyItem()
    {
        if (takesStorage)
        {
            return !(PlayerPrefs.GetInt(slug) == 1 && isOneShot) && PersistentData.curStorage >= cost;
        }
        else
        {
            return !(PlayerPrefs.GetInt(slug) == 1 && isOneShot) && PersistentData.curBitcoins >= cost;
        }
    }

    //public ShopItemDesc(Upgrade upgradeType, int cost, string name, string desc, bool isVirus, bool takesStorage, float virusChance, T param, IShopItem<T> item)
    //{
    //    this.upgradeType = upgradeType;
    //    this.cost = cost;
    //    this.name = name;
    //    this.desc = desc;
    //    this.isVirus = isVirus;
    //    this.takesStorage = takesStorage;
    //    this.virusChance = virusChance;
    //    this.item = item;
    //}
}
public class ShopItemDesc<T> : ShopItemDesc where T : struct
{
    public T param;
}


public class ShopItemDesc<T, S> : ShopItemDesc<T> where T : struct where S : IShopItem<T>, new()
{
    public IShopItem<T> item;

    public ShopItemDesc()
    {
        this.item = new S();
    }

    public void Buy()
    {
        if (PlayerPrefs.GetInt(slug) == 1 && isOneShot)
        {
            // TODO: Send some kind of buy error
            GameManager.Instance.DisplayDialog("Error: Item already purchased", KeyCode.Return, (a) => { }, true);
            return;
        }
        item.Buy(param, this);
    }
}

public class ShopItemDelimiter : IFuckCSharp
{
    public string delimit;
    public ShopItemDelimiter(string d)
    {
        delimit = d;
    }
}

public interface IShopItem<T> where T : struct
{
    bool CanBuy(ShopItemDesc<T> desc);

    void Buy(T param, ShopItemDesc<T> desc);
}

public abstract class ShopItem<T> : IShopItem<T> where T : struct
{
    public bool CanBuy(ShopItemDesc<T> desc)
    {
        if (desc.takesStorage)
        {
            return PersistentData.curStorage >= desc.cost;
        }
        else
        {
            return PersistentData.curBitcoins >= desc.cost;
        }
    }

    public bool TryBuy(ShopItemDesc<T> desc)
    {
        if (!desc.CanBuyItem())
        {
            GameManager.Instance.DisplayDialog("Error: Insufficient " + (desc.takesStorage ? "storage" : "funds"), KeyCode.Return, (a) => { }, true);
            return false;
        }
        if (!desc.takesStorage) PersistentData.curBitcoins -= desc.cost;
        else PersistentData.curStorage -= desc.cost;
        if (desc.isOneShot) PlayerPrefs.SetInt(desc.slug, 1);
        PlayerPrefs.Save();
        PersistentData.FlushValues();
        GameManager.Instance.DisplayDialog("Item purchased!", 1, () => { });

        if (desc.isVirus && Random.Range(0f, 1f) < desc.virusChance)
        {
            // Oh u dun fuked boi
            GameManager.Instance.TriggerVirus((ShopItemDesc<float>)(object)desc);
        }

        return true;
    }

    public abstract void Buy(T param, ShopItemDesc<T> desc);
}