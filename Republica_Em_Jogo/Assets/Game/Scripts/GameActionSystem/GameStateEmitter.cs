using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public static class GameStateEmitter 
    {
        public static event Action<string> Message;

        public static void SendMessage(string message)
        {
            Message?.Invoke(message);
        }


        
    }
}
