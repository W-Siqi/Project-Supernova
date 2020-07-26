using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BookController))]
public class BookControllerWrapper : MonoBehaviour
{
    [SerializeField]
    private BookController bookController;

    public void TurnNextPage() {
        bookController.NextPage();
    }

    public void TurnPreviousPage() {
        bookController.PreviousPage();
    }
}
