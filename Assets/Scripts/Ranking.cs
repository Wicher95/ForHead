using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Ranking : MonoBehaviour {

    private static List<User> ranking = new List<User>();
    public Text[] userRanking = new Text[8];

    private AudioSource source;
    public AudioClip tomenu;

    // Use this for initialization
    void Start () {
        Screen.orientation = ScreenOrientation.Portrait;
        ReadString();
        ReadUsers();
	}
	
	// Update is called once per frame
	void Update () {
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

    //Wczytywanie listy rankingowej
    void ReadString()
    {
        string path = "data/ranking";
        TextAsset asset = Resources.Load(path) as TextAsset;
        var lines = asset.text.Split("\n"[0]);
        foreach (string line in lines)
        {
            int score;
            var person = line.Split('#');
            if (person.Length > 1)
            {
                User user = new User();
                user.Name = person[0];
                Int32.TryParse(person[1], out score);
                user.Score = score;
                ranking.Add(user);
            }
        }
    }
    void ReadUsers()
    {
        ranking.Sort();
        for(int i=0; i<8;i++)
        {
            userRanking[i].text = ranking[i].Name + " score: " + ranking[i].Score;
        }
    }
}

public class User : IComparable
{
    private string name;
    private int score;

    public string Name
    {
        get
        {
            return name;
        }

        set
        {
            name = value;
        }
    }

    public int Score
    {
        get
        {
            return score;
        }

        set
        {
            score = value;
        }
    }

    public int CompareTo(object obj)
    {
        if (obj == null)
            return 1;

        User usr = obj as User;
        return usr.score - this.score;
    }
}
