using System.Linq;
using System.Collections;

namespace Console
{
    class Program
    {
        static void Main(string[] args)
        {
            //上海市 上海市 浦东新区 206号
            //江西省 南昌市 安义县 软件园20号楼

            //System.Console.WriteLine(GetMunicipalityAddress("江西省 南昌市 安义县 软件园20号楼"));
            //System.Console.WriteLine(GetMunicipalityAddress("上海市 上海市 浦东新区 206号"));

            char n = 'n';
            while (true)
            {
                System.Console.WriteLine("输入内容，回车执行：");
                string _read = System.Console.ReadLine();
                System.Console.WriteLine("处理后的结果为：");
                System.Console.WriteLine(GetMunicipalityAddress2(_read));
                System.Console.WriteLine("输入'y'继续，其他键退出：");
                n = System.Console.ReadKey().KeyChar;
                if (n != 'y')
                {
                    break;
                }
                System.Console.WriteLine();
            }

            System.Console.WriteLine("");
            System.Console.ReadKey();
        }

        /// <summary>
        /// 过滤直辖市地址的重复省市名称
        /// </summary>
        /// <param name="address">完整的地址字符串</param>
        /// <returns>无重复省市名称的地址</returns>
        public static string GetMunicipalityAddress(string address)
        {
            string[] _municipalities = { "上海市", "北京市", "重庆市", "南京市" };
            string _resAddress = address;
            foreach (var item in _municipalities)
            {
                int _sIndex = address.IndexOf(item);
                if (_sIndex > -1)
                {
                    int _eIndex = address.LastIndexOf(item);
                    if (_eIndex > _sIndex)
                    {
                        string _tmpAddress = address.Replace(item, "").Trim();
                        _resAddress = item + " " + _tmpAddress;
                        break;
                    }
                }
                continue;
            }

            return _resAddress;
        }

        public static string GetMunicipalityAddress2(string address)
        {
            string _resAddress = address;
            int _kIndex = address.IndexOf("市");
            if (_kIndex >= 0)
            {
                string _key = address.Substring(0, _kIndex + 1);
                string[] _municipalities = { "上海市", "北京市", "重庆市", "南京市" };
                if (_municipalities.Contains(_key))
                {
                    int _sIndex = address.IndexOf(_key);
                    int _eIndex = address.LastIndexOf(_key);
                    if (_eIndex > _sIndex)
                    {
                        string _tmpAddress = address.Replace(_key, "").Trim();
                        _resAddress = _key + " " + _tmpAddress;
                    }
                }
            }
            return _resAddress;
        }
    }
}
