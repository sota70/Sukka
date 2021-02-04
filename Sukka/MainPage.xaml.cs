using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.WindowManagement;
using System.Threading.Tasks;
using System.Diagnostics;
using Windows.UI.Xaml.Hosting;

// いちいち打つのが面倒だから
using Windows.Storage;
// 空白ページの項目テンプレートについては、https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x411 を参照してください

namespace Sukka
{

    /// <summary>
    /// それ自体で使用できる空白ページまたはフレーム内に移動できる空白ページ。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        // ページごとのデータマップ宣言
        private Dictionary<string, Points> page1 = new Dictionary<string, Points>();

        // データをいくつ作ったかたカウントする用のinteger
        private int count_ = 0;

        private int buttoncount = 0;


        // 数値をUIから変更できるようにするためのint
        private int scpoint_;
        private int contributionpoint_;

        // ポイントを足すか引くかを設定する用のbool
        private bool operators_;


        // アプリを開いてからデータを１回でも編集したかどうか
        private bool isEdit = false;


        public MainPage()
        {
            this.InitializeComponent();

            // ページ１にサンプル用のデータを登録する
            Points sample__ = new Points(0, 0, 0);

            page1.Add("Sample", sample__);

            buttoncount += 1;


            // データファイルを初期化
            DatabaseInit();


            // プレイヤーデータをロードする
            LoadPlayerData();
        }

        private void SC_SelectionChanged(object sender, RoutedEventArgs e)
        {

        }

        // AddDataをクリックすると発動するボタン作成イベント
        private void AddData(object sender, RoutedEventArgs e)
        {
            // 新しいボタン作成
            Button newButton__ = new Button();


            // カウントの追加
            count_ += 1;

            buttoncount += 1;


            // ボタンの文字設定
            newButton__.Content = "Sample" + buttoncount;
            newButton__.FontSize = 60;
            newButton__.FontFamily = new FontFamily("Arial");


            // バックグラウンドの背景を消す
            newButton__.Background = null;


            // ボタンのサイズ設定
            newButton__.Width = 465;
            newButton__.Height = 87;


            // ボタンにクリックイベントを追加
            newButton__.Click += ShowPoints;


            // ボタンの配置位置設定
            newButton__.HorizontalAlignment = HorizontalAlignment.Center;



            RelativePanel.SetAlignHorizontalCenterWith(newButton__, sender);


            // Addボタンデータ取得
            Button addbutton = Add;


            //一度Addボタンを付けなおして位置を直す
            NameList.Children.Remove(Add);

            NameList.Children.Add(newButton__);

            NameList.Children.Add(addbutton);


            // ページに入れるデータを宣言
            Points sample__ = new Points(1, 1, count_);

            page1.Add(newButton__.Content.ToString(), sample__);
        }

        // このボタンをクリックするとポイントが表示されるイベント
        private void ShowPoints(object sender, RoutedEventArgs e)
        {
            // クリックしたボタンのデータ取得
            Button b__ = (Button)sender;


            // mcidを表示させる
            mcid.Text = b__.Content.ToString();


            // SCを取得
            int sc__ = page1[b__.Content.ToString()].getSc_();

            // CPを取得
            int cp__ = page1[b__.Content.ToString()].getContribution_();


            // 取得したポイントを表示させる
            SC.Text = ConvertNumber(sc__);
            Contribution.Text = ConvertNumber(cp__);


            // このデータのコピーを取る
            scpoint_ = sc__;
            contributionpoint_ = cp__;

        }

        private async void ChangeSC(object sender, DoubleTappedRoutedEventArgs e)
        {
            // 入力した数字を取得する
            string text = await InputTextDialogAsync("Change SC");


            // 取得した数字を元の数字に足して再表示する
            int d;
            if (int.TryParse(text, out d))
            {
                // operators_がtrueだった場合数字を足す。operators_がfalseだった場合数字を引く
                if (operators_ == true)
                {
                    // ポイントの合計を計算して取得
                    int total__ = scpoint_ + d;
                    SC.Text = ConvertNumber(total__);


                    // データベースに新しくデータを更新して、scpointも更新する
                    page1[mcid.Text].setSc_(total__);
                    scpoint_ = total__;


                    // データ編集をしたことを記録する
                    if (isEdit) return;

                    isEdit = true;
                } else
                {
                    // ポイントの合計を計算して取得
                    int total__ = scpoint_ - d;
                    SC.Text = ConvertNumber(total__);


                    // データベースに新しくデータを更新して、scpointも更新する
                    page1[mcid.Text].setSc_(total__);
                    scpoint_ = total__;


                    // データ編集をしたことを記録する
                    if (isEdit) return;

                    isEdit = true;
                }
            }
        }


