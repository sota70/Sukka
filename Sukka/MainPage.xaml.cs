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
using Windows.UI.Core.Preview;
using Windows.UI.Popups;
using Windows.UI.Core;
using Windows.ApplicationModel.Core;
using Windows.UI.ViewManagement;
using Windows.Storage;
// 空白ページの項目テンプレートについては、https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x411 を参照してください

namespace Sukka
{

    /// <summary>
    /// それ自体で使用できる空白ページまたはフレーム内に移動できる空白ページ。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private int dataTotal = 0;

        private int buttonTotal = 0;


        // 数値をUIから変更できるようにするために作った
        private int scpoint_copy;
        private int contributionpoint_copy;

        private bool isPlus;

        // アプリを開いてからデータを１回でも編集したかどうかの確認用に作った
        public bool EditOnce = false;


        private static MainPage instance;


        public MainPage()
        {
            this.InitializeComponent();


            Points sample__ = new Points(0, 0, 0);

            DictionaryManager.setPlayerData("Sample", sample__);

            buttonTotal += 1;


            DatabaseInit();


            LoadPlayerData();

            createTestSampleWindow();


            instance = this;
        }

        // PlayerDataクラスでもMainPageのメソッドを使いたいから作った
        public static MainPage returnInstance()
        {
            return instance;
        }

        private void AddData(object sender, RoutedEventArgs e)
        {
            dataTotal += 1;

            buttonTotal += 1;


            Button NewButton = new Button()
            {
                Content             = "Sample" + buttonTotal,
                FontSize            = 60,
                FontFamily          = new FontFamily("Arial"),
                Background          = null,
                Width               = 465,
                Height              = 87,
                HorizontalAlignment = HorizontalAlignment.Center
            };

            // ※補足 NewButton.Click += ShowPoints -> ボタンにクリックイベントを追加するやつ
            NewButton.Click += ShowPoints;


            NewButton.HorizontalAlignment = HorizontalAlignment.Center;


            RelativePanel.SetAlignHorizontalCenterWith(NewButton, sender);


            Button addbutton = Add;


            NameList.Children.Remove(Add);

            NameList.Children.Add(NewButton);

            NameList.Children.Add(addbutton);


            Points playerData = new Points(1, 1, dataTotal);

            string playername = NewButton.Content.ToString();

            DictionaryManager.setPlayerData(playername, playerData);


            if(EditOnce == false)
            {
                EditOnce = true;
            }
        }

        private void ShowPoints(object clickedButton, RoutedEventArgs e)
        {
            Button b__ = (Button)clickedButton;


            // mcid（playername）を表示させる
            mcid.Text = b__.Content.ToString();

            var playerdata = DictionaryManager.getDictionary();

            int scpoint = playerdata[b__.Content.ToString()].getSc_();
            int cppoint = playerdata[b__.Content.ToString()].getContribution_();

            string convertedSCPoint = ConvertNumber(scpoint);
            string convertedCPPoint = ConvertNumber(cppoint);


            // UIにポイントを表示させる
            SC.Text = convertedSCPoint;
            Contribution.Text = convertedCPPoint;


            scpoint_copy = scpoint;
            contributionpoint_copy = cppoint;

        }

        private async void ChangeSC(object sender, DoubleTappedRoutedEventArgs e)
        {
            string orderedPoints = await InputPointsDialogAsync("Change SC");


            // ※補足 int.TryParse(orderedPoints, out d)) -> 取得した数字を元の数字に足して再表示する
            int d;
            if (int.TryParse(orderedPoints, out d))
            {
                if (isPlus)
                {
                    int totalpoints = scpoint_copy + d;
                    SC.Text = ConvertNumber(totalpoints);

                    var playerdata = DictionaryManager.getDictionary();

                    playerdata[mcid.Text].setSc_(totalpoints);
                    scpoint_copy = totalpoints;


                    if(EditOnce == false)
                    {
                        EditOnce = true;
                    }
                } else
                {
                    var playerdata = DictionaryManager.getDictionary();

                    int totalpoints = scpoint_copy - d;
                    SC.Text = ConvertNumber(totalpoints);


                    playerdata[mcid.Text].setSc_(totalpoints);
                    scpoint_copy = totalpoints;


                    if(EditOnce == false)
                    {
                        EditOnce = true;
                    }
                }
            }
        }


