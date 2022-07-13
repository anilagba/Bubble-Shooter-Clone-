using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIElements : MonoBehaviour
{
    [Header("Music")]
    [SerializeField] AudioSource audioSource;
    [SerializeField] Button toggleMusicPlayButton;
    [SerializeField] Sprite musicOnSprite;
    [SerializeField] Sprite musicOffSprite;

    [Header("Menu Buttons")]
    [SerializeField] Button playButton;
    [SerializeField] Button restartButton;
    [SerializeField] Button ExitButton;

    [Header("In Game Buttons")]
    [SerializeField] Button changeColorButton;

    BallPocket ballPocket;

    private void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            toggleMusicPlayButton.onClick.AddListener(PlayStopMusic);
            playButton.onClick.AddListener(Play);
            ExitButton.onClick.AddListener(ExitGame);

            DontDestroyOnLoad(audioSource);
        }
        else
        {
            restartButton.onClick.AddListener(Restart);
            ExitButton.onClick.AddListener(ExitGame);
            changeColorButton.onClick.AddListener(ChangeColor);

            ballPocket = FindObjectOfType<BallPocket>();
        }
    }

    void ChangeColor()
    {
        ballPocket.ChangeBalls();
    }

    void PlayStopMusic()
    {
        if (audioSource.isPlaying)
        {
            toggleMusicPlayButton.image.sprite = musicOffSprite;
            audioSource.Stop();
        }
        else
        {
            toggleMusicPlayButton.image.sprite = musicOnSprite;
            audioSource.Play();
        }
    }

    void Play()
    {
        SceneManager.LoadScene(1);
    }

    void Restart()
    {
        SceneManager.LoadScene(0);
    }

    void ExitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#endif
    }
}
