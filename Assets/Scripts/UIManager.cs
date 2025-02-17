using System;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Lives Section")]
    [SerializeField]
    Sprite[] _lostLivesSprites, _gainedLivesSprites;
    [SerializeField]
    GameObject _livesContainer, _lowHealthIndicator, _pauseScreen;
    [SerializeField]
    [Tooltip("# of lives left to be considered \"Low Health\"; Also activates the Low Health Indicator at this # ")]
    int _livesForLowHealth;

    int _livesIndex;
    readonly int _livesStages = 2;

    int oldLives;

    public static UIManager Instance;

    private void Awake()
    {
        Instance = this;
    }



    public void UpdateLostLifeUI(int newLives) 
    {
        /*
         * 1. Check current state of lives
         * 2. Figure out if lives have increased or decreased
         * 3. update UI accordingly
         */

        _livesIndex = newLives / _livesStages;
        if (oldLives < newLives && newLives % _livesStages == 0)
            _livesIndex--;
        int spriteIndex = newLives % _livesStages;


        Debug.Log("Life Stamp Index: " + _livesIndex + " | sprite index in use: " + spriteIndex);

        _livesContainer.transform.GetChild(_livesIndex).GetComponent<Image>().sprite = newLives < oldLives ?  _lostLivesSprites[spriteIndex] : _gainedLivesSprites[spriteIndex];
        _lowHealthIndicator.SetActive(newLives <= _livesForLowHealth);

        oldLives = newLives;
    }

    public void TogglePauseScreen() 
    {
        _pauseScreen.SetActive(!_pauseScreen.activeSelf);
    }



}
