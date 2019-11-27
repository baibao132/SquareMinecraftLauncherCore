using System;
using System.Collections.Generic;
using System.Text;

namespace Gac
{
    public class DownloadProgressListener : IDownloadProgressListener
    {
        DownMsg downMsg = null;
        public DownloadProgressListener(DownMsg downmsg)
        {
            this.downMsg = downmsg;
            //this.id = id;
            //this.Length = Length;
        }
        public delegate void dlgSendMsg(DownMsg msg);
        public dlgSendMsg doSendMsg = null;
        public void OnDownloadSize(long size)
        {
            if (downMsg==null)
            {
                DownMsg downMsg = new DownMsg();
            }


            //下载速度
            if (downMsg.Size == 0)
            {
                downMsg.Speed = size;
            }
            else
            {
                downMsg.Speed = (float)(size - downMsg.Size);
                
            }
            if (downMsg.Speed == 0)
            {
                downMsg.Surplus = -1;
                downMsg.SurplusInfo = "未知";
            }
            else
            {
                downMsg.Surplus = ((downMsg.Length - downMsg.Size) / downMsg.Speed);
            }
            downMsg.Size = size; //下载总量
           
            if (size == downMsg.Length)
            {
                //下载完成
                downMsg.Tag = DownStatus.End;
                downMsg.SpeedInfo = "0 K";
                downMsg.SurplusInfo = "已完成";
            }
            else
            {
                //下载中
                downMsg.Tag = DownStatus.DownLoad;
                

            }
            
            
            if (doSendMsg != null) doSendMsg(downMsg);//通知具体调用者下载进度
        }
    }
    public enum DownStatus
    {
        Start,
        GetLength,
        DownLoad,
        End,
        Error
    }
    public class DownMsg
    {
        private int _Length = 0;
        private string _LengthInfo = "";
        private int _Id = 0;
        private DownStatus _Tag = 0;
        private long _Size = 0;
        private string _SizeInfo = "";
        private float _Speed = 0;
        private float _Surplus = 0;
        private string _SurplusInfo ="";
        private string _ErrMessage = "";
        private string _SpeedInfo = "";
        private double _Progress = 0;

        public int Length
        {
            get
            {
                return _Length;
            }

            set
            {
                _Length = value;
                LengthInfo = GetFileSize(value);
            }
        }

        public int Id
        {
            get
            {
                return _Id;
            }

            set
            {
                _Id = value;
            }
        }
        public DownStatus Tag
        {
            get
            {
                return _Tag;
            }

            set
            {
                _Tag = value;
            }
        }

        public long Size
        {
            get
            {
                return _Size;
            }

            set
            {
                _Size = value;
                SizeInfo = GetFileSize(value);
                if (Length >= value)
                {
                    Progress = Math.Round((double)value / Length * 100, 2);
                }
                else
                {
                    Progress = -1;
                }
            }
        }

        public float Speed
        {
            get
            {
                return _Speed;
            }

            set
            {
                _Speed = value;
                SpeedInfo = GetFileSize(value);
            }
        }
        public string SpeedInfo
        {
            get
            {
                return _SpeedInfo;
            }

            set
            {
                _SpeedInfo = value;
            }
        }

        public float Surplus
        {
            get
            {
                return _Surplus;
            }

            set
            {
                _Surplus = value;
                if (value>0)
                {
                    SurplusInfo = GetDateName((int)Math.Round(value, 0));
                }
                
            }
        }

        public string ErrMessage
        {
            get
            {
                return _ErrMessage;
            }

            set
            {
                _ErrMessage = value;
            }
        }

        public string SizeInfo
        {
            get
            {
                return _SizeInfo;
            }

            set
            {
                _SizeInfo = value;
            }
        }

        public string LengthInfo
        {
            get
            {
                return _LengthInfo;
            }

            set
            {
                _LengthInfo = value;
            }
        }

        public double Progress
        {
            get
            {
                return _Progress;
            }

            set
            {
                _Progress = value;
            }
        }

        public string SurplusInfo
        {
            get
            {
                return _SurplusInfo;
            }

            set
            {
                _SurplusInfo = value;
            }
        }

        private string GetFileSize(float Len)
        {
            float temp = Len;
            string[] sizes = { "B", "KB", "MB", "GB" };
            int order = 0;
            while (temp >= 1024 && order + 1 < sizes.Length)
            {
                order++;
                temp = temp / 1024;
            }
            return String.Format("{0:0.##} {1}", temp, sizes[order]);
        }
        private string GetDateName(int Second)
        {
            float temp = Second;
            string suf = "秒";
            if (Second>60)
            {
                suf = "分钟";
                temp = temp / 60;
                if (Second > 60)
                {
                    suf = "小时";
                    temp = temp / 60;
                    if (Second > 24)
                    {
                        suf = "天";
                        temp = temp / 24;
                        if (Second > 30)
                        {
                            suf = "月";
                            temp = temp / 30;
                            if (Second > 12)
                            {
                                suf = "年";
                                temp = temp / 12;
                            }
                        }
                      
                    }
                   
                }
                
            }
            
            return String.Format("{0:0} {1}", temp, suf);
        }
    }
}