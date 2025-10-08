# Dev Container 環境

このディレクトリには、開発コンテナ（Dev Container）の設定が含まれています。

## 概要

このプロジェクトは、Visual Studio Code の Dev Containers 拡張機能および GitHub Codespaces に対応しています。開発に必要な環境は自動的に構成されます。

## 含まれる環境

- .NET 9.0 SDK
- Entity Framework Core ツール（dotnet-ef）
- MariaDB 11.3
- Git および基本的な開発ツール

## 使用方法

### GitHub Codespaces

1. GitHubリポジトリページで「Code」→「Codespaces」を選択
2. 「Create codespace on develop」をクリック
3. 自動的にビルドされ、開発環境が起動します

### VS Code + Docker

1. [Docker Desktop](https://www.docker.com/products/docker-desktop/)をインストール
2. VS Codeに「Dev Containers」拡張機能をインストール
3. リポジトリを開き、コマンドパレット（Ctrl+Shift+P / Cmd+Shift+P）から「Dev Containers: Reopen in Container」を実行

## 構成

- `devcontainer.json`: Dev Container の基本設定
- `docker-compose.yml`: アプリケーションとMariaDBの構成
- `Dockerfile`: 開発用コンテナイメージの定義

## データベース接続情報

開発コンテナ環境では、以下の接続情報でMariaDBにアクセスできます：

- ホスト: `db`
- ポート: `3306`
- データベース: `ongeki-museum-api`
- ユーザー: `root`
- パスワード: `devpassword`

環境変数は自動的に設定されるため、アプリケーションはそのまま動作します。

## デバッグ実行

VS Codeのデバッグビューを開き、「.NET Core Launch (web)」を選択して実行（F5）してください。

## Entity Framework Core マイグレーション

コンテナ内のターミナルで以下のコマンドを実行できます：

```bash
# マイグレーションの作成
cd /workspace/OngekiMuseumApi/OngekiMuseumApi
dotnet ef migrations add <Name>

# マイグレーションの実行
cd /workspace/OngekiMuseumApi/OngekiMuseumApi
dotnet ef database update
```
