using Aiv.Fast2D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Heads_2021
{
    enum KeyName { Up, Down, Right, Left, Fire, Jump, LAST}

    struct KeysList
    {
        private KeyCode[] keycodes;

        public KeysList(List<KeyCode> keys)
        {
            keycodes = new KeyCode[(int)KeyName.LAST];

            for (int i = 0; i < keys.Count; i++)
            {
                keycodes[i] = keys[i];
            }
        }

        public void SetKey(KeyName name, KeyCode code)
        {
            keycodes[(int)name] = code;
        }

        public KeyCode GetKey(KeyName name)
        {
            return keycodes[(int)name];
        }
    }
}
