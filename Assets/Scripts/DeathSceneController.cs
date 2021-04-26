using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class DeathSceneController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _scoreText;
    public void OnClick()
    {
        SceneManager.LoadScene("MenuScene");
    }

    void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        _scoreText.text= "Your depth was: " + Mathf.Floor(DepthManager.Singleton.Depth) + " m";
    }
}