        private async void ChangeContribution(object sender, DoubleTappedRoutedEventArgs e)
        {
            string orderedPoints = await InputPointsDialogAsync("Change Contribution");


            // ※補足 int.TryParse(orderedPoints, out d)) -> string型からint型へ変換する
            int d;
            if (int.TryParse(orderedPoints, out d))
            {

                var playerdata = DictionaryManager.getDictionary();

                if (isPlus)
                {
                    int totalpoints = contributionpoint_copy + d;
                    Contribution.Text = ConvertNumber(totalpoints);


                    playerdata[mcid.Text].setContribution_(totalpoints);
                    contributionpoint_copy = totalpoints;


                   if(EditOnce == false)
                    {
                        EditOnce = true;
                    }
                }
                else
                {
                    int totalpoints = contributionpoint_copy - d;
                    Contribution.Text = ConvertNumber(totalpoints);


                    playerdata[mcid.Text].setContribution_(totalpoints);
                    contributionpoint_copy = totalpoints;


                    if(EditOnce == false)
                    {
                        EditOnce = true;
                    }
                }

            }

        }

        private async Task<string> InputPointsDialogAsync(string title)
        {
            TextBox inputTextBox = new TextBox()
            {
                AcceptsReturn = false,

                Height = 32,
            };


            ContentDialog dialog = new ContentDialog()
            {
                Content = inputTextBox,

                Title = title,

                IsSecondaryButtonEnabled = true,

                PrimaryButtonText = "Plus",

                SecondaryButtonText = "Minus",

                CloseButtonText = "Cancel"
            };

            var result = await dialog.ShowAsync();

            switch (result)
            {
                case ContentDialogResult.Primary:
                    // ※補足 int.TryParse(inputTextBox.Text, out d)) -> string型からint型へ変換する
                    int parseResult;

                    if (int.TryParse(inputTextBox.Text, out parseResult))
                    {
                        isPlus = true;

                        return parseResult.ToString();
                    }
                    else
                    {
                        return "";
                    }
                case ContentDialogResult.Secondary:
                    int parseResult2;

                    if (int.TryParse(inputTextBox.Text, out parseResult2))
                    {
                        isPlus = false;

                        return parseResult2.ToString();
                    }
                    else
                    {
                        return "";
                    }
                default:
                    return "";

            }


        }

        private async void InputMCIDDIalogAsync(string title)
        {
            TextBox inputTextBox = new TextBox
            {
                AcceptsReturn = false,
                Height = 32,
            };


            ContentDialog dialog = new ContentDialog
            {
                Content = inputTextBox,
                Title = title,
                IsSecondaryButtonEnabled = false,
                PrimaryButtonText = "OK",
                CloseButtonText = "Cancel"
            };

            var result = await dialog.ShowAsync();

            var OK = ContentDialogResult.Primary;

            if (result == OK)
            {
                if (inputTextBox.Text == null) return;


                ChangeStyle(mcid.Text, inputTextBox.Text);


                // データとmcidのテキストを入れ替える
                var playerdata = DictionaryManager.getDictionary();

                Points data = playerdata[mcid.Text];
                playerdata.Remove(mcid.Text);
                playerdata.Add(inputTextBox.Text, data);

                DictionaryManager.setPlayerData(mcid.Text, data);


                mcid.Text = inputTextBox.Text;


                if (EditOnce) return;

                EditOnce = true;
            }
        }

        private void ChangeMCID(object sender, DoubleTappedRoutedEventArgs e)
        {
            InputMCIDDIalogAsync("Change MCID");
        }

        private void ChangeStyle(string buttonName, string newName)
        {
            int buttonCount = NameList.Children.Count;

            for (int i = 0; i < buttonCount; i++)
            {
                bool conditionsForChangingStyle = NameList.Children[i] is Button && ((Button)NameList.Children[i]).Content.ToString().Equals(buttonName, StringComparison.OrdinalIgnoreCase);

                if (conditionsForChangingStyle)
                {
                    ((Button)NameList.Children[i]).Name    = newName;
                    ((Button)NameList.Children[i]).Content = newName;
                }
            }
        }

        private void createPlayerData(string mcid, int sc, int contribution)
        {
            // ※補足 ApplicationDataContainer, database.Containers["key"] -> アプリのファイル内のデータベースにアクセス
            ApplicationDataContainer database = ApplicationData.Current.LocalSettings;

            var playerContainer = database.Containers["key"];


            // ※補足 ApplicationDataCompositeValue -> アプリのファイル内のデータベースに入れる用のデータの型
            ApplicationDataCompositeValue data = new ApplicationDataCompositeValue();

            data["SC"]           = sc;
            data["Contribution"] = contribution;


            playerContainer.Values[mcid] = data;
        }

        private void EditPlayerData(string mcid, int sc, int contribution)
        {
            ApplicationDataContainer database = ApplicationData.Current.LocalSettings;

            var playerContainer = database.Containers["key"];


            ApplicationDataCompositeValue playerdata = getPlayerData(mcid);

            playerdata["SC"]           = sc;
            playerdata["Contribution"] = contribution;


            playerContainer.Values[mcid] = playerdata;
        }

