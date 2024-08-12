using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Heads_2021
{
    class Tile : GameObject
    {
        public Tile(string textureName = "earth", DrawLayer layer = DrawLayer.Playground) : base(textureName)
        {
            RigidBody = new RigidBody(this);
            RigidBody.Type = RigidBodyType.Tile;
            RigidBody.Collider = ColliderFactory.CreateBoxFor(this);
            IsActive = true;
        }
    }
}
