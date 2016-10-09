using UnityEngine;
using System.Collections;
using VRTK;

public class Controller_MenuSwapper : MonoBehaviour
{
    MenuManager menuHandler;

    // Use this for initialization
    void Start()
    {
    
        ControllerInteractionEventHandler touchpadPressed = new ControllerInteractionEventHandler(DoTouchpadPressed);
        
        var controllerEvent = GameObject.FindObjectOfType<VRTK_ControllerEvents>();
        menuHandler = GameObject.FindObjectOfType<MenuManager>();   
        controllerEvent.TouchpadPressed += touchpadPressed;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void DoTouchpadPressed(object sender, ControllerInteractionEventArgs e)
    {
        Debug.Log(e);
        if(e.touchpadAxis.x > 0)
        {
            menuHandler.loadNextScene();
        }
        else if(e.touchpadAxis.x < 0)
        {
            menuHandler.loadPreviousScene();
        }
    }
}