        private void TCreatePlayerData(string mcid, int sc, int contribution, int id)
        {
            ApplicationDataContainer database = ApplicationData.Current.LocalSettings;

            var playerContainer = database.Containers["key"];


            if (isPlayerPresent(mcid))
            {
                ApplicationDataCompositeValue playerdata = getPlayerData(mcid);

                playerdata["SC"]           = sc;
                playerdata["Contribution"] = contribution;
                playerdata["ID"]           = id;


                playerContainer.Values[mcid] = playerdata;
            }
            else if (!isPlayerPresent(mcid))
            {
                ApplicationDataCompositeValue playerdata = new ApplicationDataCompositeValue();


                playerdata["SC"]           = sc;
                playerdata["Contribution"] = contribution;
                playerdata["ID"]           = id;


                playerContainer.Values[mcid] = playerdata;
            }
        }


        private void DatabaseInit()
        {
            var database = ApplicationData.Current.LocalSettings;


            // ※補足 dastabase.Containers.ContainsKey("key") -> データコンテイナーが作成されてるかどうかを調べる
            if (database.Containers.ContainsKey("key")) return;


            if (!database.Containers.ContainsKey("key"))
            {
                var playerContainer = database.CreateContainer("key", ApplicationDataCreateDisposition.Always);
            }
        }

