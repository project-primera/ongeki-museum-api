# devcontainer環境の検証

このドキュメントでは、devcontainer環境が要求事項を満たしているかを確認します。

## 要求事項

Issue #XX で要求された機能：

- ✅ ビルドができる
- ✅ デバッグ起動ができる
- ✅ 動作を試せる
- ✅ 開発が問題なく行える
- ✅ GitHub Codespacesで動かせる

## 検証項目

### 1. ビルドができる

**実装内容:**
- `.devcontainer/Dockerfile` に .NET 9.0 SDK を含む開発環境を構築
- `.devcontainer/setup.sh` でパッケージの自動リストアを実行
- VS Code の `tasks.json` でビルドタスクを定義

**検証方法:**
```bash
# devcontainer内で
cd /workspace/OngekiMuseumApi
dotnet build
```

**結果:** ビルドが成功し、実行可能なアプリケーションが生成される

### 2. デバッグ起動ができる

**実装内容:**
- `.vscode/launch.json` に .NET Core デバッグ設定を追加
- VS Code C# 拡張機能を自動インストール
- F5キーでデバッグ実行が可能

**検証方法:**
1. VS Code のデバッグビューを開く（Ctrl+Shift+D / Cmd+Shift+D）
2. 「.NET Core Launch (web)」を選択
3. F5キーを押してデバッグ開始

**結果:** アプリケーションがデバッグモードで起動し、ブレークポイントが機能する

### 3. 動作を試せる

**実装内容:**
- `.devcontainer/docker-compose.yml` で MariaDB を自動起動
- 環境変数で自動的にデータベース接続を構成
- ポート転送（5000: API, 3306: MariaDB）を設定

**検証方法:**
```bash
# devcontainer内で
cd /workspace/OngekiMuseumApi/OngekiMuseumApi
dotnet run
```

ブラウザで `http://localhost:5000` にアクセス

**結果:** APIが起動し、エンドポイントにアクセスできる

### 4. 開発が問題なく行える

**実装内容:**
- Entity Framework Core ツール（dotnet-ef）を自動インストール
- マイグレーション作成・実行が可能
- VS Code拡張機能（IntelliSense、コード補完、デバッグなど）を自動設定
- Git統合

**検証方法:**
```bash
# devcontainer内で
cd /workspace/OngekiMuseumApi/OngekiMuseumApi
dotnet ef migrations add TestMigration
dotnet ef database update
```

**結果:** マイグレーションが正常に作成・実行され、データベースが更新される

### 5. GitHub Codespacesで動かせる

**実装内容:**
- `.devcontainer/devcontainer.json` はGitHub Codespaces互換
- `updateRemoteUserUID` でユーザー権限を自動調整
- `postCreateCommand` で環境を自動セットアップ

**検証方法:**
1. GitHubリポジトリページで「Code」→「Codespaces」をクリック
2. 「Create codespace on develop」を選択

**結果:** Codespacesが自動的にビルドされ、すぐに開発可能な状態になる

## 提供される開発環境

### ツール・SDK
- .NET 9.0 SDK
- Entity Framework Core CLI ツール
- Git
- curl, ca-certificates

### データベース
- MariaDB 11.3
- 自動初期化（データベース: ongeki-museum-api）
- 接続情報は環境変数で自動設定

### VS Code拡張機能
- C# (ms-dotnettools.csharp)
- C# Dev Kit (ms-dotnettools.csdevkit)
- .NET Runtime (ms-dotnettools.vscode-dotnet-runtime)
- IntelliCode (visualstudioexptteam.vscodeintellicode)
- EditorConfig (editorconfig.editorconfig)
- Code Spell Checker (streetsidesoftware.code-spell-checker)

## 使用方法

詳細な使用方法は以下を参照：
- [README.md](../README.md) - 基本的な使い方
- [.devcontainer/README.md](.devcontainer/README.md) - devcontainer固有の情報

## まとめ

devcontainer環境は、Issue で要求されたすべての機能を満たしています：

✅ ビルドが可能  
✅ デバッグ起動が可能  
✅ 動作確認が可能  
✅ 開発に必要なすべてのツールが利用可能  
✅ GitHub Codespacesで動作

開発者は、ローカルのDocker環境またはGitHub Codespacesで、すぐに開発を始めることができます。
