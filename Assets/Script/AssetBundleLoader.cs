//https://tsubakit1.hateblo.jp/entry/2019/03/16/162138
using System.Collections;
using UnityEngine;
using System.IO;
using System.Text;

public class AssetbundleLoader : MonoBehaviour
{
    AssetBundle bundle;
    FileStream fileStream;

    const string password = "password";

    void OnEnable()
    {
        // �Í�������AssetBundle���擾
        fileStream = new FileStream($"{Application.streamingAssetsPath}/esprite", FileMode.Open);
        var uniqueSalt = Encoding.UTF8.GetBytes("sprite"); // AssetBundle����solt�𐶐�

        // Stream�ňÍ�������������AssetBundle�����[�h����
        var uncryptor = new SeekableAesStream(fileStream, password, uniqueSalt);
        bundle = AssetBundle.LoadFromStream(uncryptor);
    }

    private IEnumerator Start()
    {
        // ���ʂ�AssetBundle�Ɠ��l�Ƀ��[�h�o����
        var request = bundle.LoadAssetAsync<Sprite>("01");
        yield return request;

        GetComponent<SpriteRenderer>().sprite = request.asset as Sprite;
    }

    private void OnDisable()
    {
        bundle.Unload(true);
        fileStream.Close();
    }
}