        private async void ChangeContribution(object sender, DoubleTappedRoutedEventArgs e)
        {
            // 入力した数字を取得する
            string text = await InputTextDialogAsync("Change Contribution");


            // 取得した数字を元の数字に足して再表示する
            int d;
            if (int.TryParse(text, out d))
            {
                // operators_がtrueだった場合数字を足す。operators_がfalseだった場合数字を引く
                if (operators_ == true)
                {
                    // ポイントの合計を計算して取得
                    int total__ = contributionpoint_ + d;
                    Contribution.Text = ConvertNumber(total__);


                    // データベースに新しくデータを更新して、scpointも更新する
                    page1[mcid.Text].setContribution_(total__);
                    contributionpoint_ = total__;


                    // データ編集をしたことを記録する
                    if (isEdit) return;

                    isEdit = true;
                }
                else
                {
                    // ポイントの合計を計算して取得
                    int total__ = contributionpoint_ - d;
                    Contribution.Text = ConvertNumber(total__);


                    // データベースに新しくデータを更新して、scpointも更新する
                    page1[mcid.Text].setContribution_(total__);
                    contributionpoint_ = total__;


                    // データ編集をしたことを記録する
                    if (isEdit) return;

                    isEdit = true;
                }

            }

        }

        // ポイント設定に使うダイアログクラスを定義する
        private async Task<string> InputTextDialogAsync(string title)
        {
            // テキストボックスの中身を定義
            TextBox inputTextBox = new TextBox()
            {
                AcceptsReturn = false,

                Height = 32,
            };


            // ダイアログの中身を定義
            ContentDialog dialog = new ContentDialog()
            {
                Content = inputTextBox,

                Title = title,

                IsSecondaryButtonEnabled = true,

                PrimaryButtonText = "Plus",

                SecondaryButtonText = "Minus",

                CloseButtonText = "Cancel"
            };


            // Plusボタンを押した場合はoperators_をtruenに、Minusボタンを押した場合はoperators_をfalseにする
            switch (await dialog.ShowAsync())
            {
                case ContentDialogResult.Primary:
                    // テキストの中身が数字だった場合そのままその数字を返す
                    int d;
                    if (int.TryParse(inputTextBox.Text, out d))
                    {
                        operators_ = true;
                        return d.ToString();
                        break;
                    }
                    else
                    {
                        return "";
                        break;
                    }
                case ContentDialogResult.Secondary:
                    // テキストの中身が数字だった場合そのままその数字を返す
                    int d2;
                    if (int.TryParse(inputTextBox.Text, out d2))
                    {
                        operators_ = false;
                        return d2.ToString();
                        break;
                    }
                    else
                    {
                        return "";
                        break;
                    }
                default:
                    return "";
                    break;

            }


        }

        // UIのMCIDを変えるメソッド
        private async void InputMCIDDIalogAsync(string title)
        {
            // テキストボックスの中身を定義
            TextBox inputTextBox = new TextBox
            {
                AcceptsReturn = false,
                Height = 32,
            };


            // ダイアログの中身を定義
            ContentDialog dialog = new ContentDialog
            {
                Content = inputTextBox,
                Title = title,
                IsSecondaryButtonEnabled = false,
                PrimaryButtonText = "OK",
                CloseButtonText = "Cancel"
            };


            // OKを押したらinputTextBoxに入力した文字をMCIDにする
            if (await dialog.ShowAsync() == ContentDialogResult.Primary)
            {
                // inputTextBoxに何も入力していた場合はキャンセルして、文字を入力していた場合だけ作動するようにする
                if (inputTextBox.Text != null)
                {
                    ChangeStyle(mcid.Text, inputTextBox.Text);


                    // データとmcidのテキストを入れ替える
                    Points deta = page1[mcid.Text];
                    page1.Remove(mcid.Text);
                    page1.Add(inputTextBox.Text, deta);


                    mcid.Text = inputTextBox.Text;


                    // データ編集をしたことを記録する
                    if (isEdit) return;

                    isEdit = true;
                }
            }
            else
            {

            }
        }

        private async void ChangeMCID(object sender, DoubleTappedRoutedEventArgs e)
        {
            InputMCIDDIalogAsync("Change MCID");
        }

