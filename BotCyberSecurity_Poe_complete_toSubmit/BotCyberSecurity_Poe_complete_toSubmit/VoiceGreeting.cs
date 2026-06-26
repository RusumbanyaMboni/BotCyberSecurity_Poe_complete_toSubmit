using System;
using System.IO;
using System.Windows.Forms;
using NAudio.Wave;

namespace CyberSecurityBotGUI
{
    class VoiceGreeting
    {
        public static void PlayGreeting()
        {
            try
            {
                string audioPath = "welcome.wav";

                if (!File.Exists(audioPath))
                {
                    MessageBox.Show("The welcome.wav file was not found. Please make sure it is copied to the output folder.");
                    return;
                }

                using (var audioFile = new AudioFileReader(audioPath))
                using (var outputDevice = new WaveOutEvent())
                {
                    outputDevice.Init(audioFile);
                    outputDevice.Play();

                    while (outputDevice.PlaybackState == PlaybackState.Playing)
                    {
                        System.Threading.Thread.Sleep(100);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Audio greeting could not be played: " + ex.Message);
            }
        }
    }
}