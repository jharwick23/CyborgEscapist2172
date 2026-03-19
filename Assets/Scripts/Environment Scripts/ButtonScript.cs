using UnityEngine;

public class ButtonScript : MonoBehaviour, IInteractable
{
    [SerializeField] private DoorScript door;

    void Start()
    {

    }

    public bool CanInteract()
    {
        return true;
    }

    public bool Interact(Interactor interactor)
    {
        door.DoorInteraction(); // Interacts with door if it exists

        return true;
    }
}