        // NameListのボタンの名前を変更する時に使うメソッド
        private void ChangeStyle(string buttonName, string newName)
        {
            // NameListの中にあるボタンの中から名前の合うものだけを取得する
            int count = NameList.Children.Count;
            for (int i = 0; i < count; i++)
            {
                if (NameList.Children[i] is Button && ((Button)NameList.Children[i]).Content.ToString().Equals(buttonName, StringComparison.OrdinalIgnoreCase))
                {
                    // ボタンデータを取得したら新しいデータに書き換える
                    ((Button)NameList.Children[i]).Name = newName;
                    ((Button)NameList.Children[i]).Content = newName;
                }
            }


            /*
            Button myButton = (from child in NameList.Children where child is Button && ((Button)child).Name.Equals(buttonName, StringComparison.OrdinalIgnoreCase) select (Button)child).FirstOrDefault();

            // 取得したボタンからデータを変更する
            myButton.Name = newName;
            myButton.Content = newName;
            */


        }

        // データをファイルにセーブするメソッド
        private async void saveFile()
        {
            // ファイルに入れるデータを宣言する
            string text = "１行目\r\n";

            // 実際にそのデータをファイルに入れる

        }

        // データファイルを作るメソッド
        private async void createFile()
        {

            // データを保存するフォルダーを作る
            var projectFolderName = "sukkaproject";

            // ライブラリーフォルダーに既にフォルダーが作成されてるかどうか確認する
            if (await isFilePresent(KnownFolders.DocumentsLibrary, projectFolderName))
            {
                Debug.WriteLine("This folder already exists");

                var item = await KnownFolders.DocumentsLibrary.GetFolderAsync(projectFolderName);

                // ファイルが既にフォルダーに存在するか確認する
                if (await isFilePresent(item, "sukka.txt"))
                {
                    Debug.WriteLine("This file already exists");
                }
                else
                {
                    // データを保存するファイル(.txt)をフォルダーの中に作る
                    StorageFile sampleFile = await item.CreateFileAsync("sukka.txt", CreationCollisionOption.OpenIfExists);

                    /*
                    // 作ったファイルを他のメソッドでも使えるようにデータを取る
                    sukkaFile = sampleFile;
                    */

                    var listOfStrings = new List<string> { "line1", "line2", "line3" };

                    await FileIO.AppendLinesAsync(sampleFile, listOfStrings);

                }
            }
            else
            {
                // ファイル保存用のフォルダー作成
                StorageFolder projectFolder = await KnownFolders.DocumentsLibrary.CreateFolderAsync(projectFolderName);

                // ファイルが既にフォルダーに存在するか確認する
                if (await isFilePresent(projectFolder, "sukka.txt"))
                {
                    Debug.WriteLine("This file already exists");
                }
                else
                {
                    // データを保存するファイル(.txt)をフォルダーの中に作る
                    StorageFile sampleFile = await projectFolder.CreateFileAsync("sukka.txt", CreationCollisionOption.OpenIfExists);

                    /*
                    // 作ったファイルを他のメソッドでも使えるようにデータを取る
                    sukkaFile = sampleFile;
                    */

                    var listOfStrings = new List<string> { "line1", "line2", "line3" };

                    await FileIO.AppendLinesAsync(sampleFile, listOfStrings);

                }
            }

        }

        // ファイルが既に存在するかを確認するメソッド
        private async Task<bool> isFilePresent(StorageFolder folder, string fileName)
        {
            // 指定したフォルダーからファイルを取得
            var item = await folder.TryGetItemAsync(fileName);
            return item != null;


        }

        private void createPlayerData(string mcid, int sc, int contribution)
        {
            // 保存先のファイルを取得
            ApplicationDataContainer database = ApplicationData.Current.LocalSettings;

            var playerContainer = database.Containers["key"];


            // 保存したいデータ作成
            ApplicationDataCompositeValue data = new ApplicationDataCompositeValue();

            data["SC"] = sc;
            data["Contribution"] = contribution;


            // 作ったデータをファイルに入れる
            playerContainer.Values[mcid] = data;
        }

        // 指定したmcidに対応するデータを編集するメソッド
        private void EditPlayerData(string mcid, int sc, int contribution)
        {
            ApplicationDataContainer database = ApplicationData.Current.LocalSettings;

            var playerContainer = database.Containers["key"];


            // mcidに対応したデータを編集する
            ApplicationDataCompositeValue playerdata = getPlayerData(mcid);

            playerdata["SC"] = sc;
            playerdata["Contribution"] = contribution;


            // 編集したデータを格納する
            playerContainer.Values[mcid] = playerdata;
        }

