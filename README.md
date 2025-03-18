# ongeki-museum

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
