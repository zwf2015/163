        #region public string 升级版加密 二进制排序+时间验证 每秒加密结果都不同
        //升级版加密
        private string MyPlusEnCode(string text)
        {
            //时间戳
            long timestamp = (DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000000;
            int descCounts = (int)(timestamp % 10);//时间戳个位
            int descCounts2 = (int)(timestamp / 10 % 10);//时间戳十位
            descCounts2 += descCounts;//排序次数
            byte[] byteTxt = Encoding.Unicode.GetBytes(text);
            StringBuilder sbBinTxt = new StringBuilder(byteTxt.Length * 8 + 32);
            foreach (byte b in byteTxt)
            {
                sbBinTxt.Append(Convert.ToString(b, 2).PadLeft(8, '0'));
            }
            //第一次排序 txt 次数 descCounts2 
            string txt11 = sbBinTxt.ToString(); //txt11=txt加密前
            string txt12 = desCode(txt11, descCounts2);//txt12=txt11N次加密后


            Random r = new Random();
            int randCount = r.Next(0, 16);
            //第二次排序 txt+time 次数 randCount 
            sbBinTxt = new StringBuilder(txt12);
            string txt22 = sbBinTxt.Append(Convert.ToString(timestamp, 2).PadLeft(32, '0')).ToString();//txt22=txt12+time 加密前
            string txt23 = desCode(txt22, randCount);//txt33=txt22加密后

            //第三次排序 txt+time+randNum 次数 1
            sbBinTxt = new StringBuilder(txt23);
            string rank11 = Convert.ToString(randCount, 2).PadLeft(4, '0');//二进制表示的随机数 4位
            string txt33 = sbBinTxt.Append(rank11).ToString();//txt33=txt23+rand=txt+time+rank
            string txt44 = desCode(txt33, 1);

            //转换为16进制 4个一组
            StringBuilder sbhextxt = new StringBuilder();
            for (int i = 0; i < txt44.Length / 4; i++)
            {
                int isss = Convert.ToInt32(txt44.Substring(i * 4, 4), 2);
                sbhextxt.AppendFormat("{0:x1}", isss);
            }
            return sbhextxt.ToString();
        }
        //根据次数进行排序
        public String desCode(String text, int counts)
        {
            int halfLength = text.Length / 2;
            //StringBuilder sb2 = new StringBuilder(text);
            StringBuilder sb13 = new StringBuilder();
            StringBuilder sb24 = new StringBuilder();
            for (int c = 0; c < counts; c++)
            {
                StringBuilder sb = new StringBuilder(text);
                sb13.Clear(); sb24.Clear();
                //排序
                for (int i = 0; i < halfLength*2; i++)
                {
                    if (i % 2 == 0)
                    {
                        sb24.Append(sb[i]);
                    }
                    else
                    {
                        sb13.Append(sb[i]);
                    }
                }
                //sb2 = sb13.Append(sb24);
                //text = sb2.ToString();
                //sb2 = new StringBuilder(text);
                text = sb13.Append(sb24).ToString();
            }
            return text;
        }
        //根据次数进行反排序
        public string unDesCode(String strtxt, int counts)
        {
            int halfLength = strtxt.Length / 2;
            StringBuilder sb1234 = new StringBuilder();
            for (int c = 0; c < counts; c++)
            {
                sb1234.Clear();
                StringBuilder sb1324 = new StringBuilder(strtxt);
                StringBuilder sb1133 = new StringBuilder(strtxt.Substring(0, halfLength));
                StringBuilder sb2244 = new StringBuilder(strtxt.Substring(halfLength));
                for (int i = 0; i < halfLength; i++)
                {
                    sb1324[i * 2] = sb2244[i];
                    sb1324[i * 2 + 1] = sb1133[i];

                    //sb1234.Append(sb2244[i]);
                    //sb1234.Append(sb1133[i]);
                }
                strtxt = sb1324.ToString();
            }
            return strtxt;
        }

        private string MyPlusDecoder(string restxt)
        {
            string tmprestxt = restxt;
            StringBuilder sbhextobin = new StringBuilder();
            int binlength = tmprestxt.Length;
            for (int i = 0; i < binlength; i++)
            {
                //string ttts1 = tmprestxt.Substring(i, 1);
                //int ttti1 = Convert.ToInt32(ttts1, 16);
                //string ttts2 = Convert.ToString(ttti1, 2).PadLeft(4, '0');
                //sbhextobin.Append(ttts2);
                sbhextobin.Append(Convert.ToString(Convert.ToInt32(tmprestxt.Substring(i, 1), 16), 2).PadLeft(4, '0'));
            }
            string txt444 = sbhextobin.ToString();//16进制密文的二进制字符串
            //第一次反排序 txt+time+randNum 次数 1
            string txt333 = unDesCode(txt444, 1);

            //获取第二次反排序的次数
            string rank111 = txt333.Substring(txt333.Length - 4);
            int rankCount111 = Convert.ToInt32(rank111, 2);

            //第二次反排序 txt+time 次数 rankCount111 
            txt333 = txt333.Substring(0, txt333.Length - 4);
            txt333 = unDesCode(txt333, rankCount111);

            //分离时间戳，进行过期验证
            int tmpsi2 = txt333.Length - 32;//txt二进制长度
            string strTimestamp = txt333.Substring(tmpsi2);
            long encodeTimestamp = Convert.ToInt64(strTimestamp, 2);
            long NowTimestamp = (DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000000;
            if ((NowTimestamp - encodeTimestamp) < 600)//10分钟过期
            {
                //根据时间戳获取第三次反排序次数
                int unDescCounts = (int)(encodeTimestamp % 10);//次数
                int unDescCounts2 = (int)(encodeTimestamp/10 % 10);
                unDescCounts += unDescCounts2;

                //第三次反排序 txt 次数 unDescCounts 
                string txtstr = txt333.Substring(0, tmpsi2);//txt二进制字符串
                txtstr = unDesCode(txtstr, unDescCounts);

                //对二进制结果进行格式化 还原
                StringBuilder sbBinToIntTxt = new StringBuilder();
                for (int i = 0; i < tmpsi2 / 8; i++)
                {
                    sbBinToIntTxt.AppendFormat("{0:d3}", Convert.ToInt32(txtstr.Substring(i * 8, 8), 2));
                }
                int num3 = sbBinToIntTxt.Length;
                byte[] byte3 = new byte[num3 / 3];
                for (int i = 0; i < num3 / 3; i++)
                {
                    byte3[i] = (byte)Convert.ToInt32(sbBinToIntTxt.ToString().Substring(i * 3, 3));
                }
                return Encoding.Unicode.GetString(byte3);
            }
            else
            {
                return "密码已过期";
            }
        }

        #endregion

        //不为空验证
        private bool txtIsNotNull(string text) 
        {
            if (text.Length > 0)
            {
                return true;
            }
            else
            {
                this.txtres.Text = "请先输入明文或密文";
                return false;
            }
        }
