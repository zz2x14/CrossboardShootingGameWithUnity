using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneLoadManager : PersistentSingletonTool<SceneLoadManager>
{
    private const string SCENENAME_GAMEPLAY = "Gameplay";
    private const string SCENENAME_STARTSCENE = "StartScene";

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

        //��yield return loadSceneOpreation.progress >= 0.9f �Ƿ���ȫ��ͬ��TODO
        yield return new WaitUntil(() => loadSceneOpreation.progress >= 0.9f);//ȷ���첽���ؽ��ȼ��ص�0.9������ɼ��� - ��������ԭ����ع���

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
        StopAllCoroutines();//���ⷴ���л�ʱ��һ���л�Э�̻�δ����
        StartCoroutine(LoadSceneCor(SCENENAME_GAMEPLAY));
    }

    public void LoadStartScene()
    {
        StopAllCoroutines();
        StartCoroutine(LoadSceneCor(SCENENAME_STARTSCENE));
    }
}