        private bool isPlayerPresent(string mcid)
        {
            ApplicationDataContainer database = ApplicationData.Current.LocalSettings;

            var playerContainer = database.Containers["key"];


            ApplicationDataCompositeValue data = (ApplicationDataCompositeValue)playerContainer.Values[mcid];


            bool isPlayerExists = data != null;

            if (isPlayerExists)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private ApplicationDataCompositeValue getPlayerData(string mcid)
        {
            ApplicationDataContainer database = ApplicationData.Current.LocalSettings;

            var playerContainer = database.Containers["key"];


            if (isPlayerPresent(mcid))
            {
                var playerdata = (ApplicationDataCompositeValue)playerContainer.Values[mcid];

                return playerdata;
            }
            else
            {
                return null;
            }
        }

        private void LoadPlayerData()
        {
            ApplicationDataContainer database = ApplicationData.Current.LocalSettings;

            var playerContainer = database.Containers["key"];


            // ※補足 foreach (string mcid in sortList()) -> データの中にある全てのMCIDを取得する
            foreach (string mcid in sortList())
            {
                // なぜかデータからnullデータが出てしまうため、回避策としてreturnで返している
                if (mcid == null) return;

                bool mcidCondition = !mcid.Equals("Sample") && !mcid.Equals("Add");

                if (mcidCondition)
                {
                    try
                    {
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


                        // ※補足 button.Click += ShowPoints -> ボタンにクリックイベントを追加
                        button.Click += ShowPoints;


                        dataTotal = countKeys() - 1;

                        buttonTotal = countKeys();


                        Button addbutton = Add;


                        // 一度Addボタンを付けなおして位置を直す
                        NameList.Children.Remove(Add);

                        NameList.Children.Add(button);

                        NameList.Children.Add(addbutton);


                        RelativePanel.SetAlignHorizontalCenterWith(button, addbutton);


                        ApplicationDataCompositeValue playerdatafromdatabase = getPlayerData(mcid);

                        int scpoint           = (int)playerdatafromdatabase["SC"];
                        int contributionpoint = (int)playerdatafromdatabase["Contribution"];
                        int id                = (int)playerdatafromdatabase["ID"];

                        Points data = new Points(scpoint, contributionpoint, id);

                        DictionaryManager.setPlayerData(button.Content.ToString(), data);
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
            var sortedList = new string[countKeys()];


            foreach (string playerNamesInData in getKeys())
            {
                var playerData = getPlayerData(playerNamesInData);

                sortedList[(int)playerData["ID"]] = playerNamesInData;
            }


            return sortedList;
        }

        private void DeleteAllPlayerData()
        {
            ApplicationDataContainer database = ApplicationData.Current.LocalSettings;


            // ※補足 database.DeleteContainer("key") -> データベースにあるデータを全部消す
            database.DeleteContainer("key");
        }

        private List<string> getKeys()
        {
            ApplicationDataContainer database = ApplicationData.Current.LocalSettings;
            var playerContainer = database.Containers["key"];


            var allKeysInDataBase = playerContainer.Values.Keys;

            if (allKeysInDataBase != null)
            {
                List<string> keysList = allKeysInDataBase.ToList<string>();


                return keysList;
            }
            else
            {
                return null;
            }
        }

        private int countKeys()
        {
            ApplicationDataContainer database = ApplicationData.Current.LocalSettings;
            var playerContainer = database.Containers["key"];

            int keysTotal = playerContainer.Values.Keys.Count;


            return keysTotal;
        }

        private async void Click_To_Save(object sender, RoutedEventArgs e)
        {
            await ShowSaveDialog();

            saveAllData();

            EditOnce = false;
        }

        // 他のクラスでも使うためにメソッドとして作った
        public void saveAllData()
        {
            foreach (Button button in getAllButtonFromNameList())
            {
                string playername = button.Content.ToString();


                // 元からUIに存在するボタンを除くために作った
                if (playername.Equals("Add")) return;


                var playerdata = DictionaryManager.getDictionary();

                int sc           = playerdata[playername].getSc_();
                int contribution = playerdata[playername].getContribution_();
                int id           = playerdata[playername].getId_();


                TCreatePlayerData(playername, sc, contribution, id);
            }
        }

        public List<Button> getAllButtonFromNameList()
        {
            var allButtons = NameList.Children.OfType<Button>().ToList();

            // 余計なボタンを消すための処理
            allButtons.Remove(Add);


            return allButtons;
        }

        private async Task<string> ShowSaveDialog()
        {
            ContentDialog dialog = new ContentDialog
            {
                Content = "You have saved the data",
                Title = "Save Log",
                IsSecondaryButtonEnabled = false,
                CloseButtonText = "OK"
            };


            ContentDialogResult showDialog = await dialog.ShowAsync();

            return "";
        }

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

        // １回開いたら２回目以降開く時はWindowをリフレッシュさせるようにするために必要だからAppWindowをここに定義した
        private AppWindow playerDataWindow;

        private async void createPlayerDataWindow()
        {
            if(playerDataWindow == null)
            {
                // ※補足 playerDataFrame.Navigate() -> PlayerDataのコンテンツを持ってくるメソッド
                //        ElementCompositionPreview.SetAppWindowContent() -> Windowにコンテンツを設置するメソッド
                //        playerDataWindow.Closed += delegate -> ウィンドウが閉まった時にウィンドウにあるコンテンツインスタンスを消すメソッド

                playerDataWindow = await AppWindow.TryCreateAsync();

                Frame playerDataFrame = new Frame();

                playerDataFrame.Navigate(typeof(PlayerData));

                ElementCompositionPreview.SetAppWindowContent(playerDataWindow, playerDataFrame);


                await playerDataWindow.TryShowAsync();


                playerDataWindow.Closed += delegate
                {
                    playerDataFrame.Content = null;

                    playerDataWindow = null;
                };
            }
            else
            {
                Frame playerDataFrame = new Frame();

                playerDataFrame.Navigate(typeof(PlayerData));

                ElementCompositionPreview.SetAppWindowContent(playerDataWindow, playerDataFrame);


                await playerDataWindow.TryShowAsync();


                playerDataWindow.Closed += delegate
                {
                    playerDataFrame.Content = null;

                    playerDataWindow = null;
                };
            }
        }

        private AppWindow TestSampleWindow;

        private async void createTestSampleWindow()
        {
            if (TestSampleWindow == null)
            {
                // ※補足 playerDataFrame.Navigate() -> PlayerDataのコンテンツを持ってくるメソッド
                //        ElementCompositionPreview.SetAppWindowContent() -> Windowにコンテンツを設置するメソッド
                //        playerDataWindow.Closed += delegate -> ウィンドウが閉まった時にウィンドウにあるコンテンツインスタンスを消すメソッド

                TestSampleWindow = await AppWindow.TryCreateAsync();

                Frame TestSampleFrame = new Frame();

                TestSampleFrame.Navigate(typeof(testsample));

                ElementCompositionPreview.SetAppWindowContent(TestSampleWindow, TestSampleFrame);


                await TestSampleWindow.TryShowAsync();


                TestSampleWindow.Closed += delegate
                {
                    TestSampleFrame.Content = null;

                    TestSampleWindow = null;
                };
            }
            else
            {
                Frame TestSampleFrame = new Frame();

                TestSampleFrame.Navigate(typeof(testsample));

                ElementCompositionPreview.SetAppWindowContent(TestSampleWindow, TestSampleFrame);


                await TestSampleWindow.TryShowAsync();


                TestSampleWindow.Closed += delegate
                {
                    TestSampleFrame.Content = null;

                    TestSampleWindow = null;
                };
            }
        }

        private void ShowPlayerDataGridButton(object sender, RoutedEventArgs e)
        {
            createPlayerDataWindow();
        }
    }
}