        private void TCreatePlayerData(string mcid, int sc, int contribution, int id)
        {
            // 保存先のファイルを取得
            ApplicationDataContainer database = ApplicationData.Current.LocalSettings;

            var playerContainer = database.Containers["key"];


            // もし既にプレイヤーデータが作成されていた場合
            if (isPlayerPresent(mcid))
            {
                // mcidに対応したデータを編集する
                ApplicationDataCompositeValue playerdata = getPlayerData(mcid);

                playerdata["SC"] = sc;
                playerdata["Contribution"] = contribution;
                playerdata["ID"] = id;


                // 編集したデータを格納する
                playerContainer.Values[mcid] = playerdata;
            }


            // もしプレイヤーデータが未作成の場合
            if (!isPlayerPresent(mcid))
            {
                // 保存したいデータ作成
                ApplicationDataCompositeValue playerdata = new ApplicationDataCompositeValue();


                playerdata["SC"] = sc;
                playerdata["Contribution"] = contribution;
                playerdata["ID"] = id;


                // 作ったデータをファイルに入れる
                playerContainer.Values[mcid] = playerdata;
            }
        }


        private void DatabaseInit()
        {
            var database = ApplicationData.Current.LocalSettings;


            // もし既に初期化できていたら何もしない
            if (database.Containers.ContainsKey("key")) return;


            // 初期化できていなかったら初期化をする
            if (!database.Containers.ContainsKey("key"))
            {
                // データファイルを作成
                var playerContainer = database.CreateContainer("key", ApplicationDataCreateDisposition.Always);
            }
        }

