using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private List<Button> levelButtons;
    [SerializeField]
    private string[] levelScenes;

    private void Start()
    {
        int compl = Game.CountCompletedLevels();
        for(int i = 0; i < levelButtons.Count; i++)
        {
            int index = i;
            levelButtons[i].interactable = compl >= i;
            if(index < levelScenes.Length)
                levelButtons[i].onClick.AddListener(() => GoToLevel(levelScenes[index]));
        }
    }

    public void GoToLevel(string sceneName)
    {
        LoadingScreen.GoToScene(sceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}

