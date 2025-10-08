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
    - または VS Code + Dev Container (推奨)
- mariadb

## 実行

### Dev Container を使う場合 (推奨)

このリポジトリはDev Container環境をサポートしています。以下のいずれかの方法で開発できます：

#### GitHub Codespaces
1. GitHubリポジトリページで「Code」→「Codespaces」→「Create codespace on develop」をクリック
2. 自動的にビルドされ、開発環境が整います
3. データベースも自動的に起動します

#### VS Code + Docker
1. [Docker Desktop](https://www.docker.com/products/docker-desktop/)をインストール
2. VS Codeに「Dev Containers」拡張機能をインストール
3. リポジトリを開き、コマンドパレット（Ctrl+Shift+P / Cmd+Shift+P）から「Dev Containers: Reopen in Container」を実行
4. データベースも自動的に起動します

Dev Container環境では、以下が自動的に構成されます：
- .NET 9.0 SDK
- Entity Framework Core ツール
- MariaDB データベース
- VS Code C# 拡張機能

#### デバッグ実行
1. VS Codeのデバッグビュー（Ctrl+Shift+D / Cmd+Shift+D）を開く
2. 「.NET Core Launch (web)」を選択して実行（F5）

### ローカル環境で実行する場合

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
