using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour {

    public RectTransform materialMenu;
    public RectTransform actionMenu;
    private bool areMenuShowing = false;
    private bool menuAnimating = false;
    private float menuAnimationTransition;
    private float animationDuration = 0.2f;
    public GameObject instructpanel;
    public bool isinstruct;
    private void Update()
    {
        if(isinstruct)
        {
            instructpanel.SetActive(true);
        }
        else if(!isinstruct)
        {
            instructpanel.SetActive(false);
        }
        if (Input.GetKeyDown("space"))
           OnTheOneButtonClick();

        if (menuAnimating)
        {
            if (areMenuShowing)
            {
                menuAnimationTransition += Time.deltaTime * (1 - animationDuration);
                if (menuAnimationTransition > 1)
                {
                    menuAnimationTransition = 1;
                    menuAnimating = false;
                }
            }

            else
            {
                menuAnimationTransition -= Time.deltaTime * (1 - animationDuration);
                if (menuAnimationTransition <= 0)
                {
                    menuAnimationTransition = 0;
                    menuAnimating = false;
                }

            }
            materialMenu.anchoredPosition = Vector2.Lerp(new Vector2(0, 252), new Vector2(0, -315), menuAnimationTransition);
            actionMenu.anchoredPosition = Vector2.Lerp(new Vector2(-313, 0), new Vector2(445, 0), menuAnimationTransition);
        }
    } 
    public void OnTheOneButtonClick()
    {
        areMenuShowing = !areMenuShowing;
        PlayeMenuAnimation();
    }
    public void PlayeMenuAnimation()
    {
        menuAnimating = true;
    }
    public void OnBackClick()
    {
        SceneManager.LoadScene("Menu");
    }
    public void OnHelpClick()
    {
        isinstruct = !isinstruct;
    }
}
