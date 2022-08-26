using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class ChangeInput : MonoBehaviour
{
    EventSystem eventSystem;

    [SerializeField]Selectable firstInput;
    [SerializeField]Button submitButton;

    void Start()
    {
        eventSystem = EventSystem.current;
        firstInput.Select();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && Input.GetKey(KeyCode.LeftShift))
        {
            SelectPrevious();
        }
        else if (Input.GetKeyDown(KeyCode.Tab))
        {
            SelectNext();
        }
        else if (Input.GetKeyDown(KeyCode.Return))
        {
            Submit();
        }
    }

    void SelectPrevious()
    {
        Selectable previous = eventSystem.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnUp();
        if (previous != null)
        {
            previous.Select();
        }
    }

    void SelectNext()
    {
        Selectable next = eventSystem.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnDown();
        if (next != null)
        {
            next.Select();
        }
    }

    void Submit()
    {
        submitButton.onClick.Invoke();
    }
}
