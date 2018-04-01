using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Audio;

namespace ConversionFPS
{
    static class SoundManager
    {
        static Dictionary<string, SoundEffect> soundEffects = new Dictionary<string, SoundEffect>();

        public static void AddEffect(string soundName, string soundPath)
        {
            soundEffects[soundName] = Main.Instance.Content.Load<SoundEffect>(soundPath);
        }

        public static void Play(string soundName)
        {
            if (soundEffects.ContainsKey(soundName))
                soundEffects[soundName].Play();
        }
    }
}
