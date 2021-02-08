using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Pilipili
{
    public class 本体 : BilibiliDM_PluginFramework.DMPlugin
    {
        public 本体()
        {
            this.Connected += Class1_Connected;
            this.Disconnected += Class1_Disconnected;
            this.ReceivedDanmaku += Class1_ReceivedDanmaku;
            this.ReceivedRoomCount += Class1_ReceivedRoomCount;
            this.PluginAuth = "Xeno";
            this.PluginName = "批哩批哩点歌鸡";
            this.PluginCont = "admin@i-jelly.com";
            this.PluginVer = "v0.0.1";
            this.PluginDesc = "Write by Dickmann(Xeno)";
        }

        private 音乐 player = new 音乐();


        private void Class1_ReceivedRoomCount(object sender, BilibiliDM_PluginFramework.ReceivedRoomCountArgs e)
        {
            this.AddDM($"onRoomCountUpdate=>{e.UserCount}瓦达西摩+1");
        }

        private void Class1_ReceivedDanmaku(object sender, BilibiliDM_PluginFramework.ReceivedDanmakuArgs e)
        {
            if (e.Danmaku.MsgType == BilibiliDM_PluginFramework.MsgTypeEnum.Comment)
            {
                if (e.Danmaku.CommentText.StartsWith("点歌#"))
                {
                    string song = e.Danmaku.CommentText.Substring("点歌".Length + 1).Trim();
                    string json = 网络.Get($"http://music.163.com/api/search/pc?limit=1&type=1&s={song}");
                    try
                    {
                        JObject obj = JObject.Parse(json);
                        string name = obj["result"]["songs"][0]["name"].ToString();
                        string id = obj["result"]["songs"][0]["id"].ToString();
                        player.AddMusic($"http://music.163.com/song/media/outer/url?id={id.Trim()}.mp3");
                        this.AddDM($"已将{name}压入播放队列");
                    }
                    catch
                    {
                        this.AddDM("Err occurred,歌不存在或者USB声卡被踢掉了");
                    }

                }
            }
        }

        private void Class1_Disconnected(object sender, BilibiliDM_PluginFramework.DisconnectEvtArgs e)
        {
            this.AddDM("下Bo滚蛋");
        }

        private void Class1_Connected(object sender, BilibiliDM_PluginFramework.ConnectedEvtArgs e)
        {
            this.AddDM("今日Bo起");
        }

        public override void Admin()
        {
            base.Admin();
            Console.WriteLine("Hello World");
            this.Log("Hello World");
            this.AddDM("Hello World", true);
        }

        public override void Stop()
        {
            base.Stop();
            //請勿使用任何阻塞方法
            Console.WriteLine("Plugin Stoped!");
            this.Log("Plugin Stoped!");
            this.AddDM("我的插♂件萎了", true);
        }

        public override void Start()
        {
            base.Start();
            //請勿使用任何阻塞方法
            Console.WriteLine("Plugin Started!");
            this.Log("Plugin Started!");
            this.AddDM("插件又Bo♂了！", true);
            Task.Run(() => player.loop());
        }
    }
}
