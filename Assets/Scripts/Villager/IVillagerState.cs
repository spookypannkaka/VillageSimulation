public interface IVillagerState
{
    void EnterState(VillagerController villager);
    void UpdateState(VillagerController villager);
    void ExitState(VillagerController villager);
    //void HandleSteal(VillagerController villager);
    //void HandleGift(VillagerController villager);
    //void HandleAttack(VillagerController villager);
}