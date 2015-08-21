using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void UrlDecodeTest()
        {
            const string fileName = "12%3.jpg";
            string result = "12%253.jpg";
            Assert.IsFalse(System.Uri.IsWellFormedUriString(fileName, UriKind.RelativeOrAbsolute));

            Assert.AreEqual(System.Web.HttpUtility.UrlEncode(fileName), result);

            Assert.AreEqual(System.Web.HttpUtility.UrlEncode("8f-92_3dd.fps"), "8f-92_3dd.fps");

            Assert.IsTrue(fileName.IndexOf('.') == 4);

            Assert.AreEqual(fileName.Substring(fileName.IndexOf('.')), ".jpg");


        }

        [TestMethod]
        public void SubstringTest() 
        {
            const string str = "abcdefg";

            Assert.AreEqual(str.Substring(0,2),"ab");
            Assert.AreEqual(str.Substring(str.Length-3), "efg");

        }

        [TestMethod]
        public void EnumTest()
        {
            List<Bank> banks = new List<Bank>() 
            {
                new Bank(){ BankId=1,BankName="a",BankType=1},
                new Bank(){ BankId=2,BankName="b",BankType=2},
                new Bank(){ BankId=3,BankName="c",BankType=3},
                new Bank(){ BankId=4,BankName="d",BankType=4},
                new Bank(){ BankId=5,BankName="e",BankType=5},
                new Bank(){ BankId=6,BankName="f",BankType=6},
                new Bank(){ BankId=6,BankName="g",BankType=7}
            };

            var res = banks.Where(a => a.BankType == (int)(BankType.b2b | BankType.b2c));
            Assert.AreEqual<int>(res.Count(), 1);
        }

        [Flags]
        public enum BankType :int
        {
            b2b=1,
            b2c=2
        }

        public class Bank
        {
            public int BankId { get; set; }
            public string BankName { get; set; }
            public int BankType { get; set; }
        }

        [TestMethod]
        public void PowerTest() 
        {
            List<Role> roles = new List<Role>() {
                new Role(){RoleId=1,RoleName="admin",RolePower=7},
                new Role(){RoleId=1,RoleName="user",RolePower=5},
                new Role(){RoleId=1,RoleName="guest",RolePower=4},
            };
            PowerType t7 = PowerType.r & PowerType.w & PowerType.x;
            var r = roles.Where(a => (t7 & (PowerType)a.RolePower) == 0);
            Assert.AreEqual<int>(r.Count(), 3);
        }

        [Flags]
        public enum PowerType : int
        {
            r = 1,
            w = 2,
            x = 4
        }

        public class Role
        {
            public int RoleId { get; set; }
            public string RoleName { get; set; }
            public int RolePower { get; set; }
        }
    }
}
