using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEnergy : SingletonTool<PlayerEnergy>
{
    [SerializeField] private EnergyBar_HUD energyBar;
    [SerializeField] private float overdriveCostInterval;
    [SerializeField] private float overdriveCostEnergy;

    public const float MAXENERGY = 100;
    public const float ENERGYADDEDVALUE = 1;//常量（公有的）可以直接调用 ，不能通过单例的形式调用它

    private float curEnergy;

    private WaitForSeconds overdriveCostIntervalWFS;

    private bool canObtianEnergy = true;

    protected override void Awake()
    {
        base.Awake();

        overdriveCostIntervalWFS = new WaitForSeconds(overdriveCostInterval);
    }

    private void Start()
    {
        curEnergy = MAXENERGY;
        energyBar.InitializeFillAmount(curEnergy, MAXENERGY);
    }

    private void OnEnable()
    {
        PlayerOverdrive.On += OverdriveOn;
        PlayerOverdrive.Off += OverdriveOff;
    }

    private void OnDisable()
    {
        PlayerOverdrive.On -= OverdriveOn;
        PlayerOverdrive.Off -= OverdriveOff;
    }

    public bool IsEnergyEnough(float value) => curEnergy >= value;

    public void ObtainEnergy(float value)
    {
        if (curEnergy == MAXENERGY || !gameObject.activeSelf || !canObtianEnergy) return;//能量爆发时不能获得能量?是否更改为可以获得
       
        curEnergy = Mathf.Clamp(curEnergy + value, 0, MAXENERGY);
        energyBar.UpdateFillAmount(curEnergy, MAXENERGY);
    }

    public void UseEnergy(float value)
    {
        curEnergy = Mathf.Clamp(curEnergy - value, 0, MAXENERGY - value);

        energyBar.UpdateFillAmount(curEnergy,MAXENERGY);

        if(curEnergy == 0 && gameObject.activeSelf&& !canObtianEnergy)
        {
            PlayerOverdrive.Off.Invoke();
        }
    }

    public void OverdriveOn()
    {
        canObtianEnergy = false;
        StartCoroutine(nameof(OverdriveEnergyCostCor));
    }

    public void OverdriveOff()
    {
        canObtianEnergy = true;
        StopCoroutine(nameof(OverdriveEnergyCostCor));
    }

    IEnumerator OverdriveEnergyCostCor()
    {
        while(curEnergy > 0f)
        {
            yield return overdriveCostIntervalWFS;

            UseEnergy(overdriveCostEnergy);
        }
    }
}
