using UnityEngine;

public class Player_SpecialAttack : MonoBehaviour
{
    [SerializeField] Player_References playerRefs;

    float amountToHeal;

    public bool isChargeFilled()
    {
        return playerRefs.currentStats.CurrentBloodFlow! < playerRefs.currentStats.MaxBloodFlow;
    }
    public void addCharge(float amount)
    {
        float temporalValue = playerRefs.currentStats.CurrentBloodFlow + (amount * playerRefs.currentStats.BloodflowMultiplier);
        
        if (temporalValue > playerRefs.currentStats.MaxBloodFlow) { temporalValue = playerRefs.currentStats.MaxBloodFlow; }
        else if (temporalValue < 0) { temporalValue = 0; }

        playerRefs.currentStats.CurrentBloodFlow =temporalValue;
    }
    public void restartCharge()
    {
        playerRefs.currentStats.CurrentBloodFlow = 0;
    }

    //I dont like this being here pls move to a Player_AnimationEvents or into SpecialHeal State
    public void EV_ActuallyHeal()
    {
        restartCharge();
        playerRefs.GetComponent<IHealth>().RemoveHealth(-amountToHeal);
        playerRefs.events.OnActuallySpecialHeal?.Invoke();
    }
}
