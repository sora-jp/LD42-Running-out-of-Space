using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    public static GameManager Instance { get; private set; }
    public DialogBox dialogBox;
    public bool triggerVirus;
    public Canvas virusCanvas;
    public Color error = Color.red;
    public TextAsset virusText;
    [Multiline]
    public string hackingOrig;
    [Multiline]
    public string hackEndText;
    public ImageEffect imgBoi;
    public LayerMask cameraLayers;
    public AudioSource hackThing;
    bool generate;
    bool ambientIsStarted;
    bool canPause;
    public bool tutorial = true;

    private void Awake()
    {
        generate = true;
        tutorial = !PersistentData.hasPlayedTutorial;
        Application.targetFrameRate = 60;
        PersistentData.Load();
        if (Instance != null)
        {
            // O shit we dun fuked bois
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        Instance = this;
        SceneManager.activeSceneChanged += OnLoadScene;
        SoundManager.Instance.ambientSound.clip = SoundManager.Instance.start;
        SoundManager.Instance.ambientSound.Play();
    }

    private void OnLoadScene(Scene prev, Scene cur)
    {
        if (cur.buildIndex == 2 && tutorial) // TODO: Uncomment this shit pls
        {
            // Start tutorial phase 1
            canPause = false;
            StartCoroutine(Tutorial1());
        }
        else if (cur.buildIndex == 1 && tutorial)
        {
            // Start tutorial phase 2
            StartCoroutine(Tutorial2());
        }
    }

    IEnumerator Tutorial1()
    {
        Time.timeScale = 0;
        DisplayDialog("This is the game where you\ngather disk space\n", KeyCode.Return, 200);
        yield return new WaitUntil(() => Input.GetKeyUp(KeyCode.Return));
        yield return null;

        DisplayDialog("You move the ship\nwith the A and D keys\nand shoot with Space\n", KeyCode.Return, 240);
        yield return new WaitUntil(() => Input.GetKeyUp(KeyCode.Return));
        yield return null;

        DisplayDialog("You shoot the files\nand everytime you destroy one\nyou gain kilobytes (kb)\n", KeyCode.Return, 240);
        yield return new WaitUntil(() => Input.GetKeyUp(KeyCode.Return));
        yield return null;

        DisplayDialog("Kilobytes are the\nmain unit of currency\nand are used to download miners\nwhich generate bitcoin\nfor you and your wife\n", KeyCode.Return, 320);
        yield return new WaitUntil(() => Input.GetKeyUp(KeyCode.Return));
        yield return null;

        DisplayDialog("When you feel ready to\nadvance to the next stage\nof the tutorial, press\nesc and then\nthe 'y' key (to confirm)\n", KeyCode.Return, 320);
        yield return new WaitUntil(() => Input.GetKeyUp(KeyCode.Return));
        yield return null;

        DisplayDialog("Now delete some files and\nget familiar with the controls\n", KeyCode.Return, 200);
        yield return new WaitUntil(() => Input.GetKeyUp(KeyCode.Return));
        yield return null;
        Time.timeScale = 1;
        canPause = true;
    }

    IEnumerator Tutorial2()
    {
        var e = EventSystem.current;
        e.enabled = false;

        DisplayDialog("This is the upgrade screen\n", KeyCode.Return, 200);
        yield return new WaitUntil(() => Input.GetKeyUp(KeyCode.Return));
        yield return null;

        DisplayDialog("Here you purchase upgrades\n for your file destroyer, \nand you can make\nthe files stronger too\n(so you can get \nmore kilobytes faster)\n", KeyCode.Return, 320);
        yield return new WaitUntil(() => Input.GetKeyUp(KeyCode.Return));
        yield return null;

        DisplayDialog("You can switch tabs with\nthe buttons under \nthe text up top, and\nyou can scroll\nin the panel below them\nto view the upgrades\n", KeyCode.Return, 320);
        yield return new WaitUntil(() => Input.GetKeyUp(KeyCode.Return));
        yield return null;

        DisplayDialog("You can mouse over upgrades\nto view a description to the\nright and the effect below\n", KeyCode.Return, 240);
        yield return new WaitUntil(() => Input.GetKeyUp(KeyCode.Return));
        yield return null;

        DisplayDialog("Beware! Some of the applications in\nthe applications tab \nhave a small chance\nto install something \nunwanted on you pc\n(we won't tell \nyou which tho :D\ni'ts up to you\nto figure that out)\n", KeyCode.Return, 700);
        yield return new WaitUntil(() => Input.GetKeyUp(KeyCode.Return));
        yield return null;

        DisplayDialog("These applications generate way\nmore bitcoins than a 'safe' app, but this\ncan come at a cost\nThis also only happens randomly\n", KeyCode.Return, 200);
        yield return new WaitUntil(() => Input.GetKeyUp(KeyCode.Return));
        yield return null;

        DisplayDialog("Now go make your wife proud!\n", KeyCode.Return, 200);
        yield return new WaitUntil(() => Input.GetKeyUp(KeyCode.Return));
        yield return null;

        PersistentData.hasPlayedTutorial = true;
        PersistentData.FlushValues();
        e.enabled = true;
        tutorial = false;
    }

    int sc;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F12))
        {
            Debug.Log(Application.persistentDataPath);
            ScreenCapture.CaptureScreenshot(Application.persistentDataPath + "/screenshot_" + sc + ".png", 1);
            sc++;
        }
        if (SoundManager.Instance.ambientSound.time > 3.3f && !ambientIsStarted)
        {
            ambientIsStarted = true;
            SoundManager.Instance.ambientSound.Stop();
            SoundManager.Instance.ambientSound.clip = SoundManager.Instance.ambient;
            SoundManager.Instance.ambientSound.Play();
        }

        if (Input.GetKeyDown(KeyCode.Delete))
        {
            //Fuck your save bitch
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        if (Input.GetKeyDown(KeyCode.Escape) && generate && SceneManager.GetActiveScene().buildIndex == 2 && canPause)
        {
            if (SceneManager.GetActiveScene().buildIndex == 0) return;
            Time.timeScale = 0;
            DisplayDialog("- Exit minigame? -", KeyCode.Y, KeyCode.N, (c) => 
            {
                Debug.Log(c);
                if (c)
                {
                    Time.timeScale = 1;
                    PersistentData.FlushValues();
                    SceneManager.LoadScene(1);
                }
                else
                {
                    Time.timeScale = 1;
                }
            });
        }

        if (generate) PersistentData.curBitcoins += PersistentData.bitcoinPS * Time.deltaTime;
    }

    public void DisplayDialog(string text, KeyCode confirm, KeyCode cancel, System.Action<bool> onClick = null)
    {
        var db = Instantiate(dialogBox);
        db.transform.SetParent(FindObjectOfType<Canvas>().transform, false);

        EventSystem.current.SetSelectedGameObject(db.text.gameObject, null);
        db.text.text = text + "\n" + confirm.ToString().ToLower() + "/" + cancel.ToString().ToLower();
        db.cancel = cancel;
        db.confirm = confirm;
        db.callback = onClick;
    }

    public void DisplayDialog(string text, float timeout, System.Action onClick = null)
    {
        StartCoroutine(DD(text, timeout, onClick));
    }

    IEnumerator DD(string text, float timeout, System.Action onClick)
    {
        var db = Instantiate(dialogBox);
        db.transform.SetParent(FindObjectOfType<Canvas>().transform, false);

        EventSystem.current.SetSelectedGameObject(db.text.gameObject, null);
        db.text.text = text;
        yield return new WaitForSeconds(timeout);
        Destroy(db.gameObject);
    }

    public void DisplayDialog(Canvas c, string text, KeyCode confirm, KeyCode cancel, System.Action<bool> onClick = null, bool err = false)
    {
        var db = Instantiate(dialogBox);
        db.transform.SetParent(c.transform, false);

        db.text.text = text + "\n" + confirm.ToString().ToLower() + "/" + cancel.ToString().ToLower();
        db.cancel = cancel;
        db.confirm = confirm;
        db.callback = onClick;

        EventSystem.current.SetSelectedGameObject(db.text.gameObject, null);

        if (err)
        {
            var v = db.GetComponent<Variables>();
            v.desc.color = error;
            foreach (var i in v.graphics)
            {
                i.color = error;
            }
        }
    }

    public void DisplayDialog(Canvas c, string text, KeyCode confirm, System.Action<bool> onClick = null, bool err = false)
    {
        var db = Instantiate(dialogBox);
        db.transform.SetParent(c.transform, false);

        db.text.text = text + "\nPress " + confirm.ToString().ToLower() + " to close";
        db.confirm = confirm;
        db.callback = onClick;

        if (err)
        {
            var v = db.GetComponent<Variables>();
            v.desc.color = error;
            foreach (var i in v.graphics)
            {
                i.color = error;
            }
        }
    }

    public void DisplayDialog(string text, KeyCode confirm, System.Action<bool> onClick = null, bool err = false)
    {
        var db = Instantiate(dialogBox);
        db.transform.SetParent(FindObjectOfType<Canvas>().transform, false);

        db.text.text = text + "\nPress " + confirm.ToString().ToLower() + " to close";
        db.confirm = confirm;
        db.callback = onClick;

        if (err)
        {
            var v = db.GetComponent<Variables>();
            v.desc.color = error;
            foreach (var i in v.graphics)
            {
                i.color = error;
            }
        }
    }

    public void DisplayDialog(string text, KeyCode confirm, int height, System.Action<bool> onClick = null, bool err = false)
    {
        var db = Instantiate(dialogBox);
        db.transform.SetParent(FindObjectOfType<Canvas>().transform, false);
        var a = db.GetComponent<RectTransform>().sizeDelta;
        a.y = height;
        db.GetComponent<RectTransform>().sizeDelta = a;

        db.text.text = text + "\nPress " + confirm.ToString().ToLower() + " to close";
        db.confirm = confirm;
        db.callback = onClick;

        if (err)
        {
            var v = db.GetComponent<Variables>();
            v.desc.color = error;
            foreach (var i in v.graphics)
            {
                i.color = error;
            }
        }
    }

    public void TriggerVirus(ShopItemDesc<float> desc, bool bypassWait = false)
    {
        Debug.Log("Triggered");
        StartCoroutine(Virus((int)PersistentData.curBitcoins, desc != null ? desc.param : 0, bypassWait));
    }

    IEnumerator Virus(int prevBitcoins, float itemGen, bool bypassWait = false)
    {
        if (!bypassWait) yield return new WaitForSeconds(Random.Range(20, 30));
        generate = false;
        var s = FindObjectOfType<Spawner>();
        if (s != null) s.enabled = false;
        int cost = (int)PersistentData.curBitcoins - prevBitcoins + prevBitcoins / 5;
        PersistentData.bitcoinPS -= itemGen;
        PersistentData.FlushValues();

        var img = Camera.main.GetComponent<ImageEffect>();
        Application.targetFrameRate = 10;
        img.strength = 5;
        img.enabled = true;
        SoundManager.Instance.PlaySound(Sound.Hack2);
        yield return new WaitForSeconds(4.240f);
        Camera.main.cullingMask = cameraLayers;
        FindObjectOfType<Canvas>().gameObject.SetActive(false);
        Canvas c = Instantiate(virusCanvas);
        c.worldCamera = Camera.main;

        VirusStuff v = c.GetComponent<VirusStuff>();
        img.strength = 0f;//img.enabled = false;

        yield return new WaitForSeconds(3);
        Application.targetFrameRate = 60;

        string[] original = virusText.text.Split('\n');
        List<string> display = new List<string>();
        hackThing.volume = PersistentData.sfxVolume;
        hackThing.Play();
        for(int i = 0; i < Mathf.Min(original.Length, 600); i++) 
        {
            display.Add(original[i]);
            if (display.Count > 40)
            {
                display.RemoveAt(0);
            }
            v.exceptions.text = string.Join("\n", display.ToArray());
            yield return new WaitForSeconds(0.002f);
        }
        hackThing.Stop();
        yield return new WaitForSeconds(0.5f);
        v.exceptions.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        img.strength = 4;// img.enabled = true;
        v.skull.gameObject.SetActive(true);

        SoundManager.Instance.PlaySound(Sound.Hack1);
        float t = 1;
        while (t > 0)
        {
            t -= Time.deltaTime / 2.2f;
            img.strength = Mathf.Pow(t, 2)*4;
            yield return null;
        }

        img.strength = 0f;// img.enabled = false;
        v.hackingGo.SetActive(true);
        yield return new WaitForSeconds(0.5f);

        hackThing.loop = false;
        display.Clear();
        original = hackingOrig.Split('\n');
        original[original.Length - 1] = "";
        for (int i = 0; i < original.Length-1; i++)
        {
            if (original[i].Contains("rand"))
            {
                hackThing.loop = true;
                img.enabled = true;
                img.strength = 3f;
                hackThing.Play();
                for (int j = 0; j < 32; j++)
                {
                    display.Add(string.Format("0x{0:X05} 0x{0:X05} 0x{0:X05} 0x{0:X05}", Random.Range(0, 1048576), Random.Range(0, 1048576), Random.Range(0, 1048576), Random.Range(0, 1048576)));
                    if (display.Count > 14)
                    {
                        display.RemoveAt(0);
                    }
                    v.hacking.text = string.Join("\n", display.ToArray());
                    yield return new WaitForSeconds(0.05f);
                }
                hackThing.Stop();
                hackThing.loop = false;
                img.strength = 0f;// img.enabled = false;
                continue;
            }
            display.Add(original[i]);
            if (display.Count > 14)
            {
                display.RemoveAt(0);
            }
            hackThing.Play();
            v.hacking.text = string.Join("\n", display.ToArray());
            yield return new WaitForSeconds(1.5f);
        }
        hackThing.loop = true;

        DisplayHackDialog(c, cost);
    }

    void DisplayHackDialog(Canvas c, int cost)
    {
        DisplayDialog(c, string.Format(hackEndText, cost), KeyCode.Y, KeyCode.N, (confirm) =>
        {
            if (confirm)
            {
                PersistentData.curBitcoins -= cost;
                PersistentData.FlushValues();
                DisplayDialog(c, "Your money has been recieved\nYour files have now been decrypted", KeyCode.Return, (a) => { generate = true; SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); }, true);
            }
            else
            {
                DisplayDialog(c, "This will delete all of your progress\nAre you sure?", KeyCode.Y, KeyCode.N, (a) => { if (a) { generate = true; PlayerPrefs.DeleteAll(); PlayerPrefs.Save(); SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); } else DisplayHackDialog(c, cost); }, true);
            }
        }, true);
    }
}

