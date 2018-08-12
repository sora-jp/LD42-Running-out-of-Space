[System.Serializable]
public class MovementShopDesc : ShopItemDesc<float, MovementShop> { }
public class MovementShop : ShopItem<float>
{
    public override void Buy(float param, ShopItemDesc<float> desc)
    {
        if (!TryBuy(desc)) return;
        PersistentData.playerMoveSpeed += desc.param;
        PersistentData.FlushValues();
    }
}

[System.Serializable]
public class FireShopDesc : ShopItemDesc<float, FireShop> { }
public class FireShop : ShopItem<float>
{
    public override void Buy(float param, ShopItemDesc<float> desc)
    {
        if (!TryBuy(desc)) return;
        PersistentData.playerFireRate += desc.param;
        PersistentData.FlushValues();
    }
}

[System.Serializable]
public class FireUpgradeShopDesc : ShopItemDesc<float, FireUpgradeShop> { }
public class FireUpgradeShop : ShopItem<float>
{
    public override void Buy(float param, ShopItemDesc<float> desc)
    {
        if (!TryBuy(desc)) return;
        PersistentData.playerUseRapidFire = true;
        PersistentData.FlushValues();
    }
}

[System.Serializable]
public class BoostUpgradeShopDesc : ShopItemDesc<float, BoostShop> { }
public class BoostShop : ShopItem<float>
{
    public override void Buy(float param, ShopItemDesc<float> desc)
    {
        if (!TryBuy(desc)) return;
        PersistentData.playerUseBoost = true;
        PersistentData.FlushValues();
    }
}

[System.Serializable]
public class MiningShopDesc : ShopItemDesc<float, MiningShop> { }
public class MiningShop : ShopItem<float>
{
    public override void Buy(float param, ShopItemDesc<float> desc)
    {
        if (!TryBuy(desc)) return;
        PersistentData.bitcoinPS += desc.param;
        PersistentData.FlushValues();
    }
}

[System.Serializable]
public class EnemySpeedShopDesc : ShopItemDesc<float, EnemySpeedShop> { }
public class EnemySpeedShop : ShopItem<float>
{
    public override void Buy(float param, ShopItemDesc<float> desc)
    {
        if (!TryBuy(desc)) return;
        PersistentData.fallSpeed += desc.param;
        PersistentData.FlushValues();
    }
}

[System.Serializable]
public class SpawnRateShopDesc : ShopItemDesc<float, SpawnRateShop> { }
public class SpawnRateShop : ShopItem<float>
{
    public override void Buy(float param, ShopItemDesc<float> desc)
    {
        if (!TryBuy(desc)) return;
        PersistentData.spawnsPerSec += desc.param;
        PersistentData.FlushValues();
    }
}

[System.Serializable]
public class SizeShopDesc : ShopItemDesc<float, SizeShop> { }
public class SizeShop : ShopItem<float>
{
    public override void Buy(float param, ShopItemDesc<float> desc)
    {
        if (!TryBuy(desc)) return;
        PersistentData.size += desc.param;
        PersistentData.FlushValues();
    }
}

[System.Serializable]
public class EnemyHealthShopDesc : ShopItemDesc<int, EnemyHealthShop> { }
public class EnemyHealthShop : ShopItem<int>
{
    public override void Buy(int param, ShopItemDesc<int> desc)
    {
        if (!TryBuy(desc)) return;
        PersistentData.health += desc.param;
        PersistentData.FlushValues();
    }
}

[System.Serializable]
public class PlayerDamageShopDesc : ShopItemDesc<int, PlayerDamageShop> { }
public class PlayerDamageShop : ShopItem<int>
{
    public override void Buy(int param, ShopItemDesc<int> desc)
    {
        if (!TryBuy(desc)) return;
        PersistentData.damage += desc.param;
        PersistentData.FlushValues();
    }
}