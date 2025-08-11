## 项目 Releases 迁移公告

由于需求的变更，本项目在创立之初的架构可能在未来无法满足需要，因此我们未来会重写底层框架，开发更加通用强大的歌声合成工程处理接口，探索全新的调音工作流。目前本项目的日常更新以及未来框架的搭建均会在组织账号 [@openvpi](https://github.com/openvpi) 下进行，届时也将欢迎更多开发者的加入，敬请关注。

项目成果现已公测：[B站专栏](https://www.bilibili.com/read/cv16468227)

日常发行版更新已移至：[OpenVPI 主页](https://openvpi.github.io/)

# OpenSVIP

基于 X Studio · 歌手工程文件（.svip）的歌声合成软件工程文件中介模型和转换框架



## 项目简介

本项目致力于建立一个主要基于 X Studio · 歌手 `.svip` 格式工程文件的中介表示模型和格式转换框架，并实现各类歌声合成软件的工程文件互相转换。通过开发适当的插件，任何格式都将能够接入框架并与框架内其他所有支持的文件格式互相转换。

本框架能够转换的内容将包括但不限于：

- 音轨信息
- 音符序列
- 歌词序列
- 参数（需要映射规则）

当前已经支持、正在开发和计划中的格式转换器：

- X Studio 工程文件 (*.svip)
- OpenSVIP 文件 (*.json)
- Synthesizer V 工程文件 (*.svp)
- 歌叽歌叽工程文件 (*.gj)
- Project Vogen 工程文件 (*.vog)
- MIDI 文件 (*.mid)
- 元七七编辑器工程文件 (*.y77)
- ACE Studio 工程文件 (*.acep)
- OpenUtau 工程文件 (*.ustx)（开发中）
- VOCALOID 3/4 工程文件 (*.vsqx)（开发中）
- VOCALOID 6 序列文件 (*.vpr)（开发中，本仓库）
- Utau 工程文件 (*.ust)（[开发中](https://github.com/oxygen-dioxide/opensvip)）
- DeepVocal 工程文件 (*.dv)（计划中）
- VocalSharp 工程文件 (*.vspx)（计划中）

## 使用方法

### GUI 桌面应用程序

OpenSVIP 工程转换器已开启公测：详见 [OpenVPI 主页](https://openvpi.github.io/home/)

### C# 命令行工具

使用以下命令可以获得完整的使用指南：

```shell
OpenSvip.Console.exe --help
```

#### 运行环境要求

需要 .NET Framework 4.7.2 以上。运行部分插件可能需要满足其他要求，详见插件说明。

#### 工程文件转换

命令基本格式为：

```shell
OpenSvip.Console.exe -i <输入标识符> -o <输出标识符> <输入文件路径> <输出文件路径>
```

其中，输入和输出标识符用于指定所使用的插件。例如，将当前目录下的一个 `.svip` 文件转换为 OpenSVIP JSON 文件：

```shell
OpenSvip.Console.exe -i svip -o json test.svip test.json
```

此外，部分插件支持设定输入和输出选项。若要指定选项，需在命令中加入（以输入选项为例）：

```shell
--input-options <选项名>=<选项值>{;<选项名>=<选项值>}
```

例如，在转换为 `.json` 文件时指定输出带缩进的格式：

```shell
OpenSvip.Console.exe -i svip -o json test.svip test.json --output-options indented=true
```

在转换为 `.svip` 文件时指定输出版本为 SVIP6.0.0 (X Studio 1.8)，默认歌手为何畅：

```shell
OpenSvip.Console.exe -i json -o svip test.json test.svip --output-options version=6.0.0;singer=何畅
```

选项分为字符串、整数、浮点数、布尔值、枚举类型，且均具有默认值。具体选项内容由插件本身决定，详见各插件信息。

#### 查看插件信息

可以使用以下命令查看所有插件（包括其对应的标识符）：

```shell
OpenSvip.Console.exe plugins
```

使用以下命令查看某个插件的详细信息：

```shell
OpenSvip.Console.exe plugins -d <插件标识符>
```

### Python 开发工具

TODO

# 关于本仓库 opensvip-plugin-vpr

适用于 OpenSvip 的 VOCALOID 6 序列文件转换插件。

## 仓库简介

本仓库目的在于实现 OpenSvip 原生支持的 VOCALOID 6 序列文件转换，可能可以适用于 VOCALOID 5，但未测试。

## 插件

适用格式：VOCLAOID 6 序列文件 (*.vpr)

### 数据支持清单

#### 读取  

|   数据内容   | 支持等级 |                   说明                   |
| :----------: | :------: | :--------------------------------------: |
|     轨道     |    ✓     |                                          |
|     曲谱     |    ✓     |                                          |
|     歌词     |    ✓     |  VOCALOID 不支持中文，自动转换拼音为歌词   |
|   音高模板   |    ×     |                                          |
|     VEL      |    ×     |          中介模型不存在对应参数           |
|     DYN      |    ✓     |                                          |
|     PB       |    ×     |                                          |
|     PBS      |    ×     |                                          |
|     EXT      |    ×     |          中介模型不存在对应参数           |
|     GRW      |    ×     |          中介模型不存在对应参数           |
|     BRE      |    ×     |                                          |
|     AIR      |    ×     |          中介模型不存在对应参数           |
|     MOU      |    ×     |          中介模型不存在对应参数           |
|     CHA      |    ×     |          中介模型不存在对应参数           |
|     BRI      |    ×     |          中介模型不存在对应参数           |
|     CLR      |    ×     |          中介模型不存在对应参数           |
|     POR      |    ×     |          中介模型不存在对应参数           |


#### 写入  

|   数据内容   | 支持等级 |                   说明                   |
| :----------: | :------: | :--------------------------------------: |
|     轨道     |    ✓     |                                          |
|     曲谱     |    ✓     |                                          |
|     歌词     |    ✓     |                                          |
|   音高模板   |    ×     |                                          |
|     VEL      |    ×     |          中介模型不存在对应参数           |
|     DYN      |    ✓     |                                          |
|     PB       |    ✓     |                                          |
|     PBS      |    ×     |                固定为16                  |
|     EXT      |    ×     |          中介模型不存在对应参数           |
|     GRW      |    ×     |          中介模型不存在对应参数           |
|     BRE      |    ×     |                                          |
|     AIR      |    ×     |          中介模型不存在对应参数           |
|     MOU      |    ×     |          中介模型不存在对应参数           |
|     CHA      |    ×     |          中介模型不存在对应参数           |
|     BRI      |    ×     |          中介模型不存在对应参数           |
|     CLR      |    ×     |          中介模型不存在对应参数           |
|     POR      |    ×     |          中介模型不存在对应参数           |

#### 高级选项

输入选项

是否将音频文件从 VPR 文件中复制到 VPR 文件所在目录
VPR 工程中直接包含音频轨的音频文件(.wav)，若启用则将其复制到 VPR 文件所在目录，否则将其忽略。
默认值：true

输出选项

是否将音频文件复制到 VPR 文件中（会增加文件大小）
VPR 工程中直接包含音频轨的音频文件(.wav)，若启用则将其复制到 VPR 文件中，否则将其忽略。
注意：启用后，分发 VPR 文件可能同时被视为分发其工程中使用到的音频文件，请谨慎使用。
默认值：true

是否启用反音高补偿
若启用该选项，插件则会在转换音高时计算 VOCALOID 6 对最终音高的补偿并去除。
启用后，最终音高将尽力与原始音高贴合。
默认值：true

### FQA

Q：为什么为什么不合并到 OpenSvip 主仓库？  
A：因为原作者已半弃坑，且本仓库从一开始就没有考虑过合并或依赖于主仓库的分发渠道。

Q：音高参数转换不准确/我需要的参数没有转换。  
A：请先考虑使用[utaformatix](https://sdercolin.github.io/utaformatix3/)或其[镜像](https://utaformatix.phska.cn/)，它们可能可以提供更多的参数支持或更准确的音高转换。

Q：为什么有这么多警告？/为什么有警告？  
A：  
- 参数警告：因为部分参数可能由于转换算法不准或数据本身错误导致超出范围，大部分时候参数相关的警告是可忽略的。
- 音频复制警告：由于音频文件找不到或wav格式不正确，导致音频文件未能复制，通常可忽略。
- 音轨警告：音轨类型未知，无法转换，这可能是由于格式损坏导致，通常情况下这是不可能发生的。