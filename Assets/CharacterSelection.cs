using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelection : MonoBehaviour
{
    public ControllerScript controller;
    public GameObject canvas;
    public GameObject colorSelection;
    public GameObject[] character;
    public InputField playerName_IF;


    public Button prevCharacter, nextCharacter;
    public Button start;

    [SerializeField]


    public PlayerSelection ps = new PlayerSelection();

    


    void Start()
    {

        ps.name= PlayerPrefs.GetString("USER_NAME");
        playerName_IF.text = ps.name;

        ps.character = (byte)PlayerPrefs.GetInt("USER_CHARACTER");
        ps.r = (byte)PlayerPrefs.GetInt("USER_COLOR_R");
        ps.g = (byte)PlayerPrefs.GetInt("USER_COLOR_G");
        ps.b = (byte)PlayerPrefs.GetInt("USER_COLOR_B");
        

        //selectedColor = colorSelection.transform.GetChild(0).GetChild(0).GetComponent<Image>().color;

        prevCharacter.GetComponent<Button>().onClick.AddListener(() => {ps.character = (byte)(((int)character.Length + (int)ps.character-1) % character.Length);});
        nextCharacter.GetComponent<Button>().onClick.AddListener(() => {ps.character = (byte)(((int)ps.character+1) % character.Length);});

        for(int row = 0; row < colorSelection.transform.childCount; row++)
        {
            for(int col = 0; col < colorSelection.transform.GetChild(row).childCount; col++)
            {
                GameObject button = colorSelection.transform.GetChild(row).GetChild(col).gameObject;
                Color c = button.GetComponent<Image>().color;

                button.name = ((int)(255 * c.r)).ToString("X2") + ((int)(255 * c.g)).ToString("X2") + ((int)(255 * c.b)).ToString("X2");
                button.GetComponent<Button>().onClick.AddListener(() => 
                {
                    Debug.Log(c);

                    ps.r = (byte)(c.r * 255);
                    ps.g = (byte)(c.g * 255);
                    ps.b = (byte)(c.b * 255);

                });
            }
        }

        playerName_IF.onValueChanged.AddListener((name)=> 
        {
            ps.name = name; 
            PlayerPrefs.SetString("USER_NAME", ps.name);

        });

        start.onClick.AddListener(() => 
        {
            PlayerPrefs.SetString("USER_NAME", ps.name);
            PlayerPrefs.SetInt("USER_CHARACTER", ps.character);
            PlayerPrefs.SetInt("USER_COLOR_R", ps.r);
            PlayerPrefs.SetInt("USER_COLOR_G", ps.g);
            PlayerPrefs.SetInt("USER_COLOR_B", ps.b);

            ps.id = SystemInfo.deviceUniqueIdentifier;

            if(controller.SendUserSelection(ps))
                canvas.SetActive(false);
        });
        
    }
    void Update()
    {
        for(int i = 0 ; i < character.Length; i++)
        {
            character[i].gameObject.SetActive(i == ps.character);
            character[i].GetComponent<Image>().color = new Color((float)ps.r/ 255f,(float)ps.g/ 255f,(float)ps.b/ 255f);
        }
    }
}
