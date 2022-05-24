using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEnergy : SingletonTool<PlayerEnergy>
{
    [SerializeField] private EnergyBar_HUD energyBar;
    [SerializeField] private float overdriveCostInterval;
    [SerializeField] private float overdriveCostEnergy;

    public const float MAXENERGY = 100;
    public const float ENERGYADDEDVALUE = 1;//���������еģ�����ֱ�ӵ��� ������ͨ����������ʽ������

    private float curEnergy;

    private WaitForSeconds overdriveCostIntervalWFS;

    private bool canObtianEnergy;

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
        if (curEnergy == MAXENERGY || !gameObject.activeSelf || !canObtianEnergy) return;//��������ʱ���ܻ������?�Ƿ����Ϊ���Ի��

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