public static class PersistentData
{
    public static int curStorage;
    public static float curBitcoins;

    public static int storagePS;
    public static float bitcoinPS;

    public static float playerMoveSpeed;
    public static float playerFireRate;
    public static bool playerUseRapidFire;
    public static bool playerUseBoost;

    public static float spawnsPerSec;
    public static float fallSpeed;
    public static float size;
    public static int health;
    public static int damage;

    public static float sfxVolume;
    public static float ambVolume;

    public static bool hasPlayedTutorial;

    public static void FlushValues()
    {
        PlayerPrefs.SetInt("curStorage", curStorage);
        PlayerPrefs.SetInt("curBitcoins", (int)curBitcoins);

        PlayerPrefs.SetInt("storagePS", storagePS);
        PlayerPrefs.SetFloat("bitcoinPS", bitcoinPS);

        PlayerPrefs.SetFloat("playerMoveSpeed", playerMoveSpeed);
        PlayerPrefs.SetFloat("playerFireRate", playerFireRate);

        PlayerPrefs.SetInt("playerUseRapidFire", playerUseRapidFire ? 1 : 0);
        PlayerPrefs.SetInt("playerUseBoost", playerUseBoost ? 1 : 0);

        PlayerPrefs.SetFloat("spawnsPerSec", spawnsPerSec);
        PlayerPrefs.SetFloat("fallSpeed", fallSpeed);
        PlayerPrefs.SetFloat("size", size);
        PlayerPrefs.SetInt("health", health);
        PlayerPrefs.SetInt("damage", damage);

        PlayerPrefs.SetFloat("sfxVolume", sfxVolume);
        PlayerPrefs.SetFloat("ambVolume", ambVolume);

        PlayerPrefs.SetInt("hasPlayedTutorial", hasPlayedTutorial ? 1 : 0);

        PlayerPrefs.Save();
    }

