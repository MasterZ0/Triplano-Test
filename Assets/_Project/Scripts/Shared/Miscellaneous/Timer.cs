using System;
using UnityEngine;

namespace TriplanoTest.Shared {
    
    public class Timer {

        public float Progression => Counter / timeInSeconds;
        public event Action OnCompleted = delegate {  };
        public bool IsCompleted => Counter >= timeInSeconds;
        public float Counter { get; private set; }
        
        private float timeInSeconds;

        public void Set(float time) {
            timeInSeconds = time;
            Counter = 0f;
        }

        public void FixedTick() 
        {
            if (IsCompleted) 
                return;

            Counter += Time.fixedDeltaTime; 
            
            if (IsCompleted)            
                OnCompleted();            
        }
    }
}