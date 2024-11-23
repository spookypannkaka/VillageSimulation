using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerActions : Singleton<PlayerActions>
{
    public UnityEvent<Vector3> OnStealItem = new UnityEvent<Vector3>();
    public UnityEvent<Vector3> OnAttack = new UnityEvent<Vector3>();

    private ActionManager actionManager;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        actionManager = GetComponent<ActionManager>();
    }

    public void Steal(Vector3 itemPosition)
    {
        actionManager.TryExecuteAction(PlayerController.Instance.GetKeyBinding("Steal"));
    }

    public void Attack(Vector3 attackPosition)
    {
        OnAttack.Invoke(attackPosition);
    }

    public void UseItem(InputAction.CallbackContext context)
    {
        if (context.started && PlayerInventory.Instance.HasCake)
        {
            PlayerInventory.Instance.EatCake();
        }
    }

    public void AttemptToGiveGift()
    {
        VillagerController activeVillager = VillagerManager.Instance.GetActiveVillager();
        if (activeVillager != null)
        {
            if (PlayerInventory.Instance.HasCake)
            {
                activeVillager.OnPlayerGiftsItem();
            }
        }
    }

}
