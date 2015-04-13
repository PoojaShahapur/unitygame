using UnityEditor;
using UnityEngine;

namespace EditorTool
{
    /**
     * @brief 发布产品接口
     */
    public class PublishProduct
    {
        // 发布 windows 平台产品
        [MenuItem("Assets/PublishProduct/Windows")]
        static void Windows_PublishProduct()
        {
            // 实例化全局数据
            ResCfgData.Instance();
            // 打包生成 unity3d 资源
            PublishProductUtil.pkgResources();
            // 拷贝资源到输出目录
            PublishProductUtil.copyRes2Dest();
            // 删除 Resources 目录，防止资源被重复打包进输出镜像
            PublishProductUtil.delResources();
            // 生成镜像
            PublishProductUtil.buildImage();
            // 恢复 Resources 目录资源
            PublishProductUtil.restoreResources();
        }
    }
}