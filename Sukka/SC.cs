using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sukka
{
    class SC
    {
        // ポイントマップの宣言
        private Dictionary<string, Points> data = new Dictionary<string, Points>();

        // マップにデータを入れる
        public void setData(string mcid, int sc, int contribution, int id)
        {
            // マップに入れるためのデータクラス宣言
            Points p__ = new Points(sc, contribution, id);

            // マップにそのデータクラスを入れる
            data.Add(mcid, p__);
        }

        // マップにあるデータを取得する
        public Points getPoints(string mcid)
        {
            return data[mcid];
        }
    }
}
