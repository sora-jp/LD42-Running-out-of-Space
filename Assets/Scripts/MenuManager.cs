using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour {

    public Text stats;
    public Text sidePanel;
    public Text effect;

    [Space]
    [Space]
    [Space]
    public string fireDelimit = "--- Firing ---";
    public string moveDelimit = "--- Movement ---";
    public string miningDelimit = "--- Applications ---";
    public string speedDelimit = "--- Enemy Speed ---";
    public string sizeDelimit = "---Enemy Spawnrate---";
    public string spawnDelimit = "--- Enemy Size ---";
    public string healthDelimit = "--- Enemy Health ---";
    public string damageDelimit = "--- Bullet Damage ---";
    [Space]
    public BoostUpgradeShopDesc boostUpgradeDesc;
    public List<MovementShopDesc> moveItems;
    [Space]
    public FireUpgradeShopDesc fireUpgradeShopDesc;
    public List<FireShopDesc> fireItems;
    [Space]
    public List<EnemySpeedShopDesc> speedItems;
    [Space]
    public List<SpawnRateShopDesc> spawnItems;
    [Space]
    public List<SizeShopDesc> sizeItems;
    [Space]
    public List<EnemyHealthShopDesc> healthItems;
    [Space]
    public List<PlayerDamageShopDesc> damageItems;
    [Space]
    public List<MiningShopDesc> miningItems;
    [Space]
    [Space]
    [Space]
    public Transform upgradeHolder;
    public Transform appHolder;
    public Transform diffHolder;

    public GameObject upgrades;
    public GameObject apps;
    public GameObject difficulty;

    public Button upgradeBtn;
    public Button appsBtn;
    public Button diffBtn;

    public Color normal;
    public Color selected;
    public Color error;

    public Variables purchaseThing;
    public Transform delimeter;

    private void Start()
    {
        PersistentData.Load();

        SpawnShit(appHolder, miningDelimit, true, miningItems.ToArray());

        SpawnShit(upgradeHolder, moveDelimit, false, boostUpgradeDesc);
        SpawnShit(upgradeHolder, true, moveItems.ToArray());

        SpawnShit(upgradeHolder, fireDelimit, false, fireUpgradeShopDesc);
        SpawnShit(upgradeHolder, true, fireItems.ToArray());

        SpawnShit(upgradeHolder, damageDelimit, false, damageItems.ToArray());

        SpawnShit(diffHolder, speedDelimit, true, speedItems.ToArray());
        SpawnShit(diffHolder, spawnDelimit, true, spawnItems.ToArray());
        SpawnShit(diffHolder, sizeDelimit, true, sizeItems.ToArray());
        SpawnShit(diffHolder, healthDelimit, true, healthItems.ToArray());
    }

    public void ShowUpgrades()
    {
        upgrades.SetActive(true);
        apps.SetActive(false);
        difficulty.SetActive(false);

        var c = upgradeBtn.colors;
        c.normalColor = selected;
        upgradeBtn.colors = c;

        c = appsBtn.colors;
        c.normalColor = normal;
        appsBtn.colors = c;

        c = diffBtn.colors;
        c.normalColor = normal;
        diffBtn.colors = c;
    }

    public void ShowApps()
    {
        upgrades.SetActive(false);
        apps.SetActive(true);
        difficulty.SetActive(false);

        var c = upgradeBtn.colors;
        c.normalColor = normal;
        upgradeBtn.colors = c;

        c = appsBtn.colors;
        c.normalColor = selected;
        appsBtn.colors = c;

        c = diffBtn.colors;
        c.normalColor = normal;
        diffBtn.colors = c;
    }

    public void ShowDifficulty()
    {
        upgrades.SetActive(false);
        apps.SetActive(false);
        difficulty.SetActive(true);

        var c = upgradeBtn.colors;
        c.normalColor = normal;
        upgradeBtn.colors = c;

        c = appsBtn.colors;
        c.normalColor = normal;
        appsBtn.colors = c;

        c = diffBtn.colors;
        c.normalColor = selected;
        diffBtn.colors = c;
    }

    void SpawnShit<T, S>(List<IFuckCSharp> completeList, Transform parent) where T : struct where S : IShopItem<T>, new()
    {
        for (int i = 0; i < completeList.Count; i++)
        {
            var fu = completeList[i];
            if (fu is ShopItemDelimiter)
            {
                Transform fuck = Instantiate(delimeter);
                fuck.SetParent(parent, false);
                fuck.GetComponent<Text>().text = ((ShopItemDelimiter)fu).delimit;
            }
            else
            {
                var c = (ShopItemDesc<T, S>)fu;
                Variables fuckYou = Instantiate(purchaseThing);
                fuckYou.transform.SetParent(parent, false);
                fuckYou.buyButtonText.text = "Buy\n" + c.cost + (c.takesStorage ? "kb" : "btc");
                fuckYou.desc.text = c.name;
                fuckYou.thisItem = c;
                fuckYou.buy.onClick.AddListener(() => { c.Buy(); });
                fuckYou.GetComponent<ShopItem>().Init();
            }
        }
    }

    void SpawnShit<T, S>(Transform parent, bool order, params ShopItemDesc<T, S>[] inp) where T : struct where S : IShopItem<T>, new()
    {
        List<IFuckCSharp> completeList;
        if (order) completeList = (from a in inp orderby a.cost select (IFuckCSharp)a).ToList();
        else completeList = (from a in inp select (IFuckCSharp)a).ToList();
        SpawnShit<T, S>(completeList, parent);
    }

    void SpawnShit<T, S>(Transform parent, string delimit, bool order, params ShopItemDesc<T, S>[] inp) where T:struct where S:IShopItem<T>, new()
    {
        List<IFuckCSharp> completeList;
        if (order) completeList = (from a in inp orderby a.cost select (IFuckCSharp)a).ToList();
        else completeList = (from a in inp select (IFuckCSharp)a).ToList();
        completeList.Insert(0, new ShopItemDelimiter(delimit));
        SpawnShit<T, S>(completeList, parent);
    }

    private void Update()
    {
        stats.text = "--Info--\nStorage: " + PersistentData.curStorage + "kb\nBitcoins: " + (int)PersistentData.curBitcoins + "btc";
    }

    public void LoadMinigame()
    {
        // TODO: More robust pls
        PersistentData.FlushValues();
        SceneManager.LoadScene(2);
    }
}