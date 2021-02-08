using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Pilipili
{
    class 音乐
    {
        private bool playing = false;

        private Queue<MediaFoundationReader> pool = new Queue<MediaFoundationReader>();

        public void AddMusic(string url) => pool.Enqueue(new MediaFoundationReader(url));

        public void loop()
        {
            while (true)
            {
                if (!playing)
                {
                    if (pool.Count >= 1)
                    {
                        using (var wo = new WaveOutEvent())
                        {
                            playing = true;
                            wo.Init(pool.Dequeue());
                            wo.Play();
                            while (wo.PlaybackState == PlaybackState.Playing)
                            {
                                Thread.Sleep(1000);
                            }
                            playing = false;
                        }
                    }
                    else
                    {
                        Thread.Sleep(1000);
                    }
                }
                else
                {
                    Thread.Sleep(1000);
                }
            }
        }
    }
}
