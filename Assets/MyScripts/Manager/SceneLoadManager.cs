using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneLoadManager : PersisentSingletonTool<SceneLoadManager>
{
    private const string SCENENAME_GAMEPLAY = "Gameplay";

    [SerializeField] private Image faderImage;
    [SerializeField] private float faderTime;

    private Color faderColor;

    IEnumerator LoadSceneCor(string sceneName)
    {
        faderImage.gameObject.SetActive(true);

        var loadSceneOpreation = SceneManager.LoadSceneAsync(sceneName);
        //ʹ�ó����첽���� - ��boolΪfalseʱ������������ȫ������ϣ��ﵽ�ȵ����뵭��Ч������������¸�����
        loadSceneOpreation.allowSceneActivation = false;

        while(faderColor.a < 1f)
        {
            //���� - ͸���������� - �����ں���Χ��
            faderColor.a = Mathf.Clamp(faderColor.a + 1 / faderTime * Time.unscaledDeltaTime, 0f, 1f);
            faderImage.color = faderColor;
            yield return null;
        }

        loadSceneOpreation.allowSceneActivation = true; //�����������ó�����ȷ�������

        while (faderColor.a > 0f)
        {
            //���� - ͸���������� - �����ں���Χ��
            faderColor.a = Mathf.Clamp(faderColor.a - 1 / faderTime * Time.unscaledDeltaTime, 0f, 1f);
            faderImage.color = faderColor;
            yield return null;
        }

        faderImage.gameObject.SetActive(false);
    }


    public void LoadGameplayScene()
    {
        StartCoroutine(LoadSceneCor(SCENENAME_GAMEPLAY));
    }
}
