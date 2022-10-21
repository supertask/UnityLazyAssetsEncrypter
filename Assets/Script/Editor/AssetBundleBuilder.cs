
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Text;

public class AssetBundleBuilder
{
    const string password = "password";

    [MenuItem("Assets/Build")]
    static void Build()
    {
        // AssetBundle���\�z
        var exportPath = Application.streamingAssetsPath + "/Sample/";
        //Directory.CreateDirectory(exportPath);
        var manifest = BuildPipeline.BuildAssetBundles(
            exportPath, 
            BuildAssetBundleOptions.ChunkBasedCompression, 
            BuildTarget.StandaloneWindows64);
        if (manifest == null) {
            Debug.Log("manifest is null");
            return;
        }

        // �}�j�t�F�X�g���琶������AssetBundle�ꗗ���擾���āA�S�ĈÍ�����������
        foreach ( var name in manifest.GetAllAssetBundles())
        {
            // uniqueSalt��Stream���Ƀ��j�[�N�ɂ���K�v������!!
            // �����AssetBundle����ݒ�
            Debug.Log(name);
            var assetNames = AssetDatabase.GetAssetPathsFromAssetBundle(name);
            var uniqueSalt = Encoding.UTF8.GetBytes(name);

            // �Í������ăt�@�C���ɏ�������
            var data = File.ReadAllBytes($"{exportPath}/{name}");
            using (var baseStream = new FileStream($"{exportPath}/e{name}", FileMode.OpenOrCreate))
            {
                var cryptor = new SeekableAesStream(baseStream, password, uniqueSalt);
                cryptor.Write(data, 0, data.Length);
            }
        }
    }
}