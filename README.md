# ongeki-museum

[![GitHub License](https://img.shields.io/github/license/project-primera/ongeki-museum-api)
](https://github.com/project-primera/ongeki-museum-api/blob/develop/LICENSE)
[![GitHub Release](https://img.shields.io/github/v/release/project-primera/ongeki-museum-api?logo=github)
](https://github.com/project-primera/ongeki-museum-api/releases/latest)
[
![Docker Image Version](https://img.shields.io/docker/v/projectprimera/ongeki-museum-api?logo=docker&color=blue)](https://hub.docker.com/r/projectprimera/ongeki-museum-api)


オンゲキの公式楽曲データを正規化して提供するAPI。
利用に際して、各権利者様の権利を尊重し、個人利用の範囲内でご利用ください。

## 動作に必要なもの

- C#の開発環境
    - Visual Studio もしくは JetBrains Rider がオススメ
- mariadb

## 実行

- `OngekiMuseumApi\OngekiMuseumApi\appsettings.Development.json` にmariadbへの接続設定を書く
- c#の開発環境で実行

## マイグレーション

### 作成

```bash
cd ./OngekiMuseumApi/OngekiMuseumApi
dotnet ef migrations add <Name>
```

### 実行
```bash
cd ./OngekiMuseumApi/OngekiMuseumApi
dotnet ef database update
```

（ゆくゆくはちゃんと書きます）
