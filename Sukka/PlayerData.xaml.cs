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
using System.Data;
using System.Collections.ObjectModel;
using System.Diagnostics;

// 空白ページの項目テンプレートについては、https://go.microsoft.com/fwlink/?LinkId=234238 を参照してください

namespace Sukka
{
    /// <summary>
    /// それ自体で使用できる空白ページまたはフレーム内に移動できる空白ページ。
    /// </summary>
    public sealed partial class PlayerData : Page
    {
        public PlayerData()
        {
            this.InitializeComponent();

         
            // datagridにデータを挿入
            dataGrid.DataContext = Customer.Customers();
        }

        // SCポイントを高い順に並び変えるフィルターのクリックイベント
        private void FilterSCHigh(object sender, RoutedEventArgs e)
        {
            dataGrid.DataContext = Customer.sortGridData(Customer.DataTags.SCHigh);
        }

        // SCポイントを低い順に並び変えるフィルターのクリックイベント
        private void FilterSCLow(object sender, RoutedEventArgs e)
        {
            dataGrid.DataContext = Customer.sortGridData(Customer.DataTags.SCLow);
        }

        // Contributionポイントを高い順に並び変えるフィルターのクリックイベント
        private void FilterContributionHigh(object sender, RoutedEventArgs e)
        {
            dataGrid.DataContext = Customer.sortGridData(Customer.DataTags.ContributionHigh);
        }

        // Contributionポイントを低い順に並び変えるフィルターのクリックイベント
        private void FilterContributionLow(object sender, RoutedEventArgs e)
        {
            dataGrid.DataContext = Customer.sortGridData(Customer.DataTags.ContributionLow);
        }

        // MCIDをアルファベット順に並び替えるフィルターのクリックイベント
        private void FilterMCID(object sender, RoutedEventArgs e)
        {
            dataGrid.DataContext = Customer.sortGridData(Customer.DataTags.MCID);
        }

        // IDを順番通りに並び替えるフィルターのクリックイベント
        private void FilterID(object sender, RoutedEventArgs e)
        {
            dataGrid.DataContext = Customer.sortGridData(Customer.DataTags.ID);
        }
    }

    // データ管理用クラス作成
    public class Customer
    {
        public string MCID { get; set; }
        public int SC { get; set; }
        public int Contribution { get; set; }
        public int ID { get; set; }


        // DataGridに追加するデータのコピー用
        public static List<Customer> customers;


        public Customer(String mcid, int sc, int contribution, int id)
        {
            this.MCID = mcid;
            this.SC = sc;
            this.Contribution = contribution;
            this.ID = id;
        }

        // DataGridにデータを追加するメソッド
        public static List<Customer> Customers()
        {
            // 新しいリストを作ってそこに一つずつCustomerクラスを入れていく
            var unsortedList = new List<Customer>
            {
                new Customer("sota70", 3, 1, 0),
                new Customer("sukka65536", 2, 1, 1),
                new Customer("sample2", 1, 1, 2),
                new Customer("sample3", 100, 1, 3)
            };


            // DataGridのデータのコピーを取る
            customers = unsortedList;


            return unsortedList;
        }

        // GridDataを並び変えるメソッド
        public static IOrderedEnumerable<Customer> sortGridData(DataTags tag)
        {
            switch(tag)
            {
                case DataTags.MCID:

                    // MCIDをアルファベット順に並び変える
                    return customers.OrderBy(customer => customer.MCID);


                case DataTags.SCHigh:

                    // SCポイントが高い順に並び変える
                    return customers.OrderByDescending(customer => customer.SC);


                case DataTags.SCLow:

                    // SCぽいんとが低い順に並び変える
                    return customers.OrderBy(customer => customer.SC);


                case DataTags.ContributionHigh:

                    // Contributionポイントが高い順に並び変える
                    return customers.OrderByDescending(customer => customer.Contribution);


                case DataTags.ContributionLow:

                    // Contributionポイントが低い順に並び変える
                    return customers.OrderBy(customer => customer.Contribution);


                case DataTags.ID:

                    // IDを１から順番に並び変える
                    return customers.OrderBy(customer => customer.ID);


                default:
                    return null;
            }
        }

        // データタグ
        public enum DataTags
        {
            MCID,
            SCHigh,
            SCLow,
            ContributionHigh,
            ContributionLow,
            ID
        };
    }
}
