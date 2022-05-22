using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEnergy : SingletonTool<PlayerEnergy>
{
    [SerializeField] private EnergyBar_HUD energyBar;

    public const int MAXENERGY = 100;
    public const int HITADDEDVALUE = 1;//���������еģ�����ֱ�ӵ��� ������ͨ����������ʽ������

    private int curEnergy;

    private void Start()
    {
        curEnergy = MAXENERGY;
        energyBar.InitializeFillAmount(curEnergy, MAXENERGY);
    }

    public bool IsEnergyEnough(int value) => curEnergy >= value;

    public void ObtainEnergy(int value)
    {
        if (curEnergy == MAXENERGY) return;

        curEnergy = Mathf.Clamp(curEnergy + value, 0, MAXENERGY);
        energyBar.UpdateFillAmount(curEnergy, MAXENERGY);
        
    }

    public void UseEnergy(int value)
    {
        curEnergy = Mathf.Clamp(curEnergy - value, 0, MAXENERGY - value);

        energyBar.UpdateFillAmount(curEnergy,MAXENERGY);
    }
}
