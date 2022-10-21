
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
        // AssetBundleを構築
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

        // マニフェストから生成したAssetBundle一覧を取得して、全て暗号化をかける
        foreach ( var name in manifest.GetAllAssetBundles())
        {
            // uniqueSaltはStream毎にユニークにする必要がある!!
            // 今回はAssetBundle名を設定
            Debug.Log(name);
            var assetNames = AssetDatabase.GetAssetPathsFromAssetBundle(name);
            var uniqueSalt = Encoding.UTF8.GetBytes(name);

            // 暗号化してファイルに書き込む
            var data = File.ReadAllBytes($"{exportPath}/{name}");
            using (var baseStream = new FileStream($"{exportPath}/e{name}", FileMode.OpenOrCreate))
            {
                var cryptor = new SeekableAesStream(baseStream, password, uniqueSalt);
                cryptor.Write(data, 0, data.Length);
            }
        }
    }
}