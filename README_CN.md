[English README](https://github.com/kushinn/ShaderlabVS/blob/master/README.md)

ShaderlabVS
===========

ShaderlabVS 用于 Unity Shaderlab 编程的 Visual Studio 插件。最新的版本可以从 Release 页面下载。

### 支持的文件:

* .shader
* .cginc
* .glslinc
* .compute
* .cg
* .hlsl

Features
-----
### 自定义配置文件
可在插件安装目录下Data目录找到[CGIncludes.def]、
[HLSL_CG_datatype.def]、[HLSL_CG_functions.def]、[HLSL_CG_Keywords.def]、
[Unity3D_datatype.def]、[Unity3D_functions.def]、[Unity3D_keywords.def]、[Unity3D_macros.def]、[Unity3D_values.def]
等等配置项, 在[CGIncludes.def]中添加/修改预制定义的项，可以使得VS能够自动完成更多的Words。
但是切记，不要增加太多不常用的Unity版本的目录，否则可能会导致第一次打开文件变慢。

### 注意
![注意]使用ShaderlabVS\Src\ShaderlabVS\Data 下的文件覆盖插件安装目录下的文件，或自行新增关键词、函数等可丰富提升功能。

### 代码高亮和大纲

![Highlighting](./img/Highlighting.PNG)

### 帮助信息

![QuickInfo](./img/QuickInfo.PNG)

### 代码自动完成

![CodeCompletion](./img/CodeCompletion.PNG)

### CG 以及 Unity 函数提示

![SignatureHelp](./img/SignatureHelp.PNG)

### 支持黑色主题

![dark](./img/dark.png)

开发
-----

### 环境需求

* Visual Studio
* Visual Studio SDK

### 如何在 Visual Studio 中调试
1. 下载和安装 Visual Studio SDK (VS 2013 之前需要这一步)
2. 打开 ShaderlabVS 解决方案 
3. 按 *F6* 编译整个方案
4. 请确 Shaderlab 项目设置中的 **Debug** 标签页下的 **_Start exteral program_** 和 **_Comand line arguments_** 项设置的值如下:
    1. 将 **_Start exteral program_** 设置为 devenv.exe 的路径 (Visual studio 主程序)
    2. 将 **_Comand line arguments_** 设置为 **/rootsuffix Exp**. 下图是设置的实例:

![](./img/DebugSettings.PNG)

### 支持的 Visual Studio 版本:
* Visual Studio 2013
* Visual Studio 2015
* Visual Studio 2017

__其他版本暂时没有测试，欢迎 Pull Request 添加测试结果.__

### 感谢

晨曦
Rocky Lai

