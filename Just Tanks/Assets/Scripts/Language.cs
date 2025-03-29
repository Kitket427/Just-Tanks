using UnityEngine;
using UnityEngine.SceneManagement;
public class Language : MonoBehaviour
{
    private Animator anim;
    [SerializeField] private Menu menu;
    private void Start()
    {
        anim = GetComponent<Animator>();
    }
    public void LanguageSelect(int language)
    {
        PlayerPrefs.SetInt("Language", language);
        if (anim)
        {
            anim.Play("EndGame");
            Invoke(nameof(Menu), 2);
            PlayerPrefs.SetString("Firstry", "Hi");
        }
        else menu.Language();
    }
    void Menu()
    {
        SceneManager.LoadScene(1);
    }
}