    public static void Load()
    {
        curStorage = PlayerPrefs.GetInt("curStorage", 0);
        curBitcoins = PlayerPrefs.GetInt("curBitcoins", 0);

        storagePS = PlayerPrefs.GetInt("storagePS", 0);
        bitcoinPS = PlayerPrefs.GetFloat("bitcoinPS", 0);

        playerMoveSpeed = PlayerPrefs.GetFloat("playerMoveSpeed", 8f);
        playerFireRate = PlayerPrefs.GetFloat("playerFireRate", 5);

        playerUseRapidFire = PlayerPrefs.GetInt("playerUseRapidFire", 0) == 1;
        playerUseBoost = PlayerPrefs.GetInt("playerUseBoost", 0) == 1;

        spawnsPerSec = PlayerPrefs.GetFloat("spawnsPerSec", 1.5f);
        fallSpeed = PlayerPrefs.GetFloat("fallSpeed", 1.5f);
        size = PlayerPrefs.GetFloat("size", 1f);
        health = PlayerPrefs.GetInt("health", 1);
        damage = PlayerPrefs.GetInt("damage", 1);

        sfxVolume = PlayerPrefs.GetFloat("sfxVolume", 1f);
        ambVolume = PlayerPrefs.GetFloat("ambVolume", 1f);

        hasPlayedTutorial = PlayerPrefs.GetInt("hasPlayedTutorial", 0) == 1;
    }
}
