using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Lives Section")]
    [SerializeField]
    Sprite[] _livesSprites;
    [SerializeField]
    GameObject _livesContainer, _lowHealthIndicator, _pauseScreen;
    [SerializeField]
    [Tooltip("# of lives left to be considered \"Low Health\"; Also activates the Low Health Indicator at this # ")]
    int _livesForLowHealth;

    int _livesIndex;
    readonly int _livesStages = 2;

    public static UIManager Instance;

    private void Awake()
    {
        Instance = this;
    }



    public void UpdateLostLifeUI(int newLives) 
    {
        _livesIndex = newLives / _livesStages;
        int spriteIndex = newLives % _livesStages;
        Debug.Log("Life Stamp Index: " + _livesIndex + " | sprite index in use: " + spriteIndex);
        _livesContainer.transform.GetChild(_livesIndex).GetComponent<Image>().sprite = _livesSprites[spriteIndex];
        if (newLives <= _livesForLowHealth) 
        {
            _lowHealthIndicator.SetActive(true);
        }
    }

    public void TogglePauseScreen() 
    {
        _pauseScreen.SetActive(!_pauseScreen.activeSelf);
    }



}
