using UnityEngine;
using UnityEngine.Events;

public class PlayerActions : MonoBehaviour
{
    public UnityEvent<Vector3> OnStealItem = new UnityEvent<Vector3>();
    public UnityEvent<Vector3> OnAttack = new UnityEvent<Vector3>();

    public void StealItem(Vector3 itemPosition)
    {
        OnStealItem.Invoke(itemPosition);
    }

    public void Attack(Vector3 attackPosition)
    {
        OnAttack.Invoke(attackPosition);
    }
}
