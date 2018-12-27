using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace WizardDuel
{
    class Boundary
    {
        GraphicsDeviceManager graphics;
        ContentManager content;

        Texture2D border;
        Rectangle bounds;
        Vector2 borderLocation;
    }
}
