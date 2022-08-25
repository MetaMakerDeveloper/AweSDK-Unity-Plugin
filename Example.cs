using AweSDK;
using AweSDK.Core;
using AweSDK.Logic.Authorization;
using UnityEngine;
using UnityEngine.UI;
using AvatarDomain.ECS.Component;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;

public class Example : UnityEngine.MonoBehaviour
{
    public Button DestroyBtn;
    public Button RebuildBtn;

    private Context context;
    private Human human;
    LicenseManager licenseManager;

    async void Start()
    {
        AvatarEngine.NativeAPI = new NativeAPI();
        DestroyBtn.onClick.AddListener(DestroyHuman);
        RebuildBtn.onClick.AddListener(Rebuild);

        context = AweSDK.Environment.Setup();

        licenseManager = LicenseManager.GetInstance();
        licenseManager.AppKey = "{Your AppKey}";
        licenseManager.AppSecret = "{Your AppSecret}";

        // Import and export json files
        /*string humanJson = File.ReadAllText("json文件路径");
        human = await Human.Create(context, humanJson);
        human.ToJson();*/

        //dahei demo
        /*Human.BaseInfo humanInfo;
        humanInfo.Gender = Human.Gender.Male;
        humanInfo.FaceTarget = "dahei/face.target";
        humanInfo.FaceMapping = "dahei/face.jpg";
        humanInfo.BodyMapping = null;

        human = new Human(context, humanInfo);

        human.SetTarget("13004", 0.5f);
        human.SetTarget("10102", -0.2824f);
        human.SetTarget("10103", 0.2824f);
        human.SetTarget("10105", 0.2061f);
        human.SetTarget("10106", 0.4351f);
        human.SetTarget("10107", -0.2366f);
        human.SetTarget("10108", -0.2977f);
        human.SetTarget("13101", -1.3511f);
        human.SetTarget("13102", 1.4427f);
        human.SetTarget("13201", -1.4885f);
        human.SetTarget("13202", 0.8168f);

        human.WearHair("cloth/nan_tf_66");
        human.WearOutfits("cloth/nan_up_19_1", "cloth/nan_tz_12_down");
        human.WearShoes("cloth/nan_shoes_38");*/

        //xiaojing demo
        Human.BaseInfo humanInfo;
        humanInfo.Gender = Human.Gender.Female;
        humanInfo.FaceTarget = "xiaojing/face.target";
        humanInfo.FaceMapping = "xiaojing/face.jpg";
        humanInfo.BodyMapping = null;

        human = new Human(context, humanInfo);
        human.SetTarget("20003", 1f);
        human.SetTarget("23002", 0.5f);
        human.SetTarget("20101", 0.4769f);
        human.SetTarget("20102", -0.3075f);
        human.SetTarget("20502", -0.35222673416137695f);
        human.SetTarget("23202", 0.4769f);
        human.SetTarget("23503", -0.8489f);

        human.WearHair("cloth/nv_tf_128");
        human.WearOutfits("cloth/nv_up_06", "cloth/nv_tz_117_down");
        human.WearShoes("cloth/nv_shoes_98");

        human.WillLoad(() =>
        {
            UnityEngine.Debug.Log("Will load");
        });

        human.DidLoad(() =>
        {
            PlayTTS("你好，我是小静");
            GameObject player = human.GetGameObject();
            player.AddComponent<Animator>();
            if (human.GetBaseInfo().Gender == Human.Gender.Male)
            {
                player.GetComponent<Animator>().runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("animators/male");
            }
            else
            {
                player.GetComponent<Animator>().runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("animators/female");
            }
        });

        human.WillUpdate(() =>
        {
            UnityEngine.Debug.Log("Will update");
        });

        human.DidUpdate(() =>
        {
            UnityEngine.Debug.Log("Did update");
        });

        human.DidDestroy(() =>
        {
            UnityEngine.Debug.Log("Did destroy");
        });
    }

    void DestroyHuman()
    {
        human.Destroy();
    }

