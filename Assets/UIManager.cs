using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    public RectTransform materialMenu;
    public RectTransform actionMenu;
    private bool areMenuShowing = false;
    private bool menuAnimating = false;
    private float menuAnimationTransition;
    private float animationDuration = 0.2f;
    private void Update()
    {
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
            materialMenu.anchoredPosition = Vector2.Lerp(new Vector2(0, 190), new Vector2(0, -315), menuAnimationTransition);
            actionMenu.anchoredPosition = Vector2.Lerp(new Vector2(-190, 0), new Vector2(313, 0), menuAnimationTransition);
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
}
