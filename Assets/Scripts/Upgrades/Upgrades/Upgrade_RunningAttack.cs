using UnityEngine;

[CreateAssetMenu(menuName = "Upgrades/Running Attack", fileName = "RunningAttack")]
public class Upgrade_RunningAttack : Upgrade
{
    PlayerState_Runnig runningState;
    public override void onAdded(GameObject entity)
    {
        runningState = entity.GetComponent<Player_References>().RunningState as PlayerState_Runnig;

        runningState.isRunningAttackUnlocked = true;
    }

    public override void onRemoved(GameObject entity)
    {
        runningState.isRunningAttackUnlocked = false;
    }

    public override string shortDescription()
    {
        return $"Unlock { UsefullMethods.highlightString("Running Attack")}";
    }
}