    async void Rebuild()
    {
        var httpTools = GameObject.Find("Main Camera").AddComponent<HttpTools>().GetComponent<HttpTools>();

        // 人脸重建 男性
        string path = UnityEngine.Application.streamingAssetsPath + "/media/picture/male.png";
        string result = await httpTools.RebuildFace(licenseManager.GenAuthString(), path, "male");

        JObject jObject = JObject.Parse(result);

        // 返回succeed为成功
        if ("succeed".Equals(jObject["message"].ToString()))
        {
            string targetUrl = jObject["target_url"].ToString();
            string mappingUrl = jObject["mapping_url"].ToString();

            Human.BaseInfo baseInfo = human.GetBaseInfo();
            baseInfo.Gender = Human.Gender.Male;
            baseInfo.FaceTarget = GetFilePathWithParentDirectory(targetUrl);
            baseInfo.FaceMapping = GetFilePathWithParentDirectory(mappingUrl);
            human.SetBaseInfo(baseInfo);
            human.SetTarget("13004", 0.5f);
            human.SetTarget("10102", -0.2824f);
            human.SetTarget("10103", 0.2824f);
            human.SetTarget("10105", 0.2061f);
            human.SetTarget("10106", 0.4351f);
            human.SetTarget("10107", -0.2366f);
            human.SetTarget("10108", -0.2977f);
            human.SetTarget("13101", -1.3511f);
            human.SetTarget("13102", 1.4427f);
            human.SetTarget("13201", -1.4885f);
            human.SetTarget("13202", 0.8168f);

            human.WearHair("cloth/nan_tf_66");
            human.WearOutfits("cloth/nan_up_19_1", "cloth/nan_tz_12_down");
            human.WearShoes("cloth/nan_shoes_38");
        }

        //人脸重建 女性
        /*string path = UnityEngine.Application.streamingAssetsPath + "/media/picture/female.jpg";
        int result = await httpTools.RebuildFace(licenseManager.GenAuthString(), path, "female");

        if ("succeed".Equals(jObject["message"].ToString()))
        {
            string targetUrl = jObject["target_url"].ToString();
            string mappingUrl = jObject["mapping_url"].ToString();

            Human.BaseInfo baseInfo = human.GetBaseInfo();
            baseInfo.Gender = Human.Gender.Female;
            baseInfo.FaceTarget = GetFilePathWithParentDirectory(targetUrl);
            baseInfo.FaceMapping = GetFilePathWithParentDirectory(mappingUrl);
            human.SetBaseInfo(baseInfo);
            human.SetTarget("20003", 1f);
            human.SetTarget("23002", 0.5f);
            human.SetTarget("20101", 0.4769f);
            human.SetTarget("20102", -0.3075f);
            human.SetTarget("20502", -0.35222673416137695f);
            human.SetTarget("23202", 0.4769f);
            human.SetTarget("23503", -0.8489f);

            human.WearHair("cloth/nv_tf_128");
            human.WearOutfits("cloth/nv_up_06","cloth/nv_tz_117_down");
            human.WearShoes("cloth/nv_shoes_98");
        }*/
    }

    async void PlayTTS(string text, string voiceName = "智能客服_静静")
    {
        string filePath = UnityEngine.Application.persistentDataPath + $"/tts";
        string fileName = "human";
        string result = await AweSDK.HttpTool.DownloadTTS(licenseManager.GenAuthString(), filePath, fileName, text, voiceName,70, 50);

        try
        {
            var jsonObj = JObject.Parse(result);

            if (jsonObj["err_code"].ToString() == "0")
            {
                string audioPath = jsonObj["audio_wav"].ToString();

                PlayAudio(audioPath);
                human.SetTTS(fileName);
            }

        }
        catch (Exception e)
        {
        }
    }

    public void PlayAudio(string audioPath)
    {
        StartCoroutine(DownloadAudio(audioPath, (result) =>
        {
            AudioSource.PlayClipAtPoint(result[0] as AudioClip, this.transform.position, 1);
        }));
    }

    IEnumerator DownloadAudio(string audioPath, Callback callback = null)
    {
        using (UnityWebRequest request = UnityWebRequestMultimedia.GetAudioClip("file://" + audioPath, AudioType.WAV))
        {
            yield return request.SendWebRequest();
            AudioClip audioClip = ((DownloadHandlerAudioClip)request.downloadHandler).audioClip;
            callback(audioClip);
        }
    }

    public string GetFilePathWithParentDirectory(string fullPath)
    {
        var pattern = @"[^/]+/[^/]+$";
        var match = Regex.Match(fullPath, pattern, RegexOptions.RightToLeft);

        if (match.Success)
            return match.Value;

        return string.Empty;
    }

    void LateUpdate()
    {
        AweSDK.Environment.Update();
    }
}
