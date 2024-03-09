using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using System.Reflection;


public class test : MonoBehaviour
{
    public Material mat1;
    public Material mat2;
    ScriptableRendererData data;
    bool a;
    // Start is called before the first frame update
    void Start()
    {
        data = GetRendererData(1);


    }

    // Update is called once per frame
    void Update()
    {
        a = Input.GetKeyDown(KeyCode.Space);
        if(a)
        {
            List<ScriptableRendererFeature> rendererFeatures = data.rendererFeatures;
            if (rendererFeatures == null || rendererFeatures.Count <= 0) return;
            rendererFeatures[0].SetActive(true);
        }
    }

    public void SetActiveRendererFeature<T>(bool active) where T : ScriptableRendererFeature
    {
        // URP Asset�� Renderer List���� 0�� �ε��� RendererData ����
        ScriptableRendererData rendererData = GetRendererData(0);
        if (rendererData == null) return;

        List<ScriptableRendererFeature> rendererFeatures = rendererData.rendererFeatures;
        if (rendererFeatures == null || rendererFeatures.Count <= 0) return;

        for (int i = 0; i < rendererFeatures.Count; i++)
        {
            ScriptableRendererFeature rendererFeature = rendererFeatures[i];
            if (!rendererFeature) continue;
            if (rendererFeature is T) rendererFeature.SetActive(active);
        }
#if UNITY_EDITOR
        rendererData.SetDirty();
#endif
    }


    public ScriptableRendererData GetRendererData(int rendererIndex = 0)
    {
        // ���� Quality �ɼǿ� ���õ� URP Asset ����
        UniversalRenderPipelineAsset pipelineAsset = GraphicsSettings.currentRenderPipeline as UniversalRenderPipelineAsset;
        if (!pipelineAsset) return null;

        // URP Renderer List ���÷��� ���� (Internal ������ �׳� ���� �Ұ���)
        FieldInfo propertyInfo = pipelineAsset.GetType().GetField("m_RendererDataList", BindingFlags.Instance | BindingFlags.NonPublic);
        ScriptableRendererData[] rendererDatas = (ScriptableRendererData[])propertyInfo.GetValue(pipelineAsset);
        if (rendererDatas == null || rendererDatas.Length <= 0) return null;
        if (rendererIndex < 0 || rendererDatas.Length <= rendererIndex) return null;

        return rendererDatas[rendererIndex];
    }
}
