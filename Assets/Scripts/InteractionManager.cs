using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    [SerializeField] private Transform _interactionPoint;
    [SerializeField] private float _interactionPointRadius = 0.5f;
    [SerializeField] LayerMask _interactableMask;

    private readonly Collider[] _colliders = new Collider[3];
    [SerializeField] private int _numFound;
    [SerializeField] private string tag;

    void Update()
    {
        //finds the number of interactable objects around
        _numFound = Physics.OverlapSphereNonAlloc(_interactionPoint.position, _interactionPointRadius, _colliders,
            _interactableMask);

        if (_numFound > 0)
        {
            var interactable = _colliders[0].GetComponent<IInteractable>();

            //computer
            if (interactable != null && Input.GetKeyDown("e") && _colliders[0].CompareTag("Laptop"))
            {

                transform.GetComponent<PlayerBehaviour>().canMove = interactable.Interact();
                tag = _colliders[0].tag;
            }

            //coffee automat
            if (interactable != null && Input.GetKeyDown("e") && _colliders[0].CompareTag("CoffeeAutomat"))
            {
                transform.GetComponent<PlayerBehaviour>().canMove = interactable.Interact();
                tag = _colliders[0].tag;
            }

            //flag
            if (interactable != null && _colliders[0].CompareTag("Flag"))
            {
                Debug.Log("DELETE FLAG??");
                tag = _colliders[0].tag;
            }
            if (interactable != null && Input.GetKeyDown("x") && _colliders[0].CompareTag("Flag"))
            {
                tag = _colliders[0].tag;
                Destroy(_colliders[0].gameObject);
            }
        }
    }
}
