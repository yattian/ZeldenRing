using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace YT
{
    public class TitleScreenRetainButtonSelection : MonoBehaviour
    {
        private GameObject lastSelectedObject;

        private void Update()
        {
            // Check if there's no selected object using the event system
            if (EventSystem.current.currentSelectedGameObject == null)
            {
                // Check if there was a previously selected object
                if (lastSelectedObject != null)
                {
                    // Set the previously selected object as the current selected object
                    EventSystem.current.SetSelectedGameObject(lastSelectedObject);
                }
            }
            else
            {
                // Update the last selected object
                lastSelectedObject = EventSystem.current.currentSelectedGameObject;
            }

            // Check if the mouse is clicked anywhere in the game window
            if (Input.GetMouseButtonDown(0))
            {
                // Check if the current selected object is null (deselected by mouse click)
                if (EventSystem.current.currentSelectedGameObject == null)
                {
                    // Set the previously selected object as the current selected object
                    EventSystem.current.SetSelectedGameObject(lastSelectedObject);
                }
            }
        }
    }
}
