using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Intro : MonoBehaviour {

    public Text terminalText;
    public TextAsset boot;

    public GameObject bootPanel;
    public GameObject messageNotifyPanel;
    public GameObject messagePanel;
    public GameObject storagePanel;
    public GameObject flash;
    public Text message;
    public Text response;
    public Text enterToSend;
    public AudioSource hack;
    [Multiline]
    public string helenMsg;
    bool exit;

    string b = @"



			Booting";

    void Start ()
    {
        PersistentData.Load();
        if (PersistentData.hasPlayedTutorial) { SceneManager.LoadScene(1); return; }
        StartCoroutine(Animate());
	}

    IEnumerator Animate()
    {
        yield return new WaitForSeconds(1);

        terminalText.text = b;
        yield return new WaitForSeconds(0.5f);

        for (int i = 0; i < 3; i++)
        {
            terminalText.text += ".";
            yield return new WaitForSeconds(0.5f);
        }
        yield return new WaitForSeconds(0.5f);

        hack.Play();
        string[] original = boot.text.Split('\n');
        List<string> display = new List<string>();
        for (int i = 0; i < original.Length; i++)
        {
            display.Add(original[i]);
            if (display.Count > 40)
            {
                display.RemoveAt(0);
            }
            terminalText.text = string.Join("\n", display.ToArray());
            yield return new WaitForSeconds(0.01f);
        }
        hack.Stop();
        yield return new WaitForSeconds(1f);
        hack.loop = false;
        hack.Play();
        bootPanel.SetActive(true);
        yield return new WaitForSeconds(3f);
        bootPanel.SetActive(false);
        yield return new WaitForSeconds(0.1f);
        hack.Play();
        messageNotifyPanel.SetActive(true);
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Return));
        yield return new WaitForSeconds(0.5f);
        messageNotifyPanel.SetActive(false);
        yield return new WaitForSeconds(0.1f);
        messagePanel.SetActive(true);
        hack.loop = true;

        hack.Play();
        for (int i = 0; i <= helenMsg.Length; i++)
        {
            message.text = helenMsg.Substring(0, i) + "<color=#2e2e2e>" + helenMsg.Substring(i) + "</color>";
            yield return new WaitForSeconds(0.02f);
        }
        hack.Stop();
        yield return new WaitForSeconds(1);
        hack.loop = false;
        string msg = "> ";
        for (int i = 0; i < "oh fuck".Length; i++)
        {
            hack.Play();
            msg += "oh fuck"[i];
            response.text = msg;
            yield return new WaitForSeconds(0.4f);
        }
        enterToSend.gameObject.SetActive(true);
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Return));
        messagePanel.SetActive(false);
        yield return new WaitForSeconds(0.1f);
        storagePanel.SetActive(true);
        var flash = StartCoroutine(Flash());
        hack.Play();
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Return));
        StopCoroutine(flash);
        hack.loop = true;
        FindObjectOfType<GameManager>().tutorial = true;
        SceneManager.LoadScene(2);
    }

    IEnumerator Flash()
    {
        while(true)
        {
            flash.SetActive(!flash.activeSelf);
            yield return new WaitForSeconds(0.5f);
        }
    }
}
