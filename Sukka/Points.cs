using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sukka
{
    class Points
    {
        // データに入れるポイントの変数宣言
        protected int sc_;
        protected int contribution_;
        protected int id_;

        // コンストラクタ
        public Points(int sc_, int contribution_, int id_)
        {
            this.sc_ = sc_;
            this.contribution_ = contribution_;
            this.id_ = id_;
        }

        // sc_のセッター
        public void setSc_(int sc_)
        {
            this.sc_ = sc_;
        }

        // contribution_のセッター
        public void setContribution_(int contribution_)
        {
            this.contribution_ = contribution_;
        }

        // id_のセッター
        public void setId_(int id_)
        {
            this.id_ = id_;
        }

        // sc_のゲッター
        public int getSc_()
        {
            return sc_;
        }

        // contribution_のゲッター
        public int getContribution_()
        {
            return contribution_;
        }

        // id_のゲッター
        public int getId_()
        {
            return id_;
        }
    }
}
