using UnityEngine;
using UnityEngine.SceneManagement;

public class ManageScene : MonoBehaviour
{
    //Reloads the Scene
    public void ReloadScene()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    // Takes you to the Main menu
    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }

    // Starts the game
    public void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }


    //Quits the game
    public void QuitGame()
    {
        Application.Quit();
    }
}
