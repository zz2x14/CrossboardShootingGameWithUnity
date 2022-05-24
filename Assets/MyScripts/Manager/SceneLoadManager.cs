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
        //使用场景异步加载 - 该bool为false时，场景不会完全加载完毕，达到等到淡入淡出效果结束后进入下个场景
        loadSceneOpreation.allowSceneActivation = false;

        while(faderColor.a < 1f)
        {
            //淡出 - 透明度逐渐增加 - 限制在合理范围内
            faderColor.a = Mathf.Clamp(faderColor.a + 1 / faderTime * Time.unscaledDeltaTime, 0f, 1f);
            faderImage.color = faderColor;
            yield return null;
        }

        //和yield return loadSceneOpreation.progress >= 0.9f 是否完全相同？TODO
        yield return new WaitUntil(() => loadSceneOpreation.progress >= 0.9f);//确保异步加载进度加载到0.9后再完成加载 - 避免性能原因加载过久

        loadSceneOpreation.allowSceneActivation = true; //淡出结束后让场景正确加载完毕

        while (faderColor.a > 0f)
        {
            //淡出 - 透明度逐渐增加 - 限制在合理范围内
            faderColor.a = Mathf.Clamp(faderColor.a - 1 / faderTime * Time.unscaledDeltaTime, 0f, 1f);
            faderImage.color = faderColor;
            yield return null;
        }

        faderImage.gameObject.SetActive(false);
    }


    public void LoadGameplayScene()
    {
        StopAllCoroutines();//避免反复切换时上一个切换协程还未结束
        StartCoroutine(LoadSceneCor(SCENENAME_GAMEPLAY));
    }

    public void LoadStartScene()
    {
        StopAllCoroutines();
        StartCoroutine(LoadSceneCor(SCENENAME_STARTSCENE));
    }
}