        // 指定したプレイヤーデータが既に作られているかどうか確かめるメソッド
        private bool isPlayerPresent(string mcid)
        {
            // 保存先のファイルを取得
            ApplicationDataContainer database = ApplicationData.Current.LocalSettings;
            var playerContainer = database.Containers["key"];


            // ファイルのプレイヤーデータ取得
            ApplicationDataCompositeValue data = (ApplicationDataCompositeValue)playerContainer.Values[mcid];


            // もしプレイヤーデータが存在したら、trueそうでなかったら、false
            if (data != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        // 指定したmcidに対応するデータを取得するメソッド
        private ApplicationDataCompositeValue getPlayerData(string mcid)
        {
            // databaseを取得
            ApplicationDataContainer database = ApplicationData.Current.LocalSettings;
            var playerContainer = database.Containers["key"];


            // 指定したプレイヤーのデータがあるかどうか調べる
            if (isPlayerPresent(mcid))
            {
                return (ApplicationDataCompositeValue)playerContainer.Values[mcid];
            }
            else
            {
                return null;
            }
        }

        // NameListにボタンをロードする
        private void LoadPlayerData()
        {
            ApplicationDataContainer database = ApplicationData.Current.LocalSettings;
            var playerContainer = database.Containers["key"];


            // データの中にある全てのMCIDを取得する
            foreach (string mcid in sortList())
            {
                // mcidがSample、Add、nullではなかったら、ボタンをロードする
                if (!mcid.Equals("Sample") && !mcid.Equals("Add"))
                {
                    try
                    {
                        // ボタンの中身を定義
                        Button button = new Button
                        {
                            Content = mcid,

                            FontSize = 60,
                            FontFamily = new FontFamily("Arial"),

                            Background = null,

                            Width = 465,
                            Height = 87,

                            HorizontalAlignment = HorizontalAlignment.Center

                        };


                        // ボタンにクリックイベントを追加
                        button.Click += ShowPoints;

                        // カウントのセット
                        count_ = countKeys() - 1;

                        buttoncount = countKeys();


                        // Addボタンデータ取得
                        Button addbutton = Add;


                        //一度Addボタンを付けなおして位置を直す
                        NameList.Children.Remove(Add);

                        NameList.Children.Add(button);

                        NameList.Children.Add(addbutton);


                        RelativePanel.SetAlignHorizontalCenterWith(button, addbutton);


                        // データベースからプレイヤーのデータを取り出す
                        ApplicationDataCompositeValue playerdatafromdatabase = getPlayerData(mcid);

                        int scpoint = (int)playerdatafromdatabase["SC"];
                        int contributionpoint = (int)playerdatafromdatabase["Contribution"];
                        int id = (int)playerdatafromdatabase["ID"];

                        // ページに入れるデータを宣言
                        Points data = new Points(scpoint, contributionpoint, id);

                        page1.Add(button.Content.ToString(), data);
                    }
                    catch (Exception e)
                    {
                        return;
                    }

                }
            }
        }

        private string[] sortList()
        {
            // 並び変えたいstring型のlistを定義
            var sortedList = new string[countKeys()];


            // データの中の一つ一つに入れたIDに対応した場所に並び変える
            foreach (string keys in getKeys())
            {
                var playerData = getPlayerData(keys);
                sortedList[(int)playerData["ID"]] = keys;
            }


            return sortedList;
        }

        // プレイヤーデータを全て削除するメソッド
        private void DeleteAllPlayerData()
        {
            ApplicationDataContainer database = ApplicationData.Current.LocalSettings;


            // データを削除
            database.DeleteContainer("key");
        }

        // データベースにあるキー（mcid）を全て取得する
        private List<string> getKeys()
        {
            ApplicationDataContainer database = ApplicationData.Current.LocalSettings;
            var playerContainer = database.Containers["key"];


            // キーが一つ以上あった場合
            if (playerContainer.Values.Keys != null)
            {
                // キーが入ったリストを作る
                List<string> keys = playerContainer.Values.Keys.ToList<string>();


                return keys;
            }
            else
            {
                return null;
            }
        }

        // データベースにあるキー（mcid）の数を取得する
        private int countKeys()
        {
            ApplicationDataContainer database = ApplicationData.Current.LocalSettings;
            var playerContainer = database.Containers["key"];


            return playerContainer.Values.Keys.Count;
        }

        // データセーブイベント
        private async void Click_To_Save(object sender, RoutedEventArgs e)
        {
            // セーブのログを出す
            await ShowSaveDialog();


            ApplicationDataContainer database = ApplicationData.Current.LocalSettings;

            var playerContainer = database.Containers["key"];


            Debug.WriteLine(playerContainer.Values);


            // 全てのボタンを取得
            foreach (Button button in getAllButtonFromNameList())
            {
                string playername = button.Content.ToString();


                // Addボタンが入っていた場合、無視する
                if (playername.Equals("Add")) return;


                // プレイヤーデータ取得
                int sc = page1[playername].getSc_();

                int contribution = page1[playername].getContribution_();

                int id = page1[playername].getId_();


                // データをセーブする
                TCreatePlayerData(playername, sc, contribution, id);
            }
        }

        // NameListの中にあるボタンだけを取り出す
        private List<Button> getAllButtonFromNameList()
        {
            var buttons = NameList.Children.OfType<Button>().ToList();

            buttons.Remove(Add);


            return buttons;
        }

        // セーブした時のログを出すメソッド
        private async Task<string> ShowSaveDialog()
        {
            // ダイアログの中身を定義
            ContentDialog dialog = new ContentDialog
            {
                Content = "You have saved the data",
                Title = "Save Log",
                IsSecondaryButtonEnabled = false,
                CloseButtonText = "OK"
            };


            // ログを出す
            ContentDialogResult result = await dialog.ShowAsync();


            return "";

        }

        // 1000を1kにするメソッド
        private string ConvertNumber(int num)
        {
            if (num >= 1000000)
            {
                // 1000000を1にして、"m"を足す
                string convertedNumber = Math.Round(num / 1000000f, 1).ToString() + "M";


                return convertedNumber;
            }
            else if (num >= 1000)
            {
                // 1000を1にして、"k"を足す
                string convertedNumber = Math.Round(num / 1000f, 1).ToString() + "K";


                return convertedNumber;
            }
            else
            {
                return num.ToString();
            }
        }

        // ポイントリスト表示用のウィンドウ作成
        private async void createPlayerDataWindow()
        {
            // PlayerDataウィンドウを持ってくる
            var frame = new Frame();

            AppWindow appWindow = await AppWindow.TryCreateAsync();

            frame.Navigate(typeof(PlayerData));
            
            ElementCompositionPreview.SetAppWindowContent(appWindow, frame);


            // 新しいウィンドウの表示
            await appWindow.TryShowAsync();
        }

        private void ShowPlayerDataGridButton(object sender, RoutedEventArgs e)
        {
            // PlayerDataGridを表示する
            createPlayerDataWindow();
        }
    }
}
