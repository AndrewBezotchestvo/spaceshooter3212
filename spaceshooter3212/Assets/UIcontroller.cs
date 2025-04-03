using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIcontroller : MonoBehaviour
{
    [SerializeField] private Slider _hpBar;
    [SerializeField] private GameObject _player;
    [SerializeField] private GameObject _pausePanel;

    private bool _isPause;

    private void Start()
    {
        _isPause = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ChangePause();
        }

        if (_isPause)
        {
            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Time.timeScale = 1f;
            Cursor.lockState = CursorLockMode.Locked;
        }

        Cursor.visible = _isPause;
        _pausePanel.SetActive(_isPause);
    }

    public void ChangePause()
    {
        _isPause = !_isPause;
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
