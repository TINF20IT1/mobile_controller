using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelection : MonoBehaviour
{
    public GameObject colorSelection;
    public GameObject[] character;
    public InputField playerName_IF;

    public Color selectedColor;

    public Button prevCharacter, nextCharacter;
    public Button start;
    public string playerName;

    public int selectedCharacter = 0;
    void Start()
    {

        playerName_IF.text = PlayerPrefs.GetString("USER_NAME");
        selectedCharacter = PlayerPrefs.GetInt("USER_CHARACTER");
        selectedColor = new Color(PlayerPrefs.GetFloat("USER_COLOR_R"),PlayerPrefs.GetFloat("USER_COLOR_G"),PlayerPrefs.GetFloat("USER_COLOR_B"));


        selectedColor = colorSelection.transform.GetChild(0).GetChild(0).GetComponent<Image>().color;

        prevCharacter.GetComponent<Button>().onClick.AddListener(() => {selectedCharacter = (character.Length + selectedCharacter-1) % character.Length;});
        nextCharacter.GetComponent<Button>().onClick.AddListener(() => {selectedCharacter = (selectedCharacter+1) % character.Length;});

        for(int row = 0; row < colorSelection.transform.childCount; row++)
        {
            for(int col = 0; col < colorSelection.transform.GetChild(row).childCount; col++)
            {
                GameObject button = colorSelection.transform.GetChild(row).GetChild(col).gameObject;
                Color c = button.GetComponent<Image>().color;

                button.name = ((int)(255 * c.r)).ToString("X2") + ((int)(255 * c.g)).ToString("X2") + ((int)(255 * c.b)).ToString("X2");
                button.GetComponent<Button>().onClick.AddListener(() => {selectedColor = c;});
            }
        }

        playerName_IF.onValueChanged.AddListener((name)=> {playerName = name;});
        start.onClick.AddListener(() => {
            PlayerPrefs.SetString("USER_NAME", playerName);
            PlayerPrefs.SetInt("USER_CHARACTER", selectedCharacter);
            PlayerPrefs.SetFloat("USER_COLOR_R", selectedColor.r);
            PlayerPrefs.SetFloat("USER_COLOR_G", selectedColor.g);
            PlayerPrefs.SetFloat("USER_COLOR_B", selectedColor.b);


            
        });
        
    }
    void Update()
    {

        for(int i = 0 ; i < character.Length; i++)
        {
            character[i].gameObject.SetActive(i == selectedCharacter);
            character[i].GetComponent<Image>().color = selectedColor;
        }

        
    }
}
