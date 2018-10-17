using UnityEngine;

public class BackButton : MonoBehaviour
{
    void Start()
    {
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            Destroy(GetComponent<BackButton>());
        }
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (GetComponent<Menus>().newGameMenu.activeInHierarchy)
            {
                GetComponent<Menus>().NewGameMenuClose();
            }
            else if (GetComponent<Menus>().moreMenu.activeInHierarchy)
            {
                GetComponent<Menus>().MoreMenuClose();
            }
            else if (GetComponent<Menus>().howToPlayMenu.activeInHierarchy)
            {
                GetComponent<Menus>().HowToPlayClose();
            }
            else if (GetComponent<Menus>().settingsMenu.activeInHierarchy)
            {
                GetComponent<Menus>().SettingsClose();
            }
            else if (GetComponent<Menus>().statsMenu.activeInHierarchy)
            {
                GetComponent<Menus>().StatsClose();
            }
            else
            {
                GetComponent<BoardCreator>().Undo();
            }
        }
    }
}
