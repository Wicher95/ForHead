using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Game : MonoBehaviour {

    public Text instruction;
    public Text timer;
    public Text passtext;
    public Text autor;

    public Transform tlo;
    public Transform Passed;
    public Transform Gooded;
    public Transform czolo;

    public AudioClip correct;
    public AudioClip wrong;
    public AudioClip count;

    private AudioSource source;
    public static List<string> odpowiedzi = new List<string>();
    private static List<string> pytanie = new List<string>();
    public static int score;

    bool active;
    bool ready;
    bool start;
    bool time;
    bool countReady;
    private float countdown;
    private float countint;
    private float czas;
    private float timeLeft;
    private float leftint;
    private float timeLeftCategory;
    private int numerPytania;
    public static int questionLeft;

    static private float sizeOflist;

    void Start () {
        czolo.gameObject.SetActive(true);
        Passed.gameObject.SetActive(false);
        Gooded.gameObject.SetActive(false);
        source = GetComponent<AudioSource>();
        ready = false;      
        active = false;
        time = false;
        countReady = true;
        start = true;
        score = 0;
        czas = 6;        
        countdown = 4f;
        questionLeft = 10;

        //Obracanie ekranu tylko w poziomie
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        Screen.orientation = ScreenOrientation.AutoRotation;
        Screen.autorotateToLandscapeLeft = true;
        Screen.autorotateToLandscapeRight = true;
        Screen.autorotateToPortrait = false;
        Screen.autorotateToPortraitUpsideDown = false;

        //Wczytanie listy pytań
        ReadString();
        //Wylosowanie pierwszego pytania
        numerPytania = (int)Random.Range(0, sizeOflist);
        autor.text = "";
        if (LevelManager.category == "zanuc")
        {
            instruction.text = "Zanuć";
            timeLeft = 61f;
            timeLeftCategory = 30f;
        }
        else if (LevelManager.category == "opowiadaj")
        {
            instruction.text = "Opowiadaj";
            timeLeft = 31f;
            if (LevelName.level == "btnPrzyslowia")
            {
                timeLeft = 61f;
                timeLeftCategory = 30f;
            }
            else
            {
                timeLeftCategory = 0f;
            }
        }
        else if (LevelManager.category == "kalambury")
        {
            instruction.text = "Pokazuj";
            timeLeft = 181f;
            timeLeftCategory = 150f;
        }

    }
	
	void FixedUpdate () {
        czas -= Time.deltaTime;
        //ready = true;
        //czolo.gameObject.SetActive(false);

        //Rozpocznij odliczanie
        if (start == true && (Input.deviceOrientation == DeviceOrientation.LandscapeLeft || Input.deviceOrientation == DeviceOrientation.LandscapeRight))
        {
            czolo.gameObject.SetActive(false);
            tlo.gameObject.SetActive(true);
            source.PlayOneShot(count, 1f);
            ready = true;
            start = false;
        }
        //Przyłóż telefon do cioła
        if (Input.deviceOrientation == DeviceOrientation.LandscapeLeft || Input.deviceOrientation == DeviceOrientation.LandscapeRight)
        {
            ready = true;
            if (time == true)
            {
                czas = 2.5f;
                time = false;
            }            
        }
        //Odliczanie ostatnich 3 sekund
        if (timeLeft < 4 && countReady == true)
        {
            countReady = false;
            source.PlayOneShot(count, 1f);
        }
        //Anulowanie rozgrywki
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            string scene = "Score";
            SceneManager.LoadScene(scene);
        }
        //Odpowiadanie na pytanie
        if (questionLeft > 0 && active == false && ready == true)
        {
            tlo.gameObject.SetActive(true);
            //Pierwsze odliczanie
            if (countdown > 1)
            {
                countdown -= Time.deltaTime;
                countint = (int)countdown;
                timer.text = countint.ToString();
            }
            // Wypisywanie hasła na ekranie
            else if (timeLeft > 1)          
            {
                timeLeft -= Time.deltaTime;
                //Haslo dla kategorii zanuć - osobno tytuł osobno autor
                if (LevelManager.category == "zanuc")
                {
                    char split = '"';
                    string[] substrings = pytanie[numerPytania].Split(split);
                    instruction.text = substrings[1];
                    autor.text = substrings[2];
                }
                //Hasło dla pozostałych kategorii
                else
                {
                    instruction.text = pytanie[numerPytania];
                }
                //Odliczanie timera
                leftint = (int)timeLeft;
                timer.text = leftint.ToString();
            }     
            //Jesli pozostalyczas < 1 to czas minął
            else
            {
                passtext.text = "Czas minął";
                Pass();       
            }
            //Zatwierdzenie poprawnej odpowiedzi
            if (Input.deviceOrientation == DeviceOrientation.FaceUp)
            {
                Good();
            }            
        }
        // Koniec pytań
        else if (questionLeft < 1 && active == false)
        {
            string scene = "Score";
            SceneManager.LoadScene(scene);
        }

    }

    //Funkcja od błędnej odpowiedzi
    public void Pass ()
    {
        countReady = true;
        if (czas < 0 && countdown<1)
        {
            odpowiedzi.Add(pytanie[numerPytania]);
            odpowiedzi.Add("wrong");
            active = true;
            if (countdown < 1)
            {
                source.PlayOneShot(wrong,0.5f);
                StartCoroutine("Zle");                
            }
            time = true;
        }        
    }

    IEnumerator Zle()
    {
        tlo.gameObject.SetActive(false);
        Passed.gameObject.SetActive(true);
        timeLeft = 31f + timeLeftCategory;
        pytanie.RemoveAt(numerPytania);
        sizeOflist = pytanie.Count;
        numerPytania = (int)Random.Range(0, sizeOflist);
        questionLeft -= 1;        
        yield return new WaitForSeconds(1);
        ready = false;
        Passed.gameObject.SetActive(false);
        passtext.text = "Pasuję";
        active = false;
    }

    //Funkcja od poprawnej odpowiedzi
    public void Good ()
    {
        countReady = true;
        if (czas < 0 && countdown < 1)
        {
			odpowiedzi.Add(pytanie[numerPytania]);         
            odpowiedzi.Add("correct");
            active = true;
            if (countdown < 1)
            {
                source.PlayOneShot(correct,0.5f);
                StartCoroutine("Dobrze");
            }
            time = true;
        }        
    }
    IEnumerator Dobrze()
    {
        tlo.gameObject.SetActive(false);
        Gooded.gameObject.SetActive(true);
        timeLeft = 31f + timeLeftCategory;
        pytanie.RemoveAt(numerPytania);
        sizeOflist = pytanie.Count;
        numerPytania = (int)Random.Range(0, sizeOflist);
        questionLeft -= 1;        
        score++;
        yield return new WaitForSeconds(1);
        ready = false;
        Gooded.gameObject.SetActive(false);
        active = false;

    }

    //Wczytywanie listy haseł
    static void ReadString()
    {
        odpowiedzi.Clear();
        pytanie.Clear();
        string path = "data/";
        path = path + LevelName.level;
        TextAsset asset = Resources.Load(path) as TextAsset;
        var lines = asset.text.Split("\n"[0]);
        foreach (string line in lines)
        {
            pytanie.Add(line);
        }
        sizeOflist = pytanie.Count;
    }

    

}
