using UnityEngine;

public class Interactor : MonoBehaviour
{
    [SerializeField] private float interactRange = 1.5f;

    // Check around the player to see what can be interacted with
    public void TryInteract()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, interactRange);

        foreach(var hit in hits)
        {
            IInteractable interactable = hit.GetComponent<IInteractable>();

            if(interactable != null && interactable.CanInteract())
            {
                interactable.Interact(this);
                return;
            }
        }
    }
}
