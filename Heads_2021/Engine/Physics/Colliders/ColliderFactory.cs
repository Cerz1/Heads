using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Heads_2021
{
    static class ColliderFactory
    {
        public static CircleCollider CreateCircleFor(GameObject obj, bool innerCircle = true)
        {
            float radius;

            if(innerCircle)
            {
                if(obj.HalfWidth < obj.HalfHeight)
                {
                    radius = obj.HalfWidth;
                }
                else
                {
                    radius = obj.HalfHeight;
                }
            }
            else
            {
                radius = (float)Math.Sqrt(obj.HalfWidth * obj.HalfWidth + obj.HalfHeight * obj.HalfHeight);
            }

            return new CircleCollider(obj.RigidBody, radius);
        }

        public static BoxCollider CreateBoxFor(GameObject obj)
        {
            return new BoxCollider(obj.RigidBody, obj.HalfWidth * 2, obj.HalfHeight * 2);
        }
    }
}
