using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace monogame_spaceinvaders
{
    class SoundEffectObject : GameObject
    {
        private SoundEffectInstance soundEffectInstance;

        public SoundEffectObject(Level level, SoundEffect soundEffect) : base(level)
        {
            this.soundEffectInstance = soundEffect.CreateInstance();
            this.soundEffectInstance.Play();
        }

        public override void Initialize()
        {
            //throw new NotImplementedException();
        }

        public override void LoadContent(ContentManager content)
        {
            //throw new NotImplementedException();
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            //base.Draw(gameTime, spriteBatch);
        }

        public override void Move(GameTime gameTime)
        {
            //throw new NotImplementedException();
        }

        public override void OnCollision(GameObject go, GameObject other)
        {
            //throw new NotImplementedException();
        }

        public override void Update(GameTime gameTime)
        {
            if (soundEffectInstance.State == SoundState.Stopped)
            {
                Level.RemoveGameObject(this);
            }
        }
    }
}
