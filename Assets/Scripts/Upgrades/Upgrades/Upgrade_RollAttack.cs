using UnityEngine;

[CreateAssetMenu(menuName = "Upgrades/Roll Attack", fileName = "RollAttack")]
public class Upgrade_RollAttack : Upgrade
{
    PlayerState_Rolling rollingState;
    public override void onAdded(GameObject entity)
    {
        rollingState = entity.GetComponent<Player_References>().RollingState as PlayerState_Rolling;

        rollingState.isRollAttackUnlocked = true;
    }

    public override void onRemoved(GameObject entity)
    {
        rollingState.isRollAttackUnlocked = false;
    }

    public override string shortDescription()
    {
        return $"Unlock { UsefullMethods.highlightString("Roll Attack")}";
    }
}
