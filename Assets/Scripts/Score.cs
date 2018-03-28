using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
using System;

public class Score : MonoBehaviour
{

    public AudioClip tomenu;
    public AudioClip end;
    public Text scor;
    public Text[] answers = new Text[10];
    public Text motiv;
    private AudioSource source;
    int poprawne;
    private static List<string> ranking = new List<string>();

    // Use this for initialization
    void Start()
    {
        poprawne = 0;
        source = GetComponent<AudioSource>();
        source.PlayOneShot(end, 1f);
        Screen.orientation = ScreenOrientation.Portrait;
        wyn();
        scor.text = "Wynik: 0/10";
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToMenu("Menu");
        }
    }
    public void ToMenu(string scene)
    {
        source.PlayOneShot(tomenu);
        {
            scene = "Menu";
            SceneManager.LoadScene(scene);
        }
    }

    void wyn()
    {
        if (Game.odpowiedzi.Count == 0)
        {
            updateRanking();
        }
        else
        {
            StartCoroutine("wynik");
        }
    }

    IEnumerator wynik()
    {
        for (int i = 0, j = 0; i < 20 - (Game.questionLeft * 2); i = i + 2, j++)
        {
            answers[j].text = Game.odpowiedzi[i];
            if (Game.odpowiedzi[i + 1] == "wrong")
            {
                answers[j].color = Color.red;
            }
            else
            {
                answers[j].color = Color.green;
                poprawne++;
            }
            scor.text = "Wynik: " + poprawne + "/10";
            yield return new WaitForSeconds(0.5f);
        }
        if (Game.score < 5)
        {
            motiv.text = "Następnym razem pójdzie Ci lepiej :)";
        }
        else if (Game.score < 7)
        {
            motiv.text = "Całkiem nieźle!";
        }
        else if (Game.score < 10)
        {
            motiv.text = "Gratulacje, jesteś naprawdę dobra/y!";
        }
        else if (Game.score <= 11)
        {
            motiv.text = "Niesamowite! Prawdziwie jesteś geniuszem!";
        }
        updateRanking();
    }
    void updateRanking()
    {
        string path = "data/ranking";
        string score = poprawne.ToString();
        string user = "Ola";
        int rank = 0;
        TextAsset asset = Resources.Load(path) as TextAsset;
        var lines = asset.text.Split("\n"[0]);
        foreach (string line in lines)
        {
            var person = line.Split('#');
            if (person.Length > 1)
            {
                Int32.TryParse(person[1], out rank);
                if (person[0] == user)
                {
                    rank += poprawne;
                    rank += 100;
                }
                string x = person[0] + "#" + rank.ToString();
                ranking.Add(x);
            }
        }
        File.WriteAllLines(Application.dataPath+"/Resources/"+path+".txt", ranking.ToArray());
    }

}
