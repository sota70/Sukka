using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sukka
{
    // playerdataが入ったdictionaryを他のクラスで共有する意図で作った
    class DictionaryManager
    {
        private static Dictionary<string, Points> playerdata = new Dictionary<string, Points>();

        public static Dictionary<string, Points> getDictionary()
        {
            return playerdata;
        }

        public static void setPlayerData(string mcid, Points data)
        {
            playerdata[mcid] = data;
        }
    }
}
